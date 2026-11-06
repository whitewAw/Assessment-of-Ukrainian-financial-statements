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
 *      and meaningful route content.
 *   4. Captures the rendered DOM and writes <route>.html so static-file hosts
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
import { normalizeSnapshot, validateSnapshot } from './seo-output.mjs';
import { sitemapLocations, isRouteReady } from './prerender-policy.mjs';

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

function normalizeRoute(route) {
  if (!route || route === '/') return '/';
  return `/${route.replace(/^\/+/, '').replace(/\/+$/, '')}`;
}

// --- Discover routes ----------------------------------------------------------

async function discoverRoutes() {
  const sitemapPath = path.join(root, 'sitemap.xml');
  const fallback = [
    '/', '/aiassistant',
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
  const locs = sitemapLocations(xml);
  if (!locs.length) return fallback;
  const routes = new Set();
  for (const url of locs) {
    try {
      const u = new URL(url);
      let p = u.pathname;
      // Strip the GitHub Pages path prefix if present
      p = p.replace(/^\/Assessment-of-Ukrainian-financial-statements/, '');
      routes.add(normalizeRoute(p));
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

function startServer(shell) {
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
        // Every route must boot from the original shell, never a captured DOM
        // containing Blazor's old head markers and another route's content.
        if (filePath === path.join(root, 'index.html')) {
          res.setHeader('Content-Type', MIME['.html']);
          res.end(shell);
          return;
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

  const shell = await fs.readFile(path.join(root, 'index.html'), 'utf8');
  const server = await startServer(shell);
  const port = server.address().port;
  let browser;
  try {
    browser = await puppeteer.launch({
      headless: 'new',
      dumpio: true,
      args: ['--no-sandbox', '--disable-setuid-sandbox', '--disable-dev-shm-usage'],
    });
    browser.on('disconnected', () => console.error('   Chromium DevTools connection disconnected'));
    browser.process()?.on('exit', (code, signal) => {
      console.error(`   Chromium exited: code=${code}, signal=${signal}`);
    });
    await renderRoutes(browser, port, routes);
  } finally {
    try {
      await browser?.close();
    } finally {
      await new Promise(r => server.close(r));
    }
  }
}

async function renderRoutes(browser, port, routes) {
  let succeeded = 0, failed = 0;
  for (const route of routes) {
    const normalizedRoute = normalizeRoute(route);
    const url = `http://127.0.0.1:${port}${normalizedRoute}`;
    let context, page;
    try {
      // Release route-specific workers, storage and targets with each context.
      context = await browser.createBrowserContext();
      page = await context.newPage();
      page.on('pageerror', error => console.error(`   [${route}] Browser error: ${error.message}`));
      page.on('console', message => {
        if (message.type() === 'error') console.error(`   [${route}] Console: ${message.text()}`);
      });
      page.on('requestfailed', request => {
        console.error(`   [${route}] Request failed: ${request.url()} (${request.failure()?.errorText})`);
      });
      page.on('response', response => {
        if (response.status() >= 400) console.error(`   [${route}] HTTP ${response.status()}: ${response.url()}`);
      });
      await page.setUserAgent('Mozilla/5.0 (compatible; UFIN-Prerender/1.0)');
      await page.goto(url, { waitUntil: 'networkidle2', timeout: 60_000 });
      // Optional AI widgets may still be loading after route content is ready.
      await page.waitForFunction(isRouteReady, { timeout: 30_000 });

      // Small settle delay for late-binding components (Radzen tabs etc.)
      await new Promise(r => setTimeout(r, 800));

      const failedApp = await page.evaluate(() => {
        const error = document.getElementById('blazor-error-ui');
        return error && getComputedStyle(error).display !== 'none';
      });
      if (failedApp) throw new Error('Blazor error UI is visible');

      let html = await page.content();

      // Blazor emits marker comments like <!--!--> around render fragments.
      // They are harmless in the live DOM but add noise to prerendered HTML
      // consumed by crawlers and social-preview bots.
      html = html.replace(/<!--!-->/g, '');

      // Force canonical to the production origin (server runs on localhost)
      html = html.replace(/http:\/\/127\.0\.0\.1:\d+/g, canonicalOrigin.replace(/\/$/, ''));

      // Make sure <base> matches the deployment baseHref
      html = html.replace(/<base href="[^"]*"\s*\/?>/i, `<base href="${baseHref}" />`);

      // Rewrite per-page social URLs to the *current* route. The static
      // index.html ships with `og:url`/`twitter:url`/`<link rel="canonical">`
      // pointing at the site root, which is correct for "/" but wrong for
      // every sub-route after prerender. Social-share crawlers
      // (facebookexternalhit, Twitterbot, LinkedInBot, Discordbot, Slackbot)
      // read these tags verbatim and would otherwise show the homepage card
      // for deep links. Google also uses og:url as a strong canonical hint.
      const originNoSlash = canonicalOrigin.replace(/\/$/, '');
      const absoluteRouteUrl = normalizedRoute === '/' ? `${originNoSlash}/` : `${originNoSlash}${normalizedRoute}`;
      html = html.replace(
        /<meta\s+property="og:url"\s+content="[^"]*"\s*\/?>/i,
        `<meta property="og:url" content="${absoluteRouteUrl}" />`
      );
      html = html.replace(
        /<meta\s+name="twitter:url"\s+content="[^"]*"\s*\/?>/i,
        `<meta name="twitter:url" content="${absoluteRouteUrl}" />`
      );
      html = normalizeSnapshot(html, absoluteRouteUrl, baseHref);
      validateSnapshot(html, absoluteRouteUrl, baseHref);

      // Write flat `<route>.html` files (not `<route>/index.html`). Both Netlify
      // and GitHub Pages serve `/route` from `route.html` with a 200, whereas a
      // `route/` directory makes the host 301 `/route` -> `/route/`, which
      // conflicts with our no-slash canonical and yields GSC "Redirect error".
      const outFile = normalizedRoute === '/'
        ? path.join(root, 'index.html')
        : path.join(root, `${normalizedRoute.replace(/^\//, '')}.html`);
      await fs.mkdir(path.dirname(outFile), { recursive: true });
      await fs.writeFile(outFile, html, 'utf8');

      succeeded++;
      console.log(`   ✅ ${normalizedRoute} -> ${path.relative(root, outFile)}`);
    } catch (err) {
      failed++;
      console.warn(`   ⚠️  ${route} -> ${err.message}`);
      console.error(err.stack || String(err));
      try {
        if (!page) throw new Error('Browser failed before page creation');
        const state = await page.evaluate(() => {
          const main = document.querySelector('#app main');
          const error = document.getElementById('blazor-error-ui');
          return {
            url: location.href,
            title: document.title,
            baseHref: document.querySelector('base')?.href,
            mainTextLength: main?.innerText.trim().length || 0,
            heading: main?.querySelector('h1, h2')?.textContent,
            blazorErrorVisible: Boolean(error && getComputedStyle(error).display !== 'none'),
          };
        });
        console.error(`   Route state: ${JSON.stringify(state)}`);
        const diagnostics = path.resolve('prerender-diagnostics', baseHref === '/' ? 'netlify' : 'ghpages');
        await fs.mkdir(diagnostics, { recursive: true });
        const filename = path.join(diagnostics, encodeURIComponent(normalizedRoute));
        await fs.writeFile(`${filename}.html`, await page.content(), 'utf8');
        await page.screenshot({ path: `${filename}.png`, fullPage: true });
        console.error(`   Failure artifacts: ${filename}.{html,png}`);
      } catch (diagnosticError) {
        console.error(`   Could not capture diagnostics: ${diagnosticError.message}`);
      }
    } finally {
      try {
        await context?.close();
      } catch (closeError) {
        failed++;
        console.error(`   [${route}] Context cleanup failed: ${closeError.stack || closeError.message}`);
      }
    }
    if (!browser.connected) {
      throw new Error(`Chromium disconnected after ${route}; remaining routes were not rendered`);
    }
  }

  console.log(`\n📊 Prerender complete: ${succeeded} ok, ${failed} failed`);
  // A missing snapshot silently serves the generic shell to search engines.
  process.exitCode = failed > 0 ? 1 : 0;
}

main().catch(err => { console.error(err); process.exit(1); });
