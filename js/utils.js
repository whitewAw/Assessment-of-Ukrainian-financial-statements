/**
 * UFIN Financial Analysis Tool - Utilities
 * Shared utility functions and helpers
 */

(function (window) {
    'use strict';

    // Environment detection
    const isDevelopment = window.location.hostname === 'localhost' ||
        window.location.hostname === '127.0.0.1' ||
        window.location.hostname.includes('dev');

    // Smart Logger - only logs in development
    window.UFIN = window.UFIN || {};
    window.UFIN.logger = {
        log: isDevelopment ? console.log.bind(console) : () => { },
        info: isDevelopment ? console.info.bind(console) : () => { },
        warn: console.warn.bind(console), // Always show warnings
        error: console.error.bind(console), // Always show errors
        group: isDevelopment ? console.group.bind(console) : () => { },
        groupEnd: isDevelopment ? console.groupEnd.bind(console) : () => { },
        table: isDevelopment ? console.table.bind(console) : () => { }
    };

    // Performance tracker with sampling
    window.UFIN.perf = {
        marks: {},

        mark(name) {
            if (isDevelopment && window.performance) {
                window.performance.mark(name);
                this.marks[name] = performance.now();
            }
        },

        measure(name, startMark, endMark) {
            if (isDevelopment && window.performance) {
                try {
                    window.performance.measure(name, startMark, endMark);
                    const measure = window.performance.getEntriesByName(name)[0];
                    return measure ? measure.duration : 0;
                } catch (e) {
                    return 0;
                }
            }
            return 0;
        },

        getMetric(name) {
            return this.marks[name] || 0;
        },

        clear() {
            this.marks = {};
            if (window.performance && window.performance.clearMarks) {
                window.performance.clearMarks();
                window.performance.clearMeasures();
            }
        }
    };

    // === INP OPTIMIZATION UTILITIES ===
    
    /**
     * Scheduler for yielding to main thread - critical for INP optimization
     * Uses scheduler.yield() if available, falls back to setTimeout
     */
    window.UFIN.scheduler = {
        /**
         * Yields to the main thread to allow paint updates
         * @returns {Promise<void>}
         */
        async yieldToMain() {
            // Use scheduler.yield if available (Chrome 115+)
            if ('scheduler' in window && 'yield' in window.scheduler) {
                return window.scheduler.yield();
            }
            // Fallback: setTimeout with 0ms allows browser to process paint
            return new Promise(resolve => setTimeout(resolve, 0));
        },

        /**
         * Runs a task after yielding to allow UI updates
         * @param {Function} callback - Task to run after yield
         */
        async runAfterPaint(callback) {
            // requestAnimationFrame schedules before next paint
            // Double rAF ensures we run after the paint
            return new Promise(resolve => {
                requestAnimationFrame(() => {
                    requestAnimationFrame(async () => {
                        await callback();
                        resolve();
                    });
                });
            });
        },

        /**
         * Runs heavy work in chunks to avoid blocking the main thread
         * @param {Array} items - Items to process
         * @param {Function} processor - Function to process each item
         * @param {number} chunkSize - Items per chunk (default 5)
         */
        async processInChunks(items, processor, chunkSize = 5) {
            for (let i = 0; i < items.length; i += chunkSize) {
                const chunk = items.slice(i, i + chunkSize);
                for (const item of chunk) {
                    await processor(item);
                }
                // Yield between chunks to allow UI updates
                await this.yieldToMain();
            }
        }
    };

    /**
     * Optimizes click handlers for better INP
     * Provides immediate visual feedback before processing
     */
    window.UFIN.interaction = {
        /**
         * Wraps a click handler to optimize for INP
         * @param {HTMLElement} element - Element to attach handler to
         * @param {Function} handler - The actual handler logic
         * @param {Object} options - Configuration options
         */
        optimizeClick(element, handler, options = {}) {
            const { 
                feedbackClass = 'clicked',
                feedbackDuration = 150
            } = options;

            element.addEventListener('click', async (event) => {
                // Immediate visual feedback
                element.classList.add(feedbackClass);
                
                // Use requestAnimationFrame to ensure the class is painted
                requestAnimationFrame(async () => {
                    // Yield to allow the visual update to paint
                    await window.UFIN.scheduler.yieldToMain();
                    
                    // Now run the actual handler
                    try {
                        await handler(event);
                    } finally {
                        // Remove feedback class after duration
                        setTimeout(() => {
                            element.classList.remove(feedbackClass);
                        }, feedbackDuration);
                    }
                });
            });
        },

        /**
         * Creates an optimized toggle function for Blazor components
         * Returns a function that yields before calling StateHasChanged
         */
        createOptimizedToggle() {
            return async function optimizedToggle(dotNetHelper, methodName) {
                // Yield to main thread first
                await window.UFIN.scheduler.yieldToMain();
                // Then invoke the Blazor method
                await dotNetHelper.invokeMethodAsync(methodName);
            };
        }
    };

    /**
     * INP monitoring and reporting
     */
    window.UFIN.inpMonitor = {
        _observer: null,
        _interactions: [],
        _maxEntries: 10,

        /**
         * Starts monitoring INP
         */
        start() {
            if (!('PerformanceObserver' in window)) {
                return;
            }

            try {
                this._observer = new PerformanceObserver((list) => {
                    for (const entry of list.getEntries()) {
                        // Track event timing entries for INP
                        if (entry.entryType === 'event') {
                            const inp = entry.processingEnd - entry.startTime;
                            this._interactions.push({
                                name: entry.name,
                                inp: inp,
                                target: entry.target?.tagName || 'unknown',
                                timestamp: Date.now()
                            });

                            // Keep only recent interactions
                            if (this._interactions.length > this._maxEntries) {
                                this._interactions.shift();
                            }

                            // Log slow interactions in development
                            if (isDevelopment && inp > 200) {
                                window.UFIN.logger.warn(
                                    `?? Slow interaction (INP): ${inp.toFixed(0)}ms on ${entry.name}`,
                                    entry.target
                                );
                            }
                        }
                    }
                });

                // Observe event timing with buffered entries
                this._observer.observe({ 
                    type: 'event', 
                    buffered: true,
                    durationThreshold: 16 // Track events longer than 1 frame
                });
            } catch (e) {
                // Event timing not supported
                window.UFIN.logger.log('Event timing not supported for INP monitoring');
            }
        },

        /**
         * Gets the worst INP value from recent interactions
         */
        getWorstINP() {
            if (this._interactions.length === 0) return 0;
            return Math.max(...this._interactions.map(i => i.inp));
        },

        /**
         * Gets all tracked interactions
         */
        getInteractions() {
            return [...this._interactions];
        },

        /**
         * Stops monitoring
         */
        stop() {
            if (this._observer) {
                this._observer.disconnect();
                this._observer = null;
            }
        }
    };

    // Start INP monitoring automatically
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            window.UFIN.inpMonitor.start();
        });
    } else {
        window.UFIN.inpMonitor.start();
    }

    // Device info (cached)
    window.UFIN.device = {
        isMobile: /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent),
        isIOS: /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream,
        isAndroid: /Android/i.test(navigator.userAgent),
        isTouch: 'ontouchstart' in window || navigator.maxTouchPoints > 0,

        get info() {
            return {
                userAgent: navigator.userAgent,
                screenWidth: window.screen.width,
                screenHeight: window.screen.height,
                viewportWidth: window.innerWidth,
                viewportHeight: window.innerHeight,
                pixelRatio: window.devicePixelRatio,
                platform: navigator.platform,
                touchPoints: navigator.maxTouchPoints,
                connection: this.getConnection()
            };
        },

        getConnection() {
            const conn = navigator.connection || navigator.mozConnection || navigator.webkitConnection;
            if (!conn) return null;

            return {
                effectiveType: conn.effectiveType,
                downlink: conn.downlink,
                rtt: conn.rtt,
                saveData: conn.saveData
            };
        }
    };

    // Debounce utility
    window.UFIN.debounce = function (func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    };

    // Throttle utility
    window.UFIN.throttle = function (func, limit) {
        let inThrottle;
        return function (...args) {
            if (!inThrottle) {
                func.apply(this, args);
                inThrottle = true;
                setTimeout(() => inThrottle = false, limit);
            }
        };
    };

    // LocalStorage with error handling
    window.UFIN.storage = {
        get(key, defaultValue = null) {
            try {
                const item = localStorage.getItem(key);
                return item ? JSON.parse(item) : defaultValue;
            } catch (e) {
                window.UFIN.logger.warn('LocalStorage get error:', e);
                return defaultValue;
            }
        },

        set(key, value) {
            try {
                localStorage.setItem(key, JSON.stringify(value));
                return true;
            } catch (e) {
                window.UFIN.logger.warn('LocalStorage set error:', e);
                return false;
            }
        },

        remove(key) {
            try {
                localStorage.removeItem(key);
                return true;
            } catch (e) {
                window.UFIN.logger.warn('LocalStorage remove error:', e);
                return false;
            }
        }
    };

    window.UFIN.logger.log('UFIN Utilities initialized');

})(window);
