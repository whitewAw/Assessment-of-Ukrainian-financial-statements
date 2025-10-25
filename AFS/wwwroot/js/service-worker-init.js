/**
 * UFIN Financial Analysis Tool - Service Worker Initialization
 * Handles PWA service worker registration and updates
 */

(function () {
    'use strict';

    if ('serviceWorker' in navigator) {
        window.addEventListener('load', function () {
            navigator.serviceWorker.register('/service-worker.js')
                .then(function (registration) {
                    console.log('Service Worker registered successfully:', {
                        scope: registration.scope,
                        updateViaCache: registration.updateViaCache
                    });

                    // Check for updates periodically
                    setInterval(() => {
                        registration.update().then(() => {
                            console.log('Service Worker update check completed');
                        });
                    }, 60 * 60 * 1000); // Check every hour

                    // Update service worker on page load
                    registration.update();

                    // Handle waiting service worker
                    if (registration.waiting) {
                        promptUserToUpdate(registration.waiting);
                    }

                    // Listen for new service worker
                    registration.addEventListener('updatefound', () => {
                        const newWorker = registration.installing;
                        console.log('New Service Worker found');

                        newWorker.addEventListener('statechange', () => {
                            if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
                                console.log('New Service Worker installed - update available');
                                promptUserToUpdate(newWorker);
                            }
                        });
                    });
                })
                .catch(function (error) {
                    console.warn('Service Worker registration failed:', error);
                });

            // Handle service worker controller changes
            navigator.serviceWorker.addEventListener('controllerchange', function () {
                console.log('Service Worker controller changed - reloading page');
                window.location.reload();
            });

            // Listen for messages from service worker
            navigator.serviceWorker.addEventListener('message', function (event) {
                console.log('Message from Service Worker:', event.data);
            });
        });
    } else {
        console.warn('Service Workers not supported in this browser');
    }

    /**
     * Prompt user to update to new service worker version
     * @param {ServiceWorker} worker - The waiting service worker
     */
    function promptUserToUpdate(worker) {
        // Check if user wants auto-update
        const autoUpdate = localStorage.getItem('autoUpdateServiceWorker');

        if (autoUpdate === 'true') {
            worker.postMessage({ type: 'SKIP_WAITING' });
            return;
        }

        // Show update notification (you can customize this)
        const updateBanner = document.createElement('div');
        updateBanner.id = 'sw-update-banner';
        updateBanner.className = 'alert alert-info position-fixed bottom-0 start-0 end-0 m-3';
        updateBanner.innerHTML = `
            <div class="d-flex align-items-center justify-content-between">
   <span>
 <strong>Update Available!</strong> A new version of UFIN is ready.
         </span>
       <div>
       <button class="btn btn-sm btn-primary me-2" onclick="updateServiceWorker()">
         Update Now
    </button>
        <button class="btn btn-sm btn-secondary" onclick="dismissUpdateBanner()">
           Later
       </button>
     </div>
   </div>
        `;

        document.body.appendChild(updateBanner);

        // Make update function globally available
        window.updateServiceWorker = function () {
            worker.postMessage({ type: 'SKIP_WAITING' });
            updateBanner.remove();
        };

        window.dismissUpdateBanner = function () {
            updateBanner.remove();
        };
    }
})();
