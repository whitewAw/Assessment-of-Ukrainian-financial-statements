#!/usr/bin/env node
/* eslint-disable no-console */
/**
 * Build-time prerender for the AFS Blazor WebAssembly app.
 *
 * Why:
 *   Blazor WASM only renders after the .NET runtime + app DLLs download and execute.
 *   Bingbot, Yandex, Baidu and most AI crawlers (GPTBot, ClaudeBot, PerplexityBot, etc.)
 *   do *not* execute JavaScript. Without prerendering they only see the loading spinner.
 *
 * What this does:
 *   1. Spins up a static HTTP server on the published wwwroot.
 *   2. Discovers every route from sitemap.xml (and falls back to a hardcoded list).
 *   3. Loads each route in headless Chromium, waits for Blazor to hydrate
 *      and the .blazor-loaded sentinel (or a 6s ceiling).
 *   4. Captures the rendered DOM and writes <route>/index.html so static-file hosts
 *      (GitHub Pages, Netlify) serve the prerendered HTML to crawlers.
 *
 * Usage:
 *   node tools/prerender/prerender.mjs <publishedWwwrootDir> [--baseHref=/] [--origin=https://ua-finance.netlify.app]
 *
 * Requires Node 20+ and `puppeteer` (devDependency, installed by CI).
 */

import { promises as fs } from 'node:fs';
import { existsSync } from 'node:fs';
import path from 'node:path';
import http from 'node:http';
import { fileURLToPath, pathToFileURL } from 'node:url';

const args = process.argv.slice(2);
const wwwroot = args.find(a => !a.startsWith('--'));
if (!wwwroot) {
  console.error('Usage: prerender.mjs <wwwroot> [--baseHref=/] [--origin=https://ua-finance.netlify.app]');
  process.exit(2);
}
const baseHref = (args.find(a => a.startsWith('--baseHref=')) || '--baseHref=/').split('=')[1];
const canonicalOrigin = (args.find(a => a.startsWith('--origin=')) || '--origin=https://ua-finance.netlify.app').split('=')[1];
const root = path.resolve(wwwroot);
if (!existsSync(root)) {
  console.error(`wwwroot not found: ${root}`);
  process.exit(2);
}

// --- Discover routes ----------------------------------------------------------

async function discoverRoutes() {
  const sitemapPath = path.join(root, 'sitemap.xml');
  const fallback = [
    '/', '/aiassistant', '/ai-assistant',
    '/liquidityindicatorsofbalance', '/solvencyratios',
    '/generalindicatorsoffinancialstability', '/indicatorsofbusinessactivity',
    '/characteristicsofcapital', '/indicatorsofturnoverofcurrentassets',
    '/factorsaffectingturnoverofworkingcapital', '/indicatorsofefficiencyofworkingcapital',
    '/availabilityandmovementoffixedassets', '/indicatorsofstateandmovementoffixedassets',
    '/calculationofindicatorsofefficiencyofuseoffixedassets', '/factoranalysisoffixedassets',
    '/indicatorsofefficiencyofuseofintangibleassets', '/sourcesofcapitalformation',
    '/assessmentofreceivableandpayable',
    '/indicatorsoffinancialstability', '/classificationoftypesoffinancialstability',
  ];
  if (!existsSync(sitemapPath)) return fallback;
  const xml = await fs.readFile(sitemapPath, 'utf8');
  const locs = [...xml.matchAll(/<loc>([^<]+)<\/loc>/g)].map(m => m[1]);
  if (!locs.length) return fallback;
  const routes = new Set();
  for (const url of locs) {
    try {
      const u = new URL(url);
      let p = u.pathname;
      // Strip the GitHub Pages path prefix if present
      p = p.replace(/^\/Assessment-of-Ukrainian-financial-statements/, '');
      if (!p) p = '/';
      routes.add(p);
    } catch { /* ignore malformed */ }
  }
  return [...routes];
}

// --- Static server ------------------------------------------------------------

const MIME = {
  '.html': 'text/html; charset=utf-8',
  '.js': 'application/javascript; charset=utf-8',
  '.mjs': 'application/javascript; charset=utf-8',
  '.css': 'text/css; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.wasm': 'application/wasm',
  '.png': 'image/png', '.jpg': 'image/jpeg', '.svg': 'image/svg+xml',
  '.ico': 'image/x-icon', '.webp': 'image/webp',
  '.woff': 'font/woff', '.woff2': 'font/woff2', '.ttf': 'font/ttf',
  '.dll': 'application/octet-stream', '.dat': 'application/octet-stream',
  '.br': 'application/octet-stream', '.gz': 'application/gzip',
};

