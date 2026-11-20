import test from 'node:test';
import assert from 'node:assert/strict';
import { mkdtemp, readdir, readFile, rm } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { writeTrafficBadges } from './traffic-badges.mjs';

async function outputDirectory(t) {
  const output = await mkdtemp(join(tmpdir(), 'afs-traffic-badges-'));
  t.after(() => rm(output, { recursive: true }));
  return output;
}

const netlify = 'https://ua-finance.netlify.app';
const pages = 'https://whitewaw.github.io/Assessment-of-Ukrainian-financial-statements';

test('reads both production totals without sending counting requests and renders them as SVG', async t => {
  const output = await outputDirectory(t);
  const requests = [];
  const fetchStatus = async (input, options) => {
    const url = new URL(input);
    const bucket = url.searchParams.get('path');
    requests.push([url.origin, url.pathname, bucket]);
    assert.equal(options.headers.Origin, 'https://www.visitorbadge.io');
    return Response.json({ total: bucket === netlify ? 1234 : 0 });
  };

  await writeTrafficBadges(output, { fetchStatus, now: new Date('2026-11-20T19:00:00Z') });
  assert.deepEqual(requests, [
    ['https://api.visitorbadge.io', '/api/status', netlify],
    ['https://api.visitorbadge.io', '/api/status', pages],
  ]);
  assert.deepEqual((await readdir(output)).sort(), ['traffic-github-pages.svg', 'traffic-netlify.svg']);
  assert.match(await readFile(join(output, 'traffic-netlify.svg'), 'utf8'), /Netlify hits: 1,234 \(snapshot 2026-11-20 UTC\)[\s\S]*>1,234<\/text>/);
  assert.match(await readFile(join(output, 'traffic-github-pages.svg'), 'utf8'), /GitHub Pages hits: 0 \(snapshot 2026-11-20 UTC\)[\s\S]*>0<\/text>/);
});

for (const [failure, expected] of [
  [() => new Response(null, { status: 403 }), /GitHub Pages hits: status report returned HTTP 403/],
  [() => Response.json({ today: 10 }), /GitHub Pages hits: status report did not return a valid total/],
]) {
  test(`does not write a partial snapshot when a report fails: ${expected.source}`, async t => {
    const output = await outputDirectory(t);
    const fetchStatus = async input => new URL(input).searchParams.get('path') === netlify
      ? Response.json({ total: 1234 }) : failure();
    await assert.rejects(writeTrafficBadges(output, { fetchStatus }), expected);
    assert.deepEqual(await readdir(output), []);
  });
}
