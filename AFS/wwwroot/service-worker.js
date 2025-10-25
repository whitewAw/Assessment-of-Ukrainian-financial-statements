// Service Worker - Development Mode
// In development, we don't want aggressive caching
// Only register event listeners if needed

// Only add fetch listener if we're actually going to do something
// This prevents the console warning about no-op handlers
if (self.registration.scope.includes('published')) {
    // Production mode - enable caching
    self.addEventListener('fetch', (event) => {
        // Production caching logic here
    });
} else {
    // Development mode - no caching needed
    // Don't register fetch handler at all to avoid warning
    console.log('Service Worker: Development mode - caching disabled');
}
