import test from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { sitemapLocations, isRouteReady } from './prerender-policy.mjs';

function documentFixture({ text = 'Rendered route content. '.repeat(20), heading = true, hasMain = true, title = 'UFIN' } = {}) {
  const main = {
    innerText: text,
    querySelector: selector => selector === 'h1, h2' && heading ? {} : null,
  };
  const app = {
    querySelector: selector => selector === 'main' ? (hasMain ? main : null) : {},
  };
  return { title, getElementById: id => id === 'app' ? app : null };
}

test('Sitemap discovery ignores commented routes and image locations', () => {
  const xml = `<urlset>
    <url><loc>https://ua-finance.netlify.app/</loc><image:loc>https://ua-finance.netlify.app/og-image.png</image:loc></url>
    <!-- <url><loc>https://ua-finance.netlify.app/help</loc></url>
         <url><loc>https://ua-finance.netlify.app/about</loc></url> -->
    <url><loc> https://ua-finance.netlify.app/aiassistant </loc></url>
  </urlset>`;
  assert.deepEqual(sitemapLocations(xml), [
    'https://ua-finance.netlify.app/',
    'https://ua-finance.netlify.app/aiassistant',
  ]);
});

test('Repository sitemap excludes disabled help and about pages', () => {
  const xml = readFileSync(new URL('../../AFS/wwwroot/sitemap.xml', import.meta.url), 'utf8');
  const routes = sitemapLocations(xml).map(url => new URL(url).pathname);
  assert.ok(routes.includes('/'));
  assert.ok(routes.includes('/aiassistant'));
  assert.ok(!routes.includes('/help'));
  assert.ok(!routes.includes('/about'));
});

test('Ready content is not blocked by an optional widget spinner', () => {
  const doc = documentFixture();
  assert.ok(doc.getElementById('app').querySelector('.spinner-border'));
  assert.equal(isRouteReady(doc), true);
});

test('Pre-hydration shell is not ready even when it has headings', () => {
  assert.equal(isRouteReady(documentFixture({ hasMain: false })), false);
});

test('Empty or short route content is not ready', () => {
  for (const text of ['', 'Loading', 'x'.repeat(200)]) {
    assert.equal(isRouteReady(documentFixture({ text })), false);
  }
});

test('Rendered content must have a heading inside main', () => {
  assert.equal(isRouteReady(documentFixture({ heading: false })), false);
});

test('Missing app is not ready', () => {
  assert.equal(isRouteReady({ title: 'UFIN', getElementById: () => null }), false);
});

test('NotFound content is rejected even if it is long enough', () => {
  assert.throws(() => isRouteReady(documentFixture({ title: 'Page Not Found' })), /NotFound/);
});
