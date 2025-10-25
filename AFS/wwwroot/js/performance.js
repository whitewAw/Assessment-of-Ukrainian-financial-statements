/**
 * UFIN Financial Analysis Tool - Performance Monitoring (Optimized)
 * Conditional monitoring based on environment
 */

(function () {
    'use strict';

    const logger = window.UFIN?.logger || console;
    const perf = window.UFIN?.perf || { mark: () => {}, measure: () => {} };
    const isDevelopment = window.location.hostname === 'localhost' || window.location.hostname.includes('dev');

    // Only full monitoring in development
    if (!isDevelopment) {
        // Production: minimal monitoring
        window.addEventListener('load', function () {
            if (window.performance && window.performance.timing) {
                const timing = window.performance.timing;
                const loadTime = timing.loadEventEnd - timing.navigationStart;

                // Only log if unusually slow
                if (loadTime > 3000) {
                    logger.warn('Slow page load:', loadTime, 'ms');
                }
            }
        });

        // Monitor online/offline
        window.addEventListener('online', () => logger.log('Online'));
        window.addEventListener('offline', () => logger.warn('Offline - PWA active'));

        // Critical errors only
        window.addEventListener('error', (e) => logger.error('Error:', e.message, e.filename));
        window.addEventListener('unhandledrejection', (e) => logger.error('Unhandled rejection:', e.reason));

        return;
    }

    // Development: full monitoring
    window.addEventListener('load', function () {
        perf.mark('page-loaded');

        // Performance timing
        if (window.performance && window.performance.timing) {
            const timing = window.performance.timing;
  
            // Fix: Only calculate if values are valid
            const navigationStart = timing.navigationStart;
            const loadEventEnd = timing.loadEventEnd;
         
            if (navigationStart > 0 && loadEventEnd > navigationStart) {
                const loadTime = loadEventEnd - navigationStart;
                const domReady = timing.domContentLoadedEventEnd - navigationStart;
                const firstPaint = timing.responseEnd - navigationStart;

                logger.group('?? Performance Metrics');
                logger.log('Total Load Time:', loadTime, 'ms');
                logger.log('DOM Ready:', domReady, 'ms');
                logger.log('First Paint:', firstPaint, 'ms');
                logger.groupEnd();
            } else {
                // Use Navigation Timing API Level 2 if available
                if (window.performance.getEntriesByType) {
                    const navTiming = window.performance.getEntriesByType('navigation')[0];
                    if (navTiming) {
                        logger.group('?? Performance Metrics');
                        logger.log('Total Load Time:', Math.round(navTiming.loadEventEnd), 'ms');
                        logger.log('DOM Ready:', Math.round(navTiming.domContentLoadedEventEnd), 'ms');
                        logger.log('First Paint:', Math.round(navTiming.responseEnd), 'ms');
                        logger.groupEnd();
                    }
                }
            }

            // Memory usage (Chrome only)
            if (window.performance.memory) {
                const memory = window.performance.memory;
                logger.group('?? Memory Usage');
                logger.log('Used:', (memory.usedJSHeapSize / 1048576).toFixed(2), 'MB');
                logger.log('Total:', (memory.totalJSHeapSize / 1048576).toFixed(2), 'MB');
                logger.log('Limit:', (memory.jsHeapSizeLimit / 1048576).toFixed(2), 'MB');
                logger.groupEnd();
            }
        }

        // Network info
        if ('connection' in navigator) {
            const connection = navigator.connection || navigator.mozConnection || navigator.webkitConnection;
            if (connection) {
                logger.group('?? Network Info');
                logger.log('Type:', connection.effectiveType);
                logger.log('Downlink:', connection.downlink, 'Mbps');
                logger.log('RTT:', connection.rtt, 'ms');
                logger.log('Save Data:', connection.saveData);
                logger.groupEnd();

                if (connection.effectiveType === 'slow-2g' || connection.effectiveType === '2g') {
                    logger.warn('?? Slow network detected');
                }
            }
        }

        // Core Web Vitals (sampled)
        if ('PerformanceObserver' in window) {
            try {
                // LCP - sample only
                let lcpObserver = new PerformanceObserver((list) => {
                    const entries = list.getEntries();
                    const lastEntry = entries[entries.length - 1];
                    const lcp = lastEntry.renderTime || lastEntry.loadTime;
                    logger.log('?? LCP:', lcp.toFixed(2), 'ms', lcp < 2500 ? '?' : '??');
                    lcpObserver.disconnect();
                });
                lcpObserver.observe({ entryTypes: ['largest-contentful-paint'] });

                // FID
                new PerformanceObserver((list) => {
                    list.getEntries().forEach((entry) => {
                        const fid = entry.processingStart - entry.startTime;
                        logger.log('? FID:', fid.toFixed(2), 'ms', fid < 100 ? '?' : '??');
                    });
                }).observe({ entryTypes: ['first-input'] });

                // CLS - throttled
                let clsValue = 0;
                let clsTimeout;
                new PerformanceObserver((list) => {
                    for (const entry of list.getEntries()) {
                        if (!entry.hadRecentInput) {
                            clsValue += entry.value;
                        }
                    }

                    clearTimeout(clsTimeout);
                    clsTimeout = setTimeout(() => {
                        logger.log('?? CLS:', clsValue.toFixed(3), clsValue < 0.1 ? '?' : '??');
                    }, 500);
                }).observe({ entryTypes: ['layout-shift'] });
            } catch (error) {
                logger.log('Performance Observer partial support');
            }
        }
    });

    // Monitor online/offline
    window.addEventListener('online', () => logger.log('?? Online'));
    window.addEventListener('offline', () => logger.warn('?? Offline - PWA mode'));

    // Error logging with stack traces
    window.addEventListener('error', function (event) {
        logger.error('? Unhandled error:', {
            message: event.message,
            source: event.filename,
            line: event.lineno,
            column: event.colno,
            stack: event.error?.stack
        });
    });

    window.addEventListener('unhandledrejection', function (event) {
        logger.error('? Unhandled rejection:', event.reason);
    });

    // Page visibility
    document.addEventListener('visibilitychange', function () {
        logger.log(document.hidden ? '??? Page hidden' : '??? Page visible');
    });

    logger.log('?? Performance monitoring initialized (development mode)');
})();
