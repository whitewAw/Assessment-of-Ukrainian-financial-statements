import { mkdir, writeFile } from 'node:fs/promises';
import { join } from 'node:path';

const statusPage = 'https://www.visitorbadge.io';
const badges = [
  { file: 'traffic-netlify.svg', label: 'Netlify hits', color: '#00796b', bucket: 'https://ua-finance.netlify.app' },
  { file: 'traffic-github-pages.svg', label: 'GitHub Pages hits', color: '#0969da', bucket: 'https://whitewaw.github.io/Assessment-of-Ukrainian-financial-statements' },
];

async function getTotal(badge, fetchStatus) {
  const query = new URLSearchParams({ path: badge.bucket });
  const response = await fetchStatus(`https://api.visitorbadge.io/api/status?${query}`, {
    headers: { Origin: statusPage, Referer: `${statusPage}/status?${query}` },
    signal: AbortSignal.timeout(15_000),
  });
  if (!response.ok) throw new Error(`${badge.label}: status report returned HTTP ${response.status}`);
  const data = await response.json();
  if (!Number.isSafeInteger(data?.total) || data.total < 0) {
    throw new Error(`${badge.label}: status report did not return a valid total`);
  }
  return data.total;
}

function render({ label, color }, total, date) {
  const count = total.toLocaleString('en-US');
  const left = label.length * 7 + 12;
  const right = count.length * 7 + 12;
  const width = left + right;
  return `<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="20" role="img" aria-labelledby="title">
  <title id="title">${label}: ${count} (snapshot ${date} UTC)</title>
  <clipPath id="round"><rect width="${width}" height="20" rx="3"/></clipPath>
  <g clip-path="url(#round)">
    <rect width="${left}" height="20" fill="#555"/>
    <rect x="${left}" width="${right}" height="20" fill="${color}"/>
  </g>
  <g fill="#fff" text-anchor="middle" font-family="Verdana, sans-serif" font-size="11">
    <text x="${left / 2}" y="14">${label}</text>
    <text x="${left + right / 2}" y="14">${count}</text>
  </g>
</svg>\n`;
}

export async function writeTrafficBadges(output, { fetchStatus = fetch, now = new Date() } = {}) {
  const totals = await Promise.all(badges.map(badge => getTotal(badge, fetchStatus)));
  const date = now.toISOString().slice(0, 10);
  await mkdir(output, { recursive: true });
  await Promise.all(badges.map((badge, index) =>
    writeFile(join(output, badge.file), render(badge, totals[index], date))));
  return totals;
}

if (import.meta.main) {
  const output = process.argv[2];
  if (!output) throw new Error('Usage: node tools/metrics/traffic-badges.mjs <output-directory>');
  const [netlify, pages] = await writeTrafficBadges(output);
  console.log(`Tracked hits: Netlify ${netlify}, GitHub Pages ${pages}`);
}
