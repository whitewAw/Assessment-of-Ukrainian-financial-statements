import test from 'node:test';
import assert from 'node:assert/strict';
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
