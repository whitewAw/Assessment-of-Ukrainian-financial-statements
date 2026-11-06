const canonicalOrigin = 'https://ua-finance.netlify.app';

function attribute(tag, name) {
  return tag.match(new RegExp(`\\b${name}\\s*=\\s*["']([^"']*)["']`, 'i'))?.[1];
}

export function normalizeSnapshot(html, canonicalUrl, baseHref) {
  const url = new URL(canonicalUrl);
  if (url.origin !== canonicalOrigin || url.search || url.hash) {
    throw new Error(`Invalid canonical URL: ${canonicalUrl}`);
  }
  const robots = baseHref === '/' ? 'index, follow' : 'noindex, follow';
  return html.replace(/<head\b[^>]*>[\s\S]*?<\/head>/i, head => {
    head = head.replace(/<link\b[^>]*>/gi, tag => {
      const rel = attribute(tag, 'rel');
      if (rel === 'canonical' || attribute(tag, 'hreflang')) return '';
      if (rel === 'sitemap' && baseHref !== '/') return '';
      return tag;
    });
    head = head.replace(/<meta\b[^>]*>/gi, tag => {
      const name = attribute(tag, 'name');
      return ['robots', 'googlebot', 'prerender-rendered'].includes(name) ? '' : tag;
    });
    head = head.replace(/<base\b[^>]*>/gi, '');
    head = head.replace(/<head\b[^>]*>/i, tag => `${tag}\n<base href="${baseHref}" />`);
    return head.replace(/<\/head>/i,
      `<link rel="canonical" href="${canonicalUrl}" id="canonical" />\n` +
      `<link rel="alternate" hreflang="x-default" href="${canonicalUrl}" />\n` +
      `<meta name="robots" content="${robots}, max-image-preview:large" />\n` +
      `<meta name="googlebot" content="${robots}, max-image-preview:large" />\n` +
      `<meta name="prerender-rendered" content="${new Date().toISOString()}" />\n</head>`);
  });
}

export function validateSnapshot(html, canonicalUrl, baseHref) {
  const head = html.match(/<head\b[^>]*>[\s\S]*?<\/head>/i)?.[0] || '';
  const links = head.match(/<link\b[^>]*>/gi) || [];
  const canonical = links.filter(tag => attribute(tag, 'rel') === 'canonical');
  if (canonical.length !== 1 || attribute(canonical[0], 'href') !== canonicalUrl) {
    throw new Error(`Expected exactly one canonical: ${canonicalUrl}`);
  }
  const bases = head.match(/<base\b[^>]*>/gi) || [];
  if (bases.length !== 1 || attribute(bases[0], 'href') !== baseHref) {
    throw new Error(`Incorrect base path for ${canonicalUrl}`);
  }
  const expected = baseHref === '/' ? 'index' : 'noindex';
  const metas = head.match(/<meta\b[^>]*>/gi) || [];
  for (const name of ['robots', 'googlebot']) {
    const tags = metas.filter(tag => attribute(tag, 'name') === name);
    if (tags.length !== 1 || attribute(tags[0], 'content')?.split(',')[0].trim() !== expected) {
      throw new Error(`Incorrect ${name} directive for ${canonicalUrl}`);
    }
  }
  if (metas.filter(tag => attribute(tag, 'name') === 'prerender-rendered').length !== 1 ||
      !/<main\b/i.test(html) || !/<h[12]\b/i.test(html)) {
    throw new Error(`Missing rendered route content: ${canonicalUrl}`);
  }
}
