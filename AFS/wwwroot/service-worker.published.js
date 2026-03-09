// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;

// Optimized cache patterns - more specific ordering for better performance
const offlineAssetsInclude = [
    /\.wasm$/,  // WebAssembly files (largest, most important)
    /\.dll$/,      // .NET assemblies
    /\.dat$/,     // ICU data files
    /\.blat$/,       // Blazor boot resources
    /\.js$/,        // JavaScript files
    /\.css$/,            // Stylesheets
    /\.html$/,           // HTML files
    /\.json$/,     // JSON configuration
    /\.woff2?$/,         // Web fonts (woff and woff2)
    /\.ttf$/,            // TrueType fonts
    /\.eot$/,            // Embedded OpenType fonts
    /\.svg$/,            // SVG files (icons, images)
    /\.png$/,// PNG images
    /\.jpe?g$/,          // JPEG images
    /\.gif$/,            // GIF images
    /\.webp$/,      // WebP images
    /\.ico$/,            // Favicons
    /\.pdb$/     // Debug symbols (only in debug builds)
];

const offlineAssetsExclude = [
    /^service-worker\.js$/,
    /^service-worker-assets\.js$/
];

// Stale-While-Revalidate patterns (for frequently updated resources)
const staleWhileRevalidatePatterns = [
    /\.json$/,     // Configuration files may update
    /api\//        // API calls
];

// Cache-First patterns (for immutable resources)
const cacheFirstPatterns = [
    /\.wasm$/,
    /\.dll$/,
    /\.dat$/,
    /\.woff2?$/,
    /\.ttf$/,
    /\.eot$/
];

/**
 * Install event - cache all static assets
 */
async function onInstall(event) {
    console.info('Service worker: Install started');

    // Take control immediately
    self.skipWaiting();

    // Filter and prepare assets for caching
    const assetsToCache = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)));

    console.info(`Service worker: Caching ${assetsToCache.length} assets`);

    // Separate index.html from other assets (it changes frequently)
    const indexAssets = assetsToCache.filter(asset => /index\.html$/.test(asset.url));
    const otherAssets = assetsToCache.filter(asset => !/index\.html$/.test(asset.url));

    // Create requests with integrity check for immutable assets
    const assetsRequests = otherAssets.map(asset =>
        new Request(asset.url, {
            integrity: asset.hash,
            cache: 'no-cache',
            mode: 'cors',
            credentials: 'omit'
        })
    );

    // Create requests WITHOUT integrity check for index.html (it changes frequently)
    const indexRequests = indexAssets.map(asset =>
        new Request(asset.url, {
            cache: 'no-cache',
            mode: 'cors',
            credentials: 'omit'
        })
    );

    try {
        const cache = await caches.open(cacheName);

        // Cache index.html without integrity check
        if (indexRequests.length > 0) {
            try {
                await Promise.allSettled(
                    indexRequests.map(async req => {
                        try {
                            const response = await fetch(req.clone());
                            if (response.ok) {
                                await cache.put(req, response);
                                console.info(`Service worker: Cached ${req.url} (no integrity check)`);
                            }
                        } catch (e) {
                            console.warn(`Service worker: Failed to cache ${req.url}`, e);
                        }
                    })
                );
            } catch (error) {
                console.warn('Service worker: Failed to cache index.html', error);
            }
        }

        // Cache other assets in batches with integrity checks
        const batchSize = 50;
        for (let i = 0; i < assetsRequests.length; i += batchSize) {
            const batch = assetsRequests.slice(i, i + batchSize);
            try {
                await cache.addAll(batch);
                console.info(`Service worker: Cached batch ${Math.floor(i / batchSize) + 1}/${Math.ceil(assetsRequests.length / batchSize)}`);
            } catch (error) {
                console.warn(`Service worker: Failed to cache batch ${Math.floor(i / batchSize) + 1}`, error);
                // Try to cache individually for this batch
                await Promise.allSettled(
                    batch.map(async req => {
                        try {
                            const response = await fetch(req.clone());
                            if (response.ok) {
                                await cache.put(req, response);
                            }
                        } catch (e) {
                            console.warn(`Service worker: Failed to cache ${req.url}`, e);
                        }
                    })
                );
            }
        }

        console.info('Service worker: Install completed successfully');
    } catch (error) {
        console.error('Service worker: Install failed', error);
        // Continue anyway - will fall back to network
    }
}

/**
 * Activate event - clean up old caches
 */
async function onActivate(event) {
    console.info('Service worker: Activate started');

    // Take control of all clients immediately
    await self.clients.claim();

    // Delete old caches
    try {
        const cacheKeys = await caches.keys();
        const oldCacheKeys = cacheKeys.filter(key =>
            key.startsWith(cacheNamePrefix) && key !== cacheName
        );

        if (oldCacheKeys.length > 0) {
            console.info(`Service worker: Deleting ${oldCacheKeys.length} old cache(s)`);
            await Promise.all(oldCacheKeys.map(key => caches.delete(key)));
        }

        console.info('Service worker: Activate completed successfully');
    } catch (error) {
        console.error('Service worker: Activation cleanup failed', error);
    }
}

/**
 * Fetch event - serve from cache or network
 */
