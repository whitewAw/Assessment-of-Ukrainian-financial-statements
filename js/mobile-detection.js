/**
 * UFIN Financial Analysis Tool - Mobile Detection (Optimized)
 * Handles device detection and mobile-specific optimizations
 */

(function () {
    'use strict';

    const logger = window.UFIN?.logger || console;
    const device = window.UFIN?.device;

    // Use cached device info if available
    const isMobile = device?.isMobile || /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    const isIOS = device?.isIOS || /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
    const isAndroid = device?.isAndroid || /Android/i.test(navigator.userAgent);

    // Add device classes (batch DOM update)
    const deviceClasses = [];
    if (isMobile) deviceClasses.push('mobile-device');
    if (isIOS) deviceClasses.push('ios-device');
    if (isAndroid) deviceClasses.push('android-device');
    if (device?.isTouch) deviceClasses.push('touch-device');
    else deviceClasses.push('mouse-device');

    document.body.classList.add(...deviceClasses);
    logger.log('Device classes:', deviceClasses.join(', '));

    // iOS specific optimizations
    if (isIOS) {
        document.addEventListener('touchstart', function () {
            const meta = document.querySelector('meta[name="viewport"]');
            if (meta) {
                meta.content = 'width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no, viewport-fit=cover';
            }
        }, { passive: true, once: true });
    }

    // Prevent double-tap zoom (throttled)
    let lastTouchEnd = 0;
    document.addEventListener('touchend', function (event) {
        const now = Date.now();
        if (now - lastTouchEnd <= 300) {
            event.preventDefault();
        }
        lastTouchEnd = now;
    }, { passive: false });

    // Handle orientation changes (debounced)
    const handleOrientationChange = window.UFIN?.debounce(() => {
        const orientation = window.orientation === 0 || window.orientation === 180 ? 'portrait' : 'landscape';
        logger.log('Orientation:', orientation);
        window.dispatchEvent(new Event('resize'));
    }, 100);

    window.addEventListener('orientationchange', handleOrientationChange);

    // Log device info (development only)
    if (window.UFIN?.logger) {
        logger.table(device?.info || {
            userAgent: navigator.userAgent,
            screenWidth: window.screen.width,
            screenHeight: window.screen.height,
            devicePixelRatio: window.devicePixelRatio
        });
    }
})();