function startServer() {
  return new Promise(resolve => {
    const server = http.createServer(async (req, res) => {
      try {
        const url = new URL(req.url, 'http://localhost');
        let rel = decodeURIComponent(url.pathname);
        // SPA fallback: anything without an extension serves index.html
        let filePath = path.join(root, rel);
        if (!existsSync(filePath) || (await fs.stat(filePath)).isDirectory()) {
          if (path.extname(rel) === '') {
            filePath = path.join(root, 'index.html');
          } else {
            res.statusCode = 404; res.end('Not found'); return;
          }
        }
        const ext = path.extname(filePath).toLowerCase();
        res.setHeader('Content-Type', MIME[ext] || 'application/octet-stream');
        const data = await fs.readFile(filePath);
        res.end(data);
      } catch (err) {
        res.statusCode = 500; res.end(String(err));
      }
    });
    server.listen(0, '127.0.0.1', () => resolve(server));
  });
}

// --- Prerender ----------------------------------------------------------------

async function main() {
  const routes = await discoverRoutes();
  console.log(`📄 Prerendering ${routes.length} route(s) from ${root}`);
  console.log(`   Routes: ${routes.join(', ')}`);

  let puppeteer;
  try {
    puppeteer = (await import('puppeteer')).default;
  } catch {
    console.error('❌ puppeteer is not installed. Run: npm i -D puppeteer');
    process.exit(2);
  }

  const server = await startServer();
  const port = server.address().port;
  const browser = await puppeteer.launch({
    headless: 'new',
    args: ['--no-sandbox', '--disable-setuid-sandbox', '--disable-dev-shm-usage'],
  });

  let succeeded = 0, failed = 0;
  for (const route of routes) {
    const url = `http://127.0.0.1:${port}${route}`;
    const page = await browser.newPage();
    await page.setUserAgent('Mozilla/5.0 (compatible; UFIN-Prerender/1.0)');
    try {
      await page.goto(url, { waitUntil: 'networkidle2', timeout: 60_000 });

      // Wait for Blazor to render real content (the loading spinner is replaced)
      await page.waitForFunction(() => {
        const app = document.getElementById('app');
        if (!app) return false;
        // Spinner gone OR a route component rendered something meaningful
        const hasSpinner = app.querySelector('.spinner-border');
        const hasContent = app.innerText && app.innerText.length > 200;
        return !hasSpinner || hasContent;
      }, { timeout: 30_000 }).catch(() => { /* fall through with whatever we have */ });

      // Small settle delay for late-binding components (Radzen tabs etc.)
      await new Promise(r => setTimeout(r, 800));

      let html = await page.content();

      // Blazor emits marker comments like <!--!--> around render fragments.
      // They are harmless in the live DOM but add noise to prerendered HTML
      // consumed by crawlers and social-preview bots.
      html = html.replace(/<!--!-->/g, '');

      // Force canonical to the production origin (server runs on localhost)
      html = html.replace(/http:\/\/127\.0\.0\.1:\d+/g, canonicalOrigin.replace(/\/$/, ''));

      // Make sure <base> matches the deployment baseHref
      html = html.replace(/<base href="[^"]*"\s*\/?>/i, `<base href="${baseHref}" />`);

      // Tag the snapshot for debuggability
      html = html.replace('</head>', `<meta name="prerender-rendered" content="${new Date().toISOString()}" /></head>`);

      const outDir = route === '/' ? root : path.join(root, route.replace(/^\//, ''));
      await fs.mkdir(outDir, { recursive: true });
      const outFile = path.join(outDir, 'index.html');

      // Don't overwrite the original root index.html for "/" — write to root only if missing,
      // but for "/" we DO want to replace it so crawlers landing on / see prerendered HTML.
      await fs.writeFile(outFile, html, 'utf8');

      succeeded++;
      console.log(`   ✅ ${route} -> ${path.relative(root, outFile)}`);
    } catch (err) {
      failed++;
      console.warn(`   ⚠️  ${route} -> ${err.message}`);
    } finally {
      await page.close();
    }
  }

  await browser.close();
  await new Promise(r => server.close(r));

  console.log(`\n📊 Prerender complete: ${succeeded} ok, ${failed} failed`);
  // Don't fail the build on individual route failures — partial prerender is still valuable.
  process.exit(0);
}

main().catch(err => { console.error(err); process.exit(1); });