async function onFetch(event) {
    const { request } = event;
    const url = new URL(request.url);

    // Only handle GET requests
    if (request.method !== 'GET') {
        return fetch(request);
    }

    // Don't cache chrome-extension:// or other non-http(s) requests
    if (!url.protocol.startsWith('http')) {
        return fetch(request);
    }

    // Determine caching strategy based on URL pattern
    const isCacheFirst = cacheFirstPatterns.some(pattern => pattern.test(url.pathname));
    const isStaleWhileRevalidate = staleWhileRevalidatePatterns.some(pattern => pattern.test(url.pathname));
    const isNavigationRequest = request.mode === 'navigate';

    try {
        // Cache-First strategy for immutable resources
        if (isCacheFirst) {
            return await cacheFirst(request);
        }

        // Stale-While-Revalidate for frequently updated resources
        if (isStaleWhileRevalidate) {
            return await staleWhileRevalidate(request);
        }

        // Navigation requests (page loads)
        if (isNavigationRequest) {
            return await handleNavigationRequest(request);
        }

        // Default: Network-First with cache fallback
        return await networkFirst(request);

    } catch (error) {
        console.error('Service worker: Fetch error for', request.url, error);

        // Final fallback: try to serve index.html for navigation requests
        if (isNavigationRequest) {
            try {
                const cache = await caches.open(cacheName);
                const indexResponse = await cache.match('index.html');
                if (indexResponse) {
                    console.info('Service worker: Serving cached index.html as fallback');
                    return indexResponse;
                }
            } catch (cacheError) {
                console.error('Service worker: Cache fallback failed', cacheError);
            }
        }

        // Return a user-friendly offline page instead of raw 503
        return new Response(
            `<!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <title>UFIN - Offline</title>
                <style>
                    body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; 
                           display: flex; align-items: center; justify-content: center; 
                           min-height: 100vh; margin: 0; background: #1a1a1a; color: #e8e8e8; }
                    .container { text-align: center; padding: 2rem; }
                    h1 { color: #512BD4; }
                    button { background: #512BD4; color: white; border: none; padding: 12px 24px; 
                             border-radius: 8px; cursor: pointer; font-size: 1rem; margin-top: 1rem; }
                    button:hover { background: #6B3FA0; }
                </style>
            </head>
            <body>
                <div class="container">
                    <h1>📊 UFIN</h1>
                    <h2>You're Offline</h2>
                    <p>Please check your internet connection and try again.</p>
                    <button onclick="location.reload()">Retry</button>
                </div>
            </body>
            </html>`,
            {
                status: 503,
                statusText: 'Service Unavailable',
                headers: { 'Content-Type': 'text/html; charset=utf-8' }
            }
        );
    }
}

/**
 * Cache-First strategy - check cache first, fallback to network
 */
async function cacheFirst(request) {
    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(request);

    if (cachedResponse) {
        return cachedResponse;
    }

    // Not in cache, fetch from network and cache it
    const networkResponse = await fetch(request);
    if (networkResponse.ok) {
        cache.put(request, networkResponse.clone());
    }
    return networkResponse;
}

/**
 * Network-First strategy - try network first, fallback to cache
 */
async function networkFirst(request) {
    const cache = await caches.open(cacheName);
    
    try {
        const networkResponse = await fetch(request);

        // Cache successful responses
        if (networkResponse.ok) {
            cache.put(request, networkResponse.clone());
        }

        return networkResponse;
    } catch (error) {
        // Network failed, try cache
        const cachedResponse = await cache.match(request);

        if (cachedResponse) {
            console.info('Service worker: Serving from cache (offline)', request.url);
            return cachedResponse;
        }

        // No cache available - return a more helpful error for non-critical resources
        console.warn('Service worker: No cache available for', request.url);
        
        // For non-critical resources, return an empty response instead of throwing
        const url = new URL(request.url);
        if (url.pathname.match(/\.(png|jpg|jpeg|gif|svg|ico|woff2?|ttf|eot)$/i)) {
            // Return empty response for images/fonts - they're not critical
            return new Response('', { status: 200, statusText: 'OK (offline placeholder)' });
        }
        
        throw error;
    }
}

/**
 * Stale-While-Revalidate - serve cached version while fetching fresh version
 */
async function staleWhileRevalidate(request) {
    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(request);

    // Fetch fresh version in the background
    const fetchPromise = fetch(request).then(networkResponse => {
        if (networkResponse.ok) {
            cache.put(request, networkResponse.clone());
        }
        return networkResponse;
    }).catch(error => {
        console.warn('Service worker: Background fetch failed', error);
        return null;
    });

    // Return cached version immediately if available, otherwise wait for network
    return cachedResponse || fetchPromise;
}

/**
 * Handle navigation requests (page loads)
 * For SPAs, always serve index.html for navigation requests
 */
async function handleNavigationRequest(request) {
    const cache = await caches.open(cacheName);

    try {
        // For SPA routing, fetch index.html instead of the actual URL
        // This is because Blazor handles all routes client-side
        const indexRequest = new Request('index.html', {
            method: 'GET',
            headers: request.headers,
            mode: 'same-origin',
            credentials: 'same-origin'
        });

        // Try network first for fresh content
        try {
            const networkResponse = await fetch(indexRequest);

            if (networkResponse.ok) {
                cache.put('index.html', networkResponse.clone());
                return networkResponse;
            }
        } catch (networkError) {
            // Network failed, will try cache below
            console.info('Service worker: Network unavailable, trying cache');
        }

        // Network failed or not ok, serve cached index.html
        const cachedResponse = await cache.match('index.html');

        if (cachedResponse) {
            console.info('Service worker: Serving cached index.html for SPA route');
            return cachedResponse;
        }

        // No cached index.html, throw to trigger offline page
        throw new Error('No cached index.html available');

    } catch (error) {
        console.error('Service worker: Navigation handling failed', error);
        throw error;
    }
}
