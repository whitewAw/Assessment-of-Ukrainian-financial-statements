export function sitemapLocations(xml) {
  const activeXml = xml.replace(/<!--[\s\S]*?-->/g, '');
  return [...activeXml.matchAll(/<loc>\s*([^<]+?)\s*<\/loc>/g)].map(match => match[1].trim());
}

export function isRouteReady(doc = document) {
  if (doc.title === 'Page Not Found') {
    throw new Error('Route rendered the Blazor NotFound page');
  }
  const app = doc.getElementById('app');
  const main = app?.querySelector('main');
  return Boolean(main && main.innerText.trim().length > 200 && main.querySelector('h1, h2'));
}
