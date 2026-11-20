import test from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { runInNewContext } from 'node:vm';
import { normalizeSnapshot, validateSnapshot } from './seo-output.mjs';

const route = 'https://ua-finance.netlify.app/indicatorsoffinancialstability';
const source = `<!doctype html><html><head>
<base href="/" />
<link rel="canonical" href="https://ua-finance.netlify.app/" id="canonical">
<link rel="canonical">
<link href="https://whitewaw.github.io/" rel="canonical">
<link rel="alternate" hreflang="es" href="https://ua-finance.netlify.app/?lang=es">
<link rel="sitemap" href="/sitemap.xml">
<meta name="robots" content="index, follow">
<meta name="googlebot" content="index, follow">
<meta name="prerender-rendered" content="old">
<meta name="prerender-rendered" content="older">
<link rel="stylesheet" href="css/app.css">
</head><body><main><h1>Financial stability indicators</h1><table><tr><td>Example</td></tr></table></main></body></html>`;

for (const baseHref of ['/', '/Assessment-of-Ukrainian-financial-statements/']) {
  test(`Normalizes duplicate canonical tags for ${baseHref}`, () => {
    const html = normalizeSnapshot(source, route, baseHref);
    assert.doesNotThrow(() => validateSnapshot(html, route, baseHref));
  });

  test(`Keeps base before relative resources for ${baseHref}`, () => {
    const html = normalizeSnapshot(source, route, baseHref);
    assert.ok(html.indexOf('<base ') < html.indexOf('<link rel="stylesheet"'));
  });

  test(`Does not accumulate metadata on repeated normalization for ${baseHref}`, () => {
    const html = normalizeSnapshot(normalizeSnapshot(source, route, baseHref), route, baseHref);
    assert.doesNotThrow(() => validateSnapshot(html, route, baseHref));
  });
}

test('Mirror does not advertise an absent sitemap', () => {
  const html = normalizeSnapshot(source, route, '/Assessment-of-Ukrainian-financial-statements/');
  assert.doesNotMatch(html, /rel="sitemap"/);
});

test('Canonical host retains sitemap discovery', () => {
  assert.match(normalizeSnapshot(source, route, '/'), /rel="sitemap"/);
});

test('Homepage retains its trailing slash', () => {
  const home = 'https://ua-finance.netlify.app/';
  assert.doesNotThrow(() => validateSnapshot(normalizeSnapshot(source, home, '/'), home, '/'));
});

for (const url of [`${route}?lang=es`, `${route}#fragment`, 'https://whitewaw.github.io/']) {
  test(`Rejects noncanonical target ${url}`, () => {
    assert.throws(() => normalizeSnapshot(source, url, '/'), /Invalid canonical URL/);
  });
}

test('Rejects an unrendered shell', () => {
  const html = normalizeSnapshot('<html><head></head><body><div id="app">Loading</div></body></html>', route, '/');
  assert.throws(() => validateSnapshot(html, route, '/'), /Missing rendered route content/);
});

test('Rejects an additional empty canonical', () => {
  const html = normalizeSnapshot(source, route, '/').replace('</head>', '<link rel="canonical"></head>');
  assert.throws(() => validateSnapshot(html, route, '/'), /exactly one canonical/);
});

const mirror = 'https://whitewaw.github.io/Assessment-of-Ukrainian-financial-statements/';
const errorPage = readFileSync(new URL('../../AFS/wwwroot/404.html', import.meta.url), 'utf8');
const errorPageScript = errorPage.match(/<script>([\s\S]*?)<\/script>/)[1];

function runErrorPage(url) {
  const calls = [];
  const location = new URL(url);
  location.replace = target => calls.push(['redirect', target]);
  runInNewContext(errorPageScript, {
    location,
    sessionStorage: { setItem: (key, value) => calls.push(['store', key, value]) },
  });
  return calls;
}

test('Netlify error page stays on the requested URL and is noindex', () => {
  assert.deepEqual(runErrorPage('https://ua-finance.netlify.app/not-a-page'), []);
  assert.match(errorPage, /<meta name="robots" content="noindex, follow"/);
  assert.match(errorPage, /<h1>Page not found<\/h1>/);
});

test('GitHub Pages error fallback preserves the requested route, query and fragment', () => {
  const base = new URL(mirror).pathname;
  assert.deepEqual(runErrorPage(`${mirror}ai-assistant?lang=en#details`), [
    ['store', 'ufin-redirect-path', `${base}ai-assistant?lang=en#details`],
    ['redirect', base],
  ]);
});

const worker = readFileSync(new URL('../../AFS/wwwroot/service-worker.published.js', import.meta.url), 'utf8');

async function runWorkerNavigation(url, routeResponse) {
  const shell = new Response('SPA shell');
  const requests = [];
  const backgroundTasks = [];
  const response = await runInNewContext(`${worker}\nhandleNavigationRequest(request, backgroundTasks);`, {
    self: { importScripts() {}, addEventListener() {}, assetsManifest: { version: 'test' } },
    URL,
    Request: class extends Request { constructor(path, options) { super(new URL(path, url), options); } },
    request: new Request(url),
    backgroundTasks,
    caches: { open: async () => ({ put: async () => {}, match: async () => shell }) },
    fetch: async request => {
      requests.push(new URL(request.url).pathname);
      if (request.url !== url) return shell;
      if (!routeResponse) throw new TypeError('Network unavailable');
      return routeResponse;
    },
    console: { info() {} },
  });
  await Promise.all(backgroundTasks);
  return { response, requests };
}

test('Service worker passes through Netlify 404 without requesting the homepage', async () => {
  const notFound = new Response('Page not found', { status: 404 });
  const result = await runWorkerNavigation('https://ua-finance.netlify.app/not-a-page', notFound);
  assert.equal(result.response, notFound);
  assert.deepEqual(result.requests, ['/not-a-page']);
});

test('Service worker still serves the SPA shell for GitHub Pages fallback and offline navigation', async () => {
  const github = await runWorkerNavigation(`${mirror}ai-assistant`, new Response(null, { status: 404 }));
  assert.equal(await github.response.text(), 'SPA shell');
  assert.deepEqual(github.requests, [new URL(`${mirror}ai-assistant`).pathname, new URL(`${mirror}index.html`).pathname]);
  const offline = await runWorkerNavigation('https://ua-finance.netlify.app/aiassistant');
  assert.equal(await offline.response.text(), 'SPA shell');
  assert.deepEqual(offline.requests, ['/aiassistant']);
});
