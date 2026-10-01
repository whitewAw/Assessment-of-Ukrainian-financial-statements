// Typed JS interop helpers used from .NET instead of IJSRuntime "eval".
// Keeping these as named functions allows a strict Content-Security-Policy
// without 'unsafe-eval'.
(function () {
    'use strict';

    const darkLightBackgrounds = [
        'background: white', 'background: #fff', 'background: #ffffff',
        'background-color: white', 'background-color: #fff', 'background-color: #ffffff',
        'background-color: rgb(255', 'background-color: #f', 'background-color: #e'
    ];

    window.ufinInterop = {
        prefersDarkScheme: function () {
            return !!(window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches);
        },

        applyTheme: function (theme, themeColor) {
            document.documentElement.setAttribute('data-theme', theme);
            const meta = document.querySelector('meta[name="theme-color"]');
            if (meta) {
                meta.setAttribute('content', themeColor);
            }
        },

        fixInlineDarkStyles: function () {
            document.querySelectorAll('[style*="background"]').forEach(function (el) {
                const style = el.getAttribute('style');
                if (style && darkLightBackgrounds.some(function (s) { return style.includes(s); })) {
                    el.style.backgroundColor = '#2d2d2d';
                }
            });
        },

        setDocumentTitle: function (title) {
            document.title = title;
        },

        scrollToBottom: function (selector) {
            const el = document.querySelector(selector);
            if (el) {
                el.scrollTop = el.scrollHeight;
            }
        },

        yieldToMain: function () {
            const scheduler = window.UFIN && window.UFIN.scheduler;
            return scheduler && scheduler.yieldToMain ? scheduler.yieldToMain() : Promise.resolve();
        },

        mountGitHubStarButton: function (containerId, src) {
            const container = document.getElementById(containerId);
            if (!container || container.querySelector('iframe')) {
                return;
            }
            const iframe = document.createElement('iframe');
            iframe.src = src;
            iframe.frameBorder = '0';
            iframe.scrolling = '0';
            iframe.width = '170';
            iframe.height = '30';
            iframe.title = 'GitHub Star Button';
            iframe.loading = 'lazy';
            container.appendChild(iframe);
        }
    };
})();
