/**
 * UFIN Financial Analysis Tool - Main Application Script (Optimized for TBT)
 * Handles Blazor initialization with minimal blocking
 */

(function () {
    'use strict';

    const logger = window.UFIN?.logger || console;
    const perf = window.UFIN?.perf || { mark: () => { }, measure: () => { } };

    perf.mark('app-init-start');

    // Update progress indicator (non-blocking)
    function updateProgress(message) {
        requestAnimationFrame(() => {
            const progressEl = document.getElementById('load-progress');
            if (progressEl) {
                progressEl.textContent = message;
            }
        });
    }

    // Yield to main thread periodically
    function yieldToMain() {
        return new Promise(resolve => {
            setTimeout(resolve, 0);
        });
    }

    // Handle SPA redirect from 404.html
    function handleSPARedirect() {
        const redirectPath = sessionStorage.getItem('ufin-redirect-path');
        if (redirectPath) {
            sessionStorage.removeItem('ufin-redirect-path');
            
            // Get base path
            const config = window.UFIN_CONFIG || {};
            const basePath = config.basePath || '/';
            
            // Clean the path - remove base path if present to get relative path
            let cleanPath = redirectPath;
            if (cleanPath.startsWith(basePath)) {
                cleanPath = cleanPath.substring(basePath.length);
            }
            if (cleanPath.startsWith('/')) {
                cleanPath = cleanPath.substring(1);
            }
            
            // Navigate using Blazor's navigation
            if (cleanPath && window.Blazor) {
                logger.log('[SPA] Redirecting to:', cleanPath);
                // Use history API to navigate without page reload
                const fullPath = basePath + cleanPath;
                history.replaceState(null, '', fullPath);
                // Trigger Blazor navigation
                window.dispatchEvent(new PopStateEvent('popstate'));
            }
        }
    }

    updateProgress('Starting Blazor');

    // Initialize Blazor with optimized settings
    Blazor.start({
        loadBootResource: function (type, name, defaultUri, integrity) {
            // Show what's loading
            if (type === 'dotnetwasm') updateProgress('Loading WebAssembly');
            if (type === 'dotnetjs') updateProgress('Loading runtime');
            if (type === 'assembly') updateProgress('Loading assemblies');

            // Add cache-busting query param for runtime files
            // This ensures browser re-downloads when integrity changes (new build)
            if (type === 'dotnetjs' || type === 'dotnetwasm') {
                return defaultUri + (integrity ? '?v=' + integrity.substring(0, 8) : '');
            }
            
            return defaultUri;
        },
        environment: 'Production'
    }).then(async () => {
        perf.mark('app-init-end');
        const loadTime = perf.measure('app-init', 'app-init-start', 'app-init-end');

        logger.log('✅ UFIN loaded successfully in', loadTime.toFixed(2), 'ms');

        // Yield to main thread before UI updates
        await yieldToMain();

        // Handle SPA redirect from 404.html (GitHub Pages)
        handleSPARedirect();

        updateProgress('Ready');

        // Add loaded class
        document.body.classList.add('blazor-loaded');

        // Smooth fade out - non-blocking
        requestAnimationFrame(() => {
            const appElement = document.getElementById('app');
            if (appElement) {
                const loadingDiv = appElement.querySelector('.d-flex.align-items-center');
                if (loadingDiv) {
                    // Smooth transition
                    loadingDiv.style.transition = 'opacity 0.3s ease-out';
                    loadingDiv.style.opacity = '0';
                    setTimeout(() => {
                        loadingDiv.style.display = 'none';
                    }, 300);
                }
            }
        });

        // Dispatch custom event
        window.dispatchEvent(new CustomEvent('ufin:loaded'));
    }).catch(error => {
        logger.error('❌ Failed to start Blazor:', error);
        updateProgress('Error - Reload required');

        // Show user-friendly error
        const appElement = document.getElementById('app');
        if (appElement) {
            appElement.innerHTML = `
    <div class="d-flex align-items-center justify-content-center vh-100">
       <div class="text-center px-3">
           <h3 class="text-danger">Application Failed to Load</h3>
       <p class="text-muted">Please refresh the page or check your internet connection.</p>
     <button class="btn btn-primary mt-3" onclick="window.location.reload()">
          Reload Application
          </button>
        </div>
    </div>
        `;
        }
    });
})();
