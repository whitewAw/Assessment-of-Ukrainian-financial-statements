#!/usr/bin/env node
/* eslint-disable no-console */
/**
 * Generate a real 1200x630 Open Graph / Twitter social-share image.
 *
 * Why: Twitter, LinkedIn, Slack, Discord, Telegram and Facebook render
 *      `summary_large_image` previews at 1200x630 (1.91:1). Today the site
 *      uses icon-512.png — which those platforms render as a tiny square,
 *      killing click-through rates from social shares.
 *
 * Usage (after a build):
 *      node tools/prerender/generate-og-image.mjs ./AFS/wwwroot/og-image.png
 *
 *      Then reference it in index.html:
 *          <meta property="og:image" content="https://ua-finance.netlify.app/og-image.png" />
 *          <meta property="og:image:width" content="1200" />
 *          <meta property="og:image:height" content="630" />
 *          <meta name="twitter:image" content="https://ua-finance.netlify.app/og-image.png" />
 *
 * Requires Node 20+ and `puppeteer` (already a devDep of tools/prerender).
 */

import { promises as fs } from 'node:fs';
import path from 'node:path';

const out = path.resolve(process.argv[2] || './AFS/wwwroot/og-image.png');

const html = `<!doctype html>
<html><head><meta charset="utf-8"><style>
  html,body { margin:0; padding:0; width:1200px; height:630px; }
  body {
    font-family: -apple-system, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
    background: linear-gradient(135deg, #052767 0%, #3a0647 70%, #512BD4 100%);
    color: #fff;
    display: flex; flex-direction: column; justify-content: center;
    padding: 80px;
    box-sizing: border-box;
  }
  .badge { display:inline-block; padding:8px 18px; border-radius:999px;
           background:rgba(255,255,255,.12); font-size:22px; letter-spacing:.08em;
           text-transform:uppercase; margin-bottom:30px; width:fit-content; }
  h1 { font-size: 84px; font-weight: 700; line-height: 1.05; margin: 0 0 24px; letter-spacing: -0.02em; }
  p  { font-size: 32px; line-height: 1.35; margin: 0; max-width: 1000px; opacity: .92; }
  .row { display:flex; align-items:center; gap:18px; margin-top:40px; font-size:22px; opacity:.85; }
  .dot { width:8px; height:8px; background:#fff; border-radius:50%; opacity:.6; }
  .brand { position:absolute; top:48px; right:64px; font-weight:700; font-size:36px;
           letter-spacing:.18em; opacity:.9; }
  .stripe { position:absolute; left:0; bottom:0; height:10px; width:100%;
            background: linear-gradient(90deg, #FFD500 0%, #FFD500 50%, #005BBB 50%, #005BBB 100%); }
</style></head>
<body>
  <div class="brand">UFIN</div>
  <span class="badge">Free · Open Source · PWA</span>
  <h1>Ukrainian Financial<br/>Statement Analysis</h1>
  <p>Calculate liquidity, solvency and profitability ratios from balance sheets and income statements — directly in your browser.</p>
  <div class="row">
    <span>17 analysis tables</span><span class="dot"></span>
    <span>7 interactive charts</span><span class="dot"></span>
    <span>15 languages</span><span class="dot"></span>
    <span>On-device AI</span>
  </div>
  <div class="stripe"></div>
</body></html>`;

let puppeteer;
try { puppeteer = (await import('puppeteer')).default; }
catch { console.error('Run inside tools/prerender (puppeteer required): cd tools/prerender && npm install'); process.exit(2); }

const browser = await puppeteer.launch({ headless: 'new', args: ['--no-sandbox'] });
const page = await browser.newPage();
await page.setViewport({ width: 1200, height: 630, deviceScaleFactor: 1 });
await page.setContent(html, { waitUntil: 'networkidle0' });
await fs.mkdir(path.dirname(out), { recursive: true });
await page.screenshot({ path: out, type: 'png', omitBackground: false });
await browser.close();
console.log(`✅ Wrote ${out} (1200x630)`);
