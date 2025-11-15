/**
 * SEO Manager for UFIN Blazor WebAssembly Application
 * Handles dynamic meta tag updates for better search engine indexing
 * Works with Blazor routing to ensure proper canonical URLs and meta tags
 */

class SEOManager {
    constructor() {
        this.baseUrl = window.location.origin;
        this.basePath = '/Assessment-of-Ukrainian-financial-statements/';
        this.initialized = false;
    }

    /**
     * Initialize SEO Manager - should be called after Blazor loads
     */
    init() {
        console.log('[SEO] Initializing SEO Manager');

        // Set initial canonical URL
        this.updateCanonicalUrl();

        // Listen to Blazor navigation events
        this.setupBlazorNavigation();

        // Handle browser back/forward
        window.addEventListener('popstate', () => this.handleNavigation());

        // Update on hash changes (for client-side routing)
        window.addEventListener('hashchange', () => this.handleNavigation());

        this.initialized = true;
        console.log('[SEO] SEO Manager initialized');
    }

    /**
     * Setup listeners for Blazor navigation
     * Blazor fires custom navigation events
     */
    setupBlazorNavigation() {
        // Listen to Blazor's navigation completion event
        window.addEventListener('blazor-router-changed', () => {
            console.log('[SEO] Blazor router changed');
            this.handleNavigation();
        });

        // Alternative: Use MutationObserver to detect route changes
        this.setupMutationObserver();
    }

    /**
     * Setup MutationObserver to detect Blazor route changes
     * This is a fallback if blazor-router-changed doesn't fire
     */
    setupMutationObserver() {
        const observer = new MutationObserver(() => {
            // Check if URL has changed
            const currentUrl = window.location.href;
            if (this.lastUrl !== currentUrl) {
                this.lastUrl = currentUrl;
                console.log('[SEO] URL changed:', currentUrl);
                this.handleNavigation();
            }
        });

        // Observe document changes
        observer.observe(document.documentElement, {
            subtree: true,
            childList: true,
            attributes: true,
            attributeFilter: ['href', 'content']
        });
    }

    /**
     * Handle navigation changes
     */
    handleNavigation() {
        console.log('[SEO] Handling navigation');

        this.updateCanonicalUrl();
        this.updateMetaTags();
        this.updateOpenGraph();
        this.updateStructuredData();

        // Notify search engines of navigation
        if (window.gtag) {
            window.gtag('event', 'page_view', {
                page_path: window.location.pathname
            });
        }
    }

    /**
     * Update canonical URL based on current location
     */
    updateCanonicalUrl() {
        const canonicalUrl = this.getCanonicalUrl();
        let canonicalLink = document.getElementById('canonical');

        if (!canonicalLink) {
            canonicalLink = document.createElement('link');
            canonicalLink.id = 'canonical';
            canonicalLink.rel = 'canonical';
            document.head.appendChild(canonicalLink);
        }

        canonicalLink.href = canonicalUrl;
        console.log('[SEO] Canonical URL updated:', canonicalUrl);
    }

    /**
     * Get canonical URL for current page
     */
    getCanonicalUrl() {
        const path = window.location.pathname;
        const search = window.location.search;

        // Remove trailing slash for consistency
        let cleanPath = path.replace(/\/$/, '');
        if (!cleanPath) cleanPath = '/';

        // Build canonical URL
        const canonicalUrl = `${this.baseUrl}${cleanPath}${search}`;
        return canonicalUrl;
    }

    /**
     * Update meta tags based on current content
     */
    updateMetaTags() {
        const path = window.location.pathname;

        // Extract page information from URL
        const pageInfo = this.getPageInfo(path);

        // Update or create meta description
        this.setMetaTag('description', pageInfo.description);

        // Update OG tags
        this.setMetaTag('og:title', pageInfo.title, 'property');
        this.setMetaTag('og:description', pageInfo.description, 'property');
        this.setMetaTag('og:url', this.getCanonicalUrl(), 'property');

        // Update Twitter Card
        this.setMetaTag('twitter:title', pageInfo.title);
        this.setMetaTag('twitter:description', pageInfo.description);

        console.log('[SEO] Meta tags updated for:', path);
    }

