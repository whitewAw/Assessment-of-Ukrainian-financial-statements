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
