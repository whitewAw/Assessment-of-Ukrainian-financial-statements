import test from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { runInNewContext } from 'node:vm';

const html = readFileSync(new URL('../../AFS/wwwroot/index.html', import.meta.url), 'utf8');
const readme = readFileSync(new URL('../../README.md', import.meta.url), 'utf8');
const script = html.match(/id="vbadge"[^>]*>\s*<script>([\s\S]*?)<\/script>/)[1];
const netlify = 'https://ua-finance.netlify.app';
const pages = 'https://whitewaw.github.io/Assessment-of-Ukrainian-financial-statements';

function pixelRequests(url) {
  const requests = [];
  runInNewContext(script, {
    location: new URL(url),
    document: {
      getElementById(id) {
        assert.equal(id, 'vbadge');
        return { set src(value) { requests.push(new URL(value)); } };
      },
    },
  });
  return requests;
}

for (const bucket of [netlify, pages]) {
  test(`Homepage and deep route share exactly one traffic bucket on ${bucket}`, () => {
    for (const route of ['/', '/aiassistant?lang=uk#example']) {
      const requests = pixelRequests(bucket + route);
      assert.deepEqual(requests.map(url => [url.origin, url.pathname, url.searchParams.get('path')]), [
        ['https://api.visitorbadge.io', '/api/combined', bucket],
      ]);
    }
  });
}

test('Development, previews, and unrelated Pages paths do not request the pixel', () => {
  for (const url of [
    'http://localhost:5000/',
    'http://127.0.0.1:5000/',
    'https://deploy-preview-42--ua-finance.netlify.app/',
    'https://whitewaw.github.io/',
    `${pages}-other/`,
  ]) {
    assert.deepEqual(pixelRequests(url), [], url);
  }
});

test('README links to both reports without embedding counting images', () => {
  const reports = [...readme.matchAll(/\]\((https:\/\/www\.visitorbadge\.io\/status\?[^)]+)\)/g)];
  assert.deepEqual(reports.map(([, url]) => new URL(url).searchParams.get('path')).sort(), [netlify, pages].sort());
  assert.doesNotMatch(readme, /api\.visitorbadge\.io|komarev\.com\/ghpvc/);
});
