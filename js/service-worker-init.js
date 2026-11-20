/**
 * UFIN Financial Analysis Tool - Service Worker Initialization
 * Handles PWA service worker registration and updates
 */

(function () {
    'use strict';

    let autoUpdateServiceWorker = false;

    function initializeUpdatePreference() {
        try {
            const storedPreference = localStorage.getItem('autoUpdateServiceWorker');
            if (storedPreference === null) {
                localStorage.setItem('autoUpdateServiceWorker', 'false');
            } else {
                autoUpdateServiceWorker = storedPreference === 'true';
            }
        } catch (error) {
            console.warn('Unable to initialize service worker update preference; updates will require confirmation:', error);
        }
    }

    function saveUpdatePreference(enabled) {
        autoUpdateServiceWorker = enabled;
        try {
            localStorage.setItem('autoUpdateServiceWorker', String(enabled));
        } catch (error) {
            console.warn('Unable to save service worker update preference:', error);
        }
    }

    if ('serviceWorker' in navigator) {
        window.addEventListener('load', function () {
            initializeUpdatePreference();
            const hadController = Boolean(navigator.serviceWorker.controller);
            let isReloading = false;

            // Get the base path from the document's base element or default to root
            const baseElement = document.querySelector('base');
            const basePath = baseElement ? baseElement.getAttribute('href') : '/';
            const swPath = basePath + 'service-worker.js';

            navigator.serviceWorker.register(swPath, { scope: basePath, updateViaCache: 'none' })
                .then(function (registration) {
                    console.log('Service Worker registered successfully:', {
                        scope: registration.scope,
                        updateViaCache: registration.updateViaCache
                    });

                    // Check for updates when the user returns to the tab.
                    // This replaces a previous `setInterval(..., 1h)` which
                    // fired 24 useless background update checks per day even
                    // on hidden tabs — bad for mobile battery and does nothing
                    // the user can observe. visibilitychange gives us a
                    // natural event exactly when a fresh deploy could matter.
                    document.addEventListener('visibilitychange', () => {
                        if (document.visibilityState === 'visible') {
                            registration.update().catch(err => {
                                console.debug('Service Worker visibilitychange update failed:', err);
                            });
                        }
                    });

                    // Update service worker on page load
                    registration.update().catch(err => {
                        console.debug('Service Worker load update failed:', err);
                    });

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

            // Reload once after an approved update takes control. Initial control
            // on a first visit must not interrupt the page that just loaded.
            navigator.serviceWorker.addEventListener('controllerchange', function () {
                if (!hadController || isReloading) {
                    return;
                }

                isReloading = true;
                console.log('Updated service worker took control - reloading page');
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
        if (document.getElementById('sw-update-banner')) {
            return;
        }

        if (autoUpdateServiceWorker) {
            worker.postMessage({ type: 'SKIP_WAITING' });
            return;
        }

        // Show update notification (you can customize this)
        const updateBanner = document.createElement('div');
        updateBanner.id = 'sw-update-banner';
        updateBanner.className = 'alert alert-info position-fixed bottom-0 start-0 end-0 m-3';
        updateBanner.innerHTML = `
            <div class="d-flex align-items-center justify-content-between flex-wrap gap-2">
                <span><strong>Update Available!</strong> A new version of UFIN is ready.</span>
                <div>
                    <button class="btn btn-sm btn-primary me-2" onclick="updateServiceWorker()">Update Now</button>
                    <button class="btn btn-sm btn-secondary" onclick="dismissUpdateBanner()">Later</button>
                </div>
            </div>
            <label class="d-flex align-items-center gap-2 mt-2">
                <input id="auto-update-service-worker" type="checkbox">
                Automatically install future updates
            </label>
        `;

        document.body.appendChild(updateBanner);
        const autoUpdateCheckbox = updateBanner.querySelector('#auto-update-service-worker');
        autoUpdateCheckbox.checked = autoUpdateServiceWorker;
        autoUpdateCheckbox.addEventListener('change', () => {
            saveUpdatePreference(autoUpdateCheckbox.checked);
        });

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
