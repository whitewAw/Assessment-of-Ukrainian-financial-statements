/**
 * UFIN Financial Analysis Tool - Main Application Script (Optimized for LCP)
 * Handles Blazor initialization and core app functionality
 */

(function () {
    'use strict';

    const logger = window.UFIN?.logger || console;
    const perf = window.UFIN?.perf || { mark: () => { }, measure: () => { } };

    perf.mark('app-init-start');

    // Update progress indicator
    function updateProgress(message) {
        const progressEl = document.getElementById('load-progress');
        if (progressEl) {
            progressEl.textContent = message;
        }
    }

    updateProgress('Starting Blazor');

    // Initialize Blazor with optimized settings
    Blazor.start({
        loadBootResource: function (type, name, defaultUri, integrity) {
            // Show what's loading
            if (type === 'dotnetwasm') updateProgress('Loading WebAssembly');
            if (type === 'dotnetjs') updateProgress('Loading runtime');

            // Optimize resource loading with cache busting
            if (type === 'dotnetjs' || type === 'dotnetwasm') {
                return defaultUri + (integrity ? '?v=' + integrity : '');
            }
            return defaultUri;
        },
        environment: 'Production'
    }).then(() => {
        perf.mark('app-init-end');
        const loadTime = perf.measure('app-init', 'app-init-start', 'app-init-end');

        logger.log('✅ UFIN loaded successfully in', loadTime.toFixed(2), 'ms');

        updateProgress('Ready');

        // Add loaded class
        document.body.classList.add('blazor-loaded');

        // Fade out loading screen
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
