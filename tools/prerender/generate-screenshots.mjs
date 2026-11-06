#!/usr/bin/env node
/* eslint-disable no-console */
/**
 * Generates PWA manifest screenshots (wide 1280x720, narrow 540x960) by loading
 * the published app in headless Chromium.
 *
 * Usage:
 *   node tools/prerender/generate-screenshots.mjs <publishedWwwrootDir> [<extraOutputDir> ...]
 *
 * Files written to every directory: screenshot-wide.png, screenshot-narrow.png
 */

import { promises as fs, existsSync } from 'node:fs';
import path from 'node:path';
import http from 'node:http';

const [wwwroot, ...extraOutDirs] = process.argv.slice(2);
if (!wwwroot) {
  console.error('Usage: generate-screenshots.mjs <wwwroot> [<extraOutputDir> ...]');
  process.exit(2);
}
const root = path.resolve(wwwroot);
const outDirs = [root, ...extraOutDirs.map(d => path.resolve(d))];

const MIME = {
  '.html': 'text/html; charset=utf-8', '.js': 'application/javascript; charset=utf-8',
  '.mjs': 'application/javascript; charset=utf-8', '.css': 'text/css; charset=utf-8',
  '.json': 'application/json; charset=utf-8', '.wasm': 'application/wasm',
  '.png': 'image/png', '.svg': 'image/svg+xml', '.ico': 'image/x-icon',
  '.woff': 'font/woff', '.woff2': 'font/woff2',
};

function startServer() {
  return new Promise(resolve => {
    const server = http.createServer(async (req, res) => {
      try {
        const rel = decodeURIComponent(new URL(req.url, 'http://localhost').pathname);
        let filePath = path.join(root, rel);
        if (!existsSync(filePath) || (await fs.stat(filePath)).isDirectory()) {
          if (path.extname(rel) !== '') { res.statusCode = 404; res.end(); return; }
          filePath = path.join(root, 'index.html');
        }
        res.setHeader('Content-Type', MIME[path.extname(filePath).toLowerCase()] || 'application/octet-stream');
        res.end(await fs.readFile(filePath));
      } catch (err) {
        res.statusCode = 500; res.end(String(err));
      }
    });
    server.listen(0, '127.0.0.1', () => resolve(server));
  });
}

const shots = [
  { file: 'screenshot-wide.png', width: 1280, height: 720, mobile: false },
  { file: 'screenshot-narrow.png', width: 540, height: 960, mobile: true },
];

async function main() {
  const puppeteer = (await import('puppeteer')).default;
  const server = await startServer();
  const url = `http://127.0.0.1:${server.address().port}/`;
  const browser = await puppeteer.launch({
    headless: 'new',
    args: ['--no-sandbox', '--disable-setuid-sandbox', '--disable-dev-shm-usage'],
  });

  try {
    for (const shot of shots) {
      const page = await browser.newPage();
      await page.setViewport({ width: shot.width, height: shot.height, isMobile: shot.mobile, deviceScaleFactor: 1 });
      await page.goto(url, { waitUntil: 'networkidle2', timeout: 90_000 });
      await page.waitForFunction(() => {
        const app = document.getElementById('app');
        return app && !app.querySelector('.spinner-border') && app.innerText.length > 200;
      }, { timeout: 60_000 }).catch(() => { /* capture whatever rendered */ });
      await new Promise(r => setTimeout(r, 1000));

      const buffer = await page.screenshot({ type: 'png' });
      for (const dir of outDirs) {
        await fs.writeFile(path.join(dir, shot.file), buffer);
      }
      console.log(`✅ ${shot.file} (${shot.width}x${shot.height})`);
      await page.close();
    }
  } finally {
    await browser.close();
    await new Promise(r => server.close(r));
  }
}

main().catch(err => { console.error(err); process.exit(1); });