    /**
   * Get page-specific information based on URL
     */
    getPageInfo(path) {
        // Remove base path and leading slash
        const cleanPath = path
            .replace(this.basePath, '')
            .replace(/^\//, '')
            .toLowerCase();

        // Map routes to descriptions
        const pageDescriptions = {
            '': {
                title: 'UFIN - Free Ukrainian Financial Statement Analysis Tool',
                description: 'Free professional financial analysis tool for Ukrainian businesses. Analyze balance sheets, income statements, calculate liquidity ratios, solvency metrics, profitability indicators. No registration required. Works offline. Supports 6 languages.'
            },
            'analysis': {
                title: 'Financial Analysis - UFIN',
                description: 'Analyze financial statements with UFIN. Calculate key financial ratios for liquidity, solvency, profitability, and business activity. Professional-grade analysis for Ukrainian companies.'
            },
            'reports': {
                title: 'Financial Reports - UFIN',
                description: 'Generate detailed financial reports with UFIN. Export comprehensive analysis of your company financials including all calculated ratios and indicators.'
            },
            'about': {
                title: 'About UFIN - Ukrainian Financial Analysis Tool',
                description: 'Learn about UFIN, the free financial analysis tool for Ukrainian businesses. Open-source, no registration required, works offline, supports 6 languages.'
            },
            'help': {
                title: 'Help & Documentation - UFIN',
                description: 'Get help using UFIN. Find tutorials, FAQs, and documentation for financial analysis and ratio calculations.'
            }
        };

        // Return page-specific info or default
        return pageDescriptions[cleanPath] || pageDescriptions[''];
    }

    /**
     * Update OpenGraph tags
 */
    updateOpenGraph() {
        const pageInfo = this.getPageInfo(window.location.pathname);

        this.setMetaTag('og:title', pageInfo.title, 'property');
        this.setMetaTag('og:description', pageInfo.description, 'property');
        this.setMetaTag('og:url', this.getCanonicalUrl(), 'property');
        this.setMetaTag('og:type', 'website', 'property');
    }

    /**
       * Update structured data (JSON-LD)
       */
    updateStructuredData() {
        const path = window.location.pathname;
        const pageInfo = this.getPageInfo(path);

        // Update BreadcrumbList schema if not root
        if (path !== '/' && !path.includes(this.basePath + '/')) {
            this.updateBreadcrumbSchema(pageInfo);
        }
    }

    /**
     * Update BreadcrumbList schema
     */
    updateBreadcrumbSchema(pageInfo) {
        let schema = document.getElementById('breadcrumb-schema');
        if (!schema) {
            schema = document.createElement('script');
            schema.id = 'breadcrumb-schema';
            schema.type = 'application/ld+json';
            document.head.appendChild(schema);
        }

        const breadcrumbs = this.generateBreadcrumbs();
        schema.textContent = JSON.stringify({
            "@context": "https://schema.org",
            "@type": "BreadcrumbList",
            "itemListElement": breadcrumbs
        });
    }

    /**
 * Generate breadcrumb schema items
     */
    generateBreadcrumbs() {
        const path = window.location.pathname.replace(this.basePath, '').split('/').filter(p => p);
        const breadcrumbs = [{
            "@type": "ListItem",
            "position": 1,
            "name": "Home",
            "item": `${this.baseUrl}${this.basePath}`
        }];

        let currentPath = this.basePath;
        path.forEach((segment, index) => {
            currentPath += segment + '/';
            breadcrumbs.push({
                "@type": "ListItem",
                "position": index + 2,
                "name": segment.charAt(0).toUpperCase() + segment.slice(1),
                "item": `${this.baseUrl}${currentPath}`
            });
        });

        return breadcrumbs;
    }

    /**
     * Set or update a meta tag
     */
    setMetaTag(name, content, attribute = 'name') {
        if (!content) return;

        let tag = document.querySelector(`meta[${attribute}="${name}"]`);

        if (!tag) {
            tag = document.createElement('meta');
            tag.setAttribute(attribute, name);
            document.head.appendChild(tag);
        }

        tag.content = content;
    }

    /**
     * Register page for search engine indexing
     * Call this after Blazor renders new content
     */
    registerPage(title, description) {
        // Update page title
        if (title) {
            document.title = title;
        }

        // Update or add meta description
        if (description) {
            this.setMetaTag('description', description);
        }

        // Trigger general update
        this.updateMetaTags();

        console.log('[SEO] Page registered:', title);
    }

    /**
     * Notify Google Search Console of new page
     * (if using XML sitemap, this is less critical)
     */
    notifySearchEngines() {
        const url = this.getCanonicalUrl();
        console.log('[SEO] Notifying search engines about:', url);

        // Google doesn't require explicit notification for pages with canonical tags
        // But you can implement this if needed
    }
}

// Initialize SEO Manager when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.seoManager = new SEOManager();
        window.seoManager.init();
    });
} else {
    // DOM is already loaded
    window.seoManager = new SEOManager();
    window.seoManager.init();
}

// Also initialize when Blazor loads
window.addEventListener('ufin:loaded', () => {
    console.log('[SEO] UFIN loaded, re-initializing SEO Manager');
    if (window.seoManager) {
        window.seoManager.init();
    }
});

// Export for use in Blazor components
window.registerPageSeo = (title, description) => {
    if (window.seoManager) {
        window.seoManager.registerPage(title, description);
    }
};
