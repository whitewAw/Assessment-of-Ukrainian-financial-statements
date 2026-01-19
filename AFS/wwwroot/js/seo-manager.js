/**
 * SEO Manager for UFIN (Ukrainian Financial Statement Analysis) Blazor WebAssembly Application
 * Handles dynamic meta tag updates for better search engine indexing
 * Works with Blazor routing to ensure proper canonical URLs and meta tags
 * Optimized for Google Search, Bing, and international search engines
 * 
 * @version 2.1.0
 * @author UFIN Development Team
 * @license MIT
 */

class SEOManager {
    constructor() {
        this.baseUrl = window.location.origin;
        this.basePath = '/Assessment-of-Ukrainian-financial-statements/';
        this.siteName = 'UFIN - Ukrainian Financial Statement Analysis';
        this.defaultImage = `${this.baseUrl}${this.basePath}icon-512.png`;
        this.initialized = false;
        this.lastUrl = null;
        
        // Track page views for analytics
        this.pageViews = 0;
        
        // Cache for page metadata
        this.metaCache = new Map();
        
        // Prerender hints tracking
        this.prerenderHints = new Set();
    }

    /**
     * Initialize SEO Manager - should be called after Blazor loads
     */
    init() {
        console.log('[SEO] Initializing Advanced SEO Manager v2.1');

        // Set initial canonical URL and meta tags
        this.updateCanonicalUrl();
        this.updateMetaTags();
        this.updateOpenGraph();
        this.updateStructuredData();
        this.addSpeculationRules();

        // Listen to Blazor navigation events
        this.setupBlazorNavigation();

        // Handle browser back/forward
        window.addEventListener('popstate', () => this.handleNavigation());

        // Update on hash changes (for client-side routing)
        window.addEventListener('hashchange', () => this.handleNavigation());

        // Handle language changes
        window.addEventListener('language-changed', (e) => this.handleLanguageChange(e.detail));

        // Set up intersection observer for prerendering hints
        this.setupPrerenderObserver();

        this.initialized = true;
        console.log('[SEO] Advanced SEO Manager initialized successfully');
    }

    /**
     * Add Speculation Rules for faster page navigation
     * Uses the Speculation Rules API for prerendering
     */
    addSpeculationRules() {
        // Check if Speculation Rules API is supported
        if (!('HTMLScriptElement' in window) || !document.createElement('script').supports?.('speculationrules')) {
            console.log('[SEO] Speculation Rules API not supported');
            return;
        }

        // Remove existing speculation rules
        const existingRules = document.querySelector('script[type="speculationrules"]');
        if (existingRules) {
            existingRules.remove();
        }

        const speculationRules = {
            prerender: [
                {
                    where: {
                        and: [
                            { href_matches: `${this.basePath}*` },
                            { not: { href_matches: `${this.basePath}_framework/*` } },
                            { not: { href_matches: `${this.basePath}_content/*` } }
                        ]
                    },
                    eagerness: "moderate"
                }
            ],
            prefetch: [
                {
                    where: {
                        href_matches: `${this.basePath}*`
                    },
                    eagerness: "conservative"
                }
            ]
        };

        const script = document.createElement('script');
        script.type = 'speculationrules';
        script.textContent = JSON.stringify(speculationRules);
        document.head.appendChild(script);

        console.log('[SEO] Speculation Rules added for faster navigation');
    }

    /**
     * Setup intersection observer for prerendering hints on visible links
     */
    setupPrerenderObserver() {
        if (!('IntersectionObserver' in window)) {
            return;
        }

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const link = entry.target;
                    const href = link.getAttribute('href');
                    if (href && !this.prerenderHints.has(href) && href.startsWith(this.basePath)) {
                        this.addPrefetchHint(href);
                        this.prerenderHints.add(href);
                    }
                }
            });
        }, { rootMargin: '100px' });

        // Observe all internal links
        document.querySelectorAll('a[href^="/"]').forEach(link => {
            observer.observe(link);
        });
    }

    /**
     * Add prefetch hint for a URL
     */
    addPrefetchHint(url) {
        const existingLink = document.querySelector(`link[rel="prefetch"][href="${url}"]`);
        if (existingLink) return;

        const link = document.createElement('link');
        link.rel = 'prefetch';
        link.href = url;
        link.as = 'document';
        document.head.appendChild(link);
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
                console.log('[SEO] URL changed via MutationObserver:', currentUrl);
                this.handleNavigation();
            }
        });

        // Observe document changes
        observer.observe(document.documentElement, {
            subtree: true,
            childList: true,
            attributes: true,
            attributeFilter: ['href', 'content', 'title']
        });
    }

    /**
     * Handle navigation changes
     */
    handleNavigation() {
        console.log('[SEO] Handling navigation to:', window.location.pathname);

        this.pageViews++;

        // Update all SEO elements
        this.updateCanonicalUrl();
        this.updateMetaTags();
        this.updateOpenGraph();
        this.updateTwitterCard();
        this.updateStructuredData();
        this.updateAlternateLanguages();
        this.updateArticleSchema();

        // Notify search engines and analytics of navigation
        this.notifyPageView();

        // Update prerender hints for new page
        setTimeout(() => this.setupPrerenderObserver(), 500);
    }

    /**
     * Handle language change events
     */
    handleLanguageChange(lang) {
        console.log('[SEO] Language changed to:', lang);
        
        // Update lang attribute
        document.documentElement.lang = lang || 'en';
        
        // Update hreflang tags
        this.updateAlternateLanguages(lang);
        
        // Update content language
        this.setMetaTag('content-language', lang || 'en', 'http-equiv');
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
        let path = window.location.pathname;
        const search = window.location.search;

        // Normalize path
        if (!path.startsWith(this.basePath)) {
            path = this.basePath + path.replace(/^\//, '');
        }

        // Remove trailing slash for consistency (except root)
        path = path.replace(/\/$/, '');
        if (!path || path === this.basePath.replace(/\/$/, '')) {
            path = this.basePath;
        }

        // Build canonical URL (without query params for cleaner URLs)
        const canonicalUrl = `${this.baseUrl}${path}`;
        return canonicalUrl;
    }

    /**
     * Update meta tags based on current content
     */
    updateMetaTags() {
        const path = window.location.pathname;

        // Extract page information from URL
        const pageInfo = this.getPageInfo(path);

        // Update document title
        document.title = pageInfo.title;

        // Update meta description
        this.setMetaTag('description', pageInfo.description);

        // Update keywords
        this.setMetaTag('keywords', pageInfo.keywords);

        // Update author
        this.setMetaTag('author', 'UFIN Financial Tools');

        // Update category
        this.setMetaTag('category', pageInfo.category);

        // Update subject and abstract for enhanced SEO
        this.setMetaTag('subject', pageInfo.title);
        this.setMetaTag('abstract', pageInfo.description.substring(0, 200));

        // Update page type for analytics
        this.setMetaTag('page-type', pageInfo.pageType || 'tool');

        console.log('[SEO] Meta tags updated for:', path);
    }

    /**
     * Get comprehensive page-specific information based on URL
     */
    getPageInfo(path) {
        // Check cache first
        const cacheKey = path.toLowerCase();
        if (this.metaCache.has(cacheKey)) {
            return this.metaCache.get(cacheKey);
        }

        // Remove base path and normalize
        const cleanPath = path
            .replace(this.basePath, '')
            .replace(/^\//, '')
            .replace(/\/$/, '')
            .toLowerCase();

        // Comprehensive page metadata with SEO-optimized titles and descriptions
        const pageDescriptions = {
            '': {
                title: 'UFIN - Free Ukrainian Financial Statement Analysis Tool | AI-Powered Business Assessment',
                description: 'Professional financial analysis tool for Ukrainian businesses. FREE AI-powered analysis of balance sheets, income statements. Calculate liquidity ratios, solvency metrics, profitability indicators. 16 comprehensive tables + 7 interactive charts. No registration. Works offline. Supports 6 languages including Ukrainian and English.',
                keywords: 'Ukrainian financial analysis, AI financial analysis, balance sheet analyzer Ukraine, income statement analysis, liquidity ratios calculator, solvency assessment tool, profitability metrics, ROA calculator, ROE calculator, free financial tools, Ukrainian accounting software, business financial health, financial statement analysis, accounting analysis Ukraine, financial metrics calculator, working capital analysis, debt to equity ratio, current ratio calculator, quick ratio calculator, financial stability assessment, business analysis tool, Ukrainian business tools, free accounting software, financial reporting Ukraine',
                category: 'Finance, Business Tools, Accounting Software, AI Tools',
                pageType: 'home',
                image: this.defaultImage
            },
            'aiassistant': {
                title: 'AI Financial Assistant - UFIN | Free AI-Powered Financial Analysis',
                description: 'Revolutionary AI-powered financial analysis using Chrome AI (Gemini Nano). Ask questions about your Ukrainian company finances in natural language. Get instant AI analysis of liquidity, solvency, profitability. 100% private, runs locally in browser. No data sent to servers. Free AI financial advisor for Ukrainian businesses.',
                keywords: 'AI financial analysis, AI financial advisor, Gemini Nano financial tool, Chrome AI finance, AI business analysis, AI accounting assistant, AI financial chatbot, machine learning finance, AI profitability analysis, AI solvency assessment, conversational AI finance, local AI financial advisor, private AI analysis, free AI financial tools, AI-powered accounting',
                category: 'AI Tools, Financial AI, Business Intelligence',
                pageType: 'ai-tool',
                image: this.defaultImage
            },
            'characteristicsofcapital': {
                title: 'Capital Structure Analysis - UFIN | Working Capital Efficiency Calculator',
                description: 'Analyze capital structure and working capital efficiency for Ukrainian businesses. Calculate capital turnover ratios, working capital productivity, return on capital employed. Free professional tool with detailed calculations and AI insights.',
                keywords: 'capital structure analysis, working capital analysis, capital efficiency calculator, capital turnover ratio, working capital productivity, return on capital, capital composition Ukraine, capital efficiency metrics, working capital management, capital structure ratios',
                category: 'Financial Analysis, Capital Management',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'indicatorsofturnoverofcurrentassets': {
                title: 'Current Assets Turnover Analysis - UFIN | Asset Velocity Calculator',
                description: 'Calculate current assets turnover ratios for Ukrainian companies. Analyze inventory turnover, receivables turnover, asset velocity. Free tool with comprehensive calculations and trend analysis.',
                keywords: 'current assets turnover, inventory turnover ratio, receivables turnover, asset velocity, current assets analysis, inventory management ratios, accounts receivable turnover, asset efficiency metrics, working capital turnover',
                category: 'Financial Analysis, Asset Management',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'factorsaffectingturnoverofworkingcapital': {
                title: 'Working Capital Turnover Factors - UFIN | Factor Analysis Tool',
                description: 'Detailed factor analysis of working capital turnover for Ukrainian businesses. Identify key drivers affecting working capital efficiency. Free comprehensive analysis with AI-powered insights.',
                keywords: 'working capital factors, turnover factor analysis, working capital drivers, capital efficiency factors, working capital analysis, factor impact analysis, capital turnover drivers, working capital management',
                category: 'Financial Analysis, Working Capital',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'indicatorsofefficiencyofworkingcapital': {
                title: 'Working Capital Efficiency Metrics - UFIN | ROI Calculator',
                description: 'Calculate working capital efficiency indicators for Ukrainian companies. Measure ROI, productivity, turnover ratios. Free professional analysis tool with detailed calculations.',
                keywords: 'working capital efficiency, working capital ROI, capital productivity, working capital metrics, capital efficiency ratios, working capital analysis, capital utilization, efficiency indicators',
                category: 'Financial Analysis, Efficiency Metrics',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'availabilityandmovementoffixedassets': {
                title: 'Fixed Assets Analysis - UFIN | Asset Lifecycle Tracking',
                description: 'Track availability and movement of fixed assets for Ukrainian businesses. Analyze asset acquisitions, disposals, depreciation. Free comprehensive fixed assets management tool.',
                keywords: 'fixed assets analysis, asset movement tracking, fixed assets management, asset lifecycle, depreciation analysis, asset acquisition tracking, fixed assets Ukraine, asset management tool, capital assets analysis',
                category: 'Financial Analysis, Asset Management',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'indicatorsofstateandmovementoffixedassets': {
                title: 'Fixed Assets Quality Metrics - UFIN | Asset Condition Analysis',
                description: 'Analyze state and movement of fixed assets for Ukrainian companies. Calculate wear ratio, renewal coefficient, retirement rate. Free professional fixed assets analysis.',
                keywords: 'fixed assets quality, asset condition analysis, wear ratio calculator, renewal coefficient, asset retirement rate, fixed assets metrics, asset quality indicators, depreciation metrics',
                category: 'Financial Analysis, Asset Quality',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'calculationofindicatorsofefficiencyofuseoffixedassets': {
                title: 'Fixed Assets Efficiency Calculator - UFIN | Asset Productivity Metrics',
                description: 'Calculate efficiency indicators for fixed assets use. Measure asset productivity, capital intensity, return on fixed assets for Ukrainian businesses. Free comprehensive analysis.',
                keywords: 'fixed assets efficiency, asset productivity calculator, capital intensity ratio, return on fixed assets, asset utilization, fixed assets ROI, productivity metrics, asset efficiency ratios',
                category: 'Financial Analysis, Asset Efficiency',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'factoranalysisoffixedassets': {
                title: 'Fixed Assets Factor Analysis - UFIN | Multifactor Impact Study',
                description: 'Comprehensive factor analysis of fixed assets for Ukrainian companies. Identify key drivers of asset productivity and efficiency. Free multifactor analysis tool with AI insights.',
                keywords: 'fixed assets factor analysis, asset productivity factors, multifactor analysis, asset efficiency drivers, factor impact study, fixed assets analysis, productivity drivers',
                category: 'Financial Analysis, Factor Analysis',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'indicatorsofefficiencyofuseofintangibleassets': {
                title: 'Intangible Assets Efficiency - UFIN | IP & Goodwill Analysis',
                description: 'Analyze efficiency of intangible assets for Ukrainian businesses. Calculate ROI on intellectual property, goodwill, patents. Free professional intangible assets analysis.',
                keywords: 'intangible assets analysis, IP efficiency, goodwill analysis, intangible assets ROI, intellectual property metrics, patent analysis, brand value analysis, intangible assets calculator',
                category: 'Financial Analysis, Intangible Assets',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'sourcesofcapitalformation': {
                title: 'Capital Formation Sources - UFIN | Funding Sources Analysis',
                description: 'Analyze sources of capital formation for Ukrainian companies. Breakdown of equity, debt, retained earnings. Free comprehensive capital structure analysis with interactive charts.',
                keywords: 'capital formation sources, funding sources analysis, capital structure, equity analysis, debt analysis, retained earnings, capital sources breakdown, financing structure',
                category: 'Financial Analysis, Capital Structure',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'assessmentofreceivableandpayable': {
                title: 'Receivables & Payables Analysis - UFIN | Credit Management Tool',
                description: 'Assess receivables and payables for Ukrainian businesses. Calculate collection periods, payment terms, credit metrics. Free comprehensive credit management analysis.',
                keywords: 'receivables analysis, payables analysis, credit management, collection period calculator, payment terms analysis, accounts receivable metrics, accounts payable metrics, credit analysis tool',
                category: 'Financial Analysis, Credit Management',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'indicatorsofbusinessactivity': {
                title: 'Business Activity Indicators - UFIN | Operational Efficiency Analysis',
                description: 'Calculate business activity indicators for Ukrainian companies. Analyze asset turnover, inventory cycles, operational efficiency. Free comprehensive operational analysis.',
                keywords: 'business activity ratios, operational efficiency, asset turnover calculator, inventory turnover, business cycle analysis, operational metrics, activity indicators, efficiency ratios',
                category: 'Financial Analysis, Business Activity',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'liquidityindicatorsofbalance': {
                title: 'Liquidity Ratios Calculator - UFIN | Short-term Solvency Analysis',
                description: 'Calculate liquidity indicators for Ukrainian businesses. Current ratio, quick ratio, cash ratio calculator. Free professional liquidity analysis with AI-powered insights.',
                keywords: 'liquidity ratios calculator, current ratio calculator, quick ratio calculator, cash ratio calculator, short-term solvency, liquidity analysis, working capital ratios, liquidity metrics Ukraine',
                category: 'Financial Analysis, Liquidity',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'solvencyratios': {
                title: 'Solvency Ratios Calculator - UFIN | Long-term Financial Health Analysis',
                description: 'Calculate solvency ratios for Ukrainian companies. Debt-to-equity ratio, interest coverage, financial leverage analysis. Free comprehensive solvency assessment tool.
',
                keywords: 'solvency ratios calculator, debt to equity ratio, interest coverage calculator, financial leverage analysis, long-term solvency, debt analysis, solvency metrics, financial health calculator',
                category: 'Financial Analysis, Solvency',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'generalindicatorsoffinancialstability': {
                title: 'Financial Stability Indicators - UFIN | Stability Metrics Calculator',
                description: 'Calculate general indicators of financial stability for Ukrainian businesses. Autonomy coefficient, financial independence, stability metrics. Free professional analysis.',
                keywords: 'financial stability indicators, autonomy coefficient, financial independence, stability metrics calculator, financial stability analysis, stability ratios, autonomy ratio, independence metrics',
                category: 'Financial Analysis, Financial Stability',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'indicatorsoffinancialstability': {
                title: 'Detailed Financial Stability Analysis - UFIN | Comprehensive Stability Metrics',
                description: 'In-depth analysis of financial stability indicators for Ukrainian companies. Complete stability metrics breakdown with year-over-year comparisons. Free professional tool.',
                keywords: 'financial stability analysis, stability indicators, comprehensive stability metrics, financial health indicators, stability assessment, financial independence ratios',
                category: 'Financial Analysis, Financial Stability',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'classificationoftypesoffinancialstability': {
                title: 'Financial Stability Classification - UFIN | Risk Categorization',
                description: 'Classify financial stability types for Ukrainian companies. 4-tier stability assessment, risk categorization. Free comprehensive stability classification tool.',
                keywords: 'financial stability classification, stability types, risk categorization, stability assessment, financial risk levels, stability tiers, financial health categories, risk classification',
                category: 'Financial Analysis, Risk Assessment',
                pageType: 'analysis-table',
                image: this.defaultImage
            },
            'compositionofassetsbase': {
                title: 'Asset Composition Chart (Base Year) - UFIN | Asset Structure Visualization',
                description: 'Interactive pie chart showing asset composition for base year. Visualize current vs non-current assets for Ukrainian businesses. Free financial visualization tool.',
                keywords: 'asset composition chart, asset structure visualization, asset breakdown, current assets chart, non-current assets, asset visualization, financial charts, balance sheet visualization',
                category: 'Charts, Financial Visualization',
                pageType: 'chart',
                image: this.defaultImage
            },
            'compositionofassetscurrent': {
                title: 'Asset Composition Chart (Current Year) - UFIN | Current Asset Structure',
                description: 'Interactive pie chart showing current year asset composition. Compare asset structure year-over-year for Ukrainian companies. Free visualization tool.',
                keywords: 'current year assets, asset composition current, asset structure chart, year-over-year comparison, current assets visualization, asset breakdown chart',
                category: 'Charts, Financial Visualization',
                pageType: 'chart',
                image: this.defaultImage
            },
            'sourcesofcapitalformationbase': {
                title: 'Capital Sources Chart (Base Year) - UFIN | Funding Structure Visualization',
                description: 'Interactive chart showing capital formation sources for base year. Visualize equity, debt, retained earnings for Ukrainian businesses. Free chart tool.',
                keywords: 'capital sources chart, funding structure visualization, capital formation chart, equity debt visualization, capital structure chart, financing sources',
                category: 'Charts, Financial Visualization',
                pageType: 'chart',
                image: this.defaultImage
            },
            'sourcesofcapitalformationcurrent': {
                title: 'Capital Sources Chart (Current Year) - UFIN | Current Funding Structure',
                description: 'Interactive chart showing current year capital sources. Compare capital structure changes for Ukrainian companies. Free visualization tool.',
                keywords: 'current capital sources, capital structure current year, funding visualization, capital formation chart, financing structure chart',
                category: 'Charts, Financial Visualization',
                pageType: 'chart',
                image: this.defaultImage
            },
            'structureofaccountspayablebase': {
                title: 'Accounts Payable Chart (Base Year) - UFIN | Liability Breakdown',
                description: 'Interactive chart showing accounts payable structure for base year. Visualize liability composition for Ukrainian businesses. Free chart tool.',
                keywords: 'accounts payable chart, liability breakdown, payables structure, accounts payable visualization, liability composition chart, payables analysis',
                category: 'Charts, Financial Visualization',
                pageType: 'chart',
                image: this.defaultImage
            },
            'structureofaccountspayablecurrent': {
                title: 'Accounts Payable Chart (Current Year) - UFIN | Current Liability Structure',
                description: 'Interactive chart showing current year accounts payable structure. Track liability changes for Ukrainian companies. Free visualization.',
                keywords: 'current accounts payable, liability structure current, payables breakdown chart, current liabilities visualization',
                category: 'Charts, Financial Visualization',
                pageType: 'chart',
                image: this.defaultImage
            },
            'workingcapitalturnovertime': {
                title: 'Working Capital Turnover Time Chart - UFIN | Efficiency Trends',
                description: 'Interactive line chart showing working capital turnover time trends. Track efficiency improvements for Ukrainian businesses. Free trend analysis.',
                keywords: 'working capital turnover chart, efficiency trends, turnover time analysis, working capital visualization, efficiency chart, trend analysis',
                category: 'Charts, Financial Visualization',
                pageType: 'chart',
                image: this.defaultImage
            },
            'help': {
                title: 'Help & Documentation - UFIN | Financial Analysis Tutorials & FAQs',
                description: 'Comprehensive help and documentation for UFIN. Find tutorials, FAQs, user guides for financial analysis. Learn how to calculate ratios, interpret results, use AI features.',
                keywords: 'UFIN help, financial analysis tutorials, FAQs, user guide, financial ratios help, analysis documentation, how to use UFIN, financial analysis guide',
                category: 'Documentation, Help',
                pageType: 'help',
                image: this.defaultImage
            },
            'about': {
                title: 'About UFIN - Free Ukrainian Financial Analysis Tool | Open Source Project',
                description: 'Learn about UFIN, the free AI-powered financial analysis tool for Ukrainian businesses. Open-source project, MIT license, no registration required. Works offline, supports 6 languages. Built with .NET 10 Blazor WebAssembly.',
                keywords: 'about UFIN, Ukrainian financial tool, open source financial analysis, free accounting software, Blazor financial app, .NET financial tool, open source accounting',
                category: 'About, Information',
                pageType: 'about',
                image: this.defaultImage
            }
        };

        // Return page-specific info or default
        const info = pageDescriptions[cleanPath] || pageDescriptions[''];
        
        // Cache the result
        this.metaCache.set(cacheKey, info);
        
        return info;
    }

    /**
     * Update OpenGraph tags for social sharing
     */
    updateOpenGraph() {
        const pageInfo = this.getPageInfo(window.location.pathname);
        const canonicalUrl = this.getCanonicalUrl();

        this.setMetaTag('og:title', pageInfo.title, 'property');
        this.setMetaTag('og:description', pageInfo.description, 'property');
        this.setMetaTag('og:url', canonicalUrl, 'property');
        this.setMetaTag('og:type', 'website', 'property');
        this.setMetaTag('og:image', pageInfo.image || this.defaultImage, 'property');
        this.setMetaTag('og:image:secure_url', pageInfo.image || this.defaultImage, 'property');
        this.setMetaTag('og:image:width', '512', 'property');
        this.setMetaTag('og:image:height', '512', 'property');
        this.setMetaTag('og:image:alt', pageInfo.title, 'property');
        this.setMetaTag('og:site_name', this.siteName, 'property');
        this.setMetaTag('og:locale', 'en_US', 'property');
        this.setMetaTag('og:locale:alternate', 'uk_UA', 'property');
        this.setMetaTag('og:updated_time', new Date().toISOString(), 'property');
        
        console.log('[SEO] OpenGraph tags updated');
    }

    /**
     * Update Twitter Card tags
     */
    updateTwitterCard() {
        const pageInfo = this.getPageInfo(window.location.pathname);
        const canonicalUrl = this.getCanonicalUrl();

        this.setMetaTag('twitter:card', 'summary_large_image');
        this.setMetaTag('twitter:title', pageInfo.title);
        this.setMetaTag('twitter:description', pageInfo.description);
        this.setMetaTag('twitter:url', canonicalUrl);
        this.setMetaTag('twitter:image', pageInfo.image || this.defaultImage);
        this.setMetaTag('twitter:image:alt', pageInfo.title);
        this.setMetaTag('twitter:site', '@wAw_fromUkraine');
        this.setMetaTag('twitter:creator', '@wAw_fromUkraine');
        this.setMetaTag('twitter:domain', 'whitewaw.github.io');
        
        console.log('[SEO] Twitter Card tags updated');
    }

    /**
     * Update alternate language links for international SEO
     */
    updateAlternateLanguages(currentLang = 'en') {
        const path = window.location.pathname.replace(this.basePath, '').replace(/^\//, '');
        const languages = [
            { code: 'en', name: 'English' },
            { code: 'uk', name: 'Ukrainian' },
            { code: 'ru', name: 'Russian' },
            { code: 'es', name: 'Spanish' },
            { code: 'de', name: 'German' },
            { code: 'fr', name: 'French' }
        ];

        // Remove existing alternate links (except regional variants)
        document.querySelectorAll('link[rel="alternate"][hreflang]').forEach(link => {
            const hreflang = link.getAttribute('hreflang');
            if (hreflang && !hreflang.includes('-') && hreflang !== 'x-default') {
                link.remove();
            }
        });

        // Add new alternate links
        languages.forEach(lang => {
            const existingLink = document.querySelector(`link[rel="alternate"][hreflang="${lang.code}"]`);
            if (!existingLink) {
                const link = document.createElement('link');
                link.rel = 'alternate';
                link.hreflang = lang.code;
                link.href = `${this.baseUrl}${this.basePath}${path}${path ? '?' : '?'}lang=${lang.code}`;
                document.head.appendChild(link);
            }
        });

        // Add x-default if not exists
        const xDefaultLink = document.querySelector('link[rel="alternate"][hreflang="x-default"]');
        if (!xDefaultLink) {
            const defaultLink = document.createElement('link');
            defaultLink.rel = 'alternate';
            defaultLink.hreflang = 'x-default';
            defaultLink.href = `${this.baseUrl}${this.basePath}${path}`;
            document.head.appendChild(defaultLink);
        }
        
        console.log('[SEO] Alternate language links updated');
    }

    /**
     * Update structured data (JSON-LD) for rich snippets
     */
    updateStructuredData() {
        const path = window.location.pathname;
        const pageInfo = this.getPageInfo(path);

        // Update BreadcrumbList schema
        this.updateBreadcrumbSchema(pageInfo);

        // Update WebPage schema
        this.updateWebPageSchema(pageInfo);
        
        console.log('[SEO] Structured data updated');
    }

    /**
     * Update Article schema for content pages
     */
    updateArticleSchema() {
        const path = window.location.pathname;
        const pageInfo = this.getPageInfo(path);
        
        // Only add article schema for analysis pages
        if (!pageInfo.pageType || pageInfo.pageType === 'home') {
            // Remove article schema if exists
            const existingSchema = document.getElementById('article-schema');
            if (existingSchema) existingSchema.remove();
            return;
        }

        let schema = document.getElementById('article-schema');
        if (!schema) {
            schema = document.createElement('script');
            schema.id = 'article-schema';
            schema.type = 'application/ld+json';
            document.head.appendChild(schema);
        }

        const articleSchema = {
            "@context": "https://schema.org",
            "@type": "TechArticle",
            "headline": pageInfo.title,
            "description": pageInfo.description,
            "url": this.getCanonicalUrl(),
            "datePublished": "2022-01-01",
            "dateModified": new Date().toISOString(),
            "author": {
                "@type": "Organization",
                "name": "UFIN Development Team",
                "url": "https://github.com/whitewAw"
            },
            "publisher": {
                "@type": "Organization",
                "name": "UFIN",
                "logo": {
                    "@type": "ImageObject",
                    "url": this.defaultImage
                }
            },
            "image": pageInfo.image || this.defaultImage,
            "mainEntityOfPage": {
                "@type": "WebPage",
                "@id": this.getCanonicalUrl()
            },
            "keywords": pageInfo.keywords,
            "articleSection": pageInfo.category,
            "inLanguage": document.documentElement.lang || "en",
            "isAccessibleForFree": true,
            "about": {
                "@type": "Thing",
                "name": "Financial Analysis",
                "description": pageInfo.description.substring(0, 100)
            }
        };

        schema.textContent = JSON.stringify(articleSchema, null, 2);
        console.log('[SEO] Article schema updated');
    }

    /**
     * Update BreadcrumbList schema for navigation
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
        }, null, 2);
    }

    /**
     * Update WebPage schema
     */
    updateWebPageSchema(pageInfo) {
        let schema = document.getElementById('webpage-schema');
        if (!schema) {
            schema = document.createElement('script');
            schema.id = 'webpage-schema';
            schema.type = 'application/ld+json';
            document.head.appendChild(schema);
        }

        schema.textContent = JSON.stringify({
            "@context": "https://schema.org",
            "@type": "WebPage",
            "@id": `${this.getCanonicalUrl()}#webpage`,
            "name": pageInfo.title,
            "description": pageInfo.description,
            "url": this.getCanonicalUrl(),
            "inLanguage": document.documentElement.lang || "en",
            "isPartOf": {
                "@type": "WebSite",
                "@id": `${this.baseUrl}${this.basePath}#website`,
                "name": this.siteName,
                "url": `${this.baseUrl}${this.basePath}`
            },
            "about": {
                "@type": "Thing",
                "name": "Financial Analysis",
                "description": "Ukrainian Financial Statement Analysis"
            },
            "primaryImageOfPage": {
                "@type": "ImageObject",
                "url": pageInfo.image || this.defaultImage,
                "width": 512,
                "height": 512
            },
            "datePublished": "2022-01-01",
            "dateModified": new Date().toISOString(),
            "author": {
                "@type": "Organization",
                "name": "UFIN Development Team"
            },
            "publisher": {
                "@type": "Organization",
                "name": "UFIN",
                "logo": {
                    "@type": "ImageObject",
                    "url": this.defaultImage
                }
            },
            "speakable": {
                "@type": "SpeakableSpecification",
                "cssSelector": ["h1", "h2", ".description", "#seo-content"]
            },
            "breadcrumb": {
                "@id": `${this.getCanonicalUrl()}#breadcrumb`
            }
        }, null, 2);
    }

    /**
     * Generate breadcrumb schema items
     */
    generateBreadcrumbs() {
        const path = window.location.pathname
            .replace(this.basePath, '')
            .split('/')
            .filter(p => p);
        
        const breadcrumbs = [{
            "@type": "ListItem",
            "position": 1,
            "name": "Home",
            "item": `${this.baseUrl}${this.basePath}`
        }];

        let currentPath = this.basePath;
        path.forEach((segment, index) => {
            currentPath += segment + '/';
            const name = segment
                .split(/(?=[A-Z])/)
                .join(' ')
                .replace(/^\w/, c => c.toUpperCase());
            
            breadcrumbs.push({
                "@type": "ListItem",
                "position": index + 2,
                "name": name,
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
    registerPage(title, description, keywords = null) {
        // Update page title
        if (title) {
            document.title = title;
        }

        // Update meta description
        if (description) {
            this.setMetaTag('description', description);
        }

        // Update keywords
        if (keywords) {
            this.setMetaTag('keywords', keywords);
        }

        // Trigger general update
        this.handleNavigation();

        console.log('[SEO] Page manually registered:', title);
    }

    /**
     * Notify analytics and search engines of page view
     */
    notifyPageView() {
        const url = this.getCanonicalUrl();
        console.log('[SEO] Page view notified:', url);

        // Google Analytics 4
        if (window.gtag) {
            window.gtag('event', 'page_view', {
                page_location: url,
                page_path: window.location.pathname,
                page_title: document.title
            });
        }

        // Google Analytics Universal (legacy)
        if (window.ga) {
            window.ga('send', 'pageview', window.location.pathname);
        }

        // Microsoft Clarity
        if (window.clarity) {
            window.clarity('set', 'page', window.location.pathname);
        }

        // Yandex Metrica
        if (window.ym) {
            window.ym(88888888, 'hit', url); // Replace with your actual Yandex Metrica ID
        }
    }

    /**
     * Get SEO health score for current page
     * @returns {Object} SEO health metrics
     */
    getSEOHealth() {
        const health = {
            score: 0,
            issues: [],
            warnings: [],
            passed: []
        };

        // Check title
        const title = document.title;
        if (title && title.length >= 30 && title.length <= 60) {
            health.passed.push('Title length optimal (30-60 chars)');
            health.score += 15;
        } else if (title && title.length > 0) {
            health.warnings.push(`Title length ${title.length} (optimal: 30-60 chars)`);
            health.score += 5;
        } else {
            health.issues.push('Missing or empty title');
        }

        // Check description
        const description = document.querySelector('meta[name="description"]')?.content;
        if (description && description.length >= 120 && description.length <= 160) {
            health.passed.push('Description length optimal (120-160 chars)');
            health.score += 15;
        } else if (description && description.length > 0) {
            health.warnings.push(`Description length ${description.length} (optimal: 120-160 chars)`);
            health.score += 5;
        } else {
            health.issues.push('Missing or empty description');
        }

        // Check canonical
        const canonical = document.getElementById('canonical');
        if (canonical && canonical.href) {
            health.passed.push('Canonical URL present');
            health.score += 10;
        } else {
            health.issues.push('Missing canonical URL');
        }

        // Check OpenGraph
        const ogTitle = document.querySelector('meta[property="og:title"]');
        const ogDesc = document.querySelector('meta[property="og:description"]');
        const ogImage = document.querySelector('meta[property="og:image"]');
        if (ogTitle && ogDesc && ogImage) {
            health.passed.push('OpenGraph tags complete');
            health.score += 10;
        } else {
            health.warnings.push('Incomplete OpenGraph tags');
            health.score += 5;
        }

        // Check Twitter Card
        const twitterCard = document.querySelector('meta[name="twitter:card"]');
        const twitterTitle = document.querySelector('meta[name="twitter:title"]');
        if (twitterCard && twitterTitle) {
            health.passed.push('Twitter Card tags present');
            health.score += 5;
        } else {
            health.warnings.push('Incomplete Twitter Card tags');
        }

        // Check structured data
        const schemas = document.querySelectorAll('script[type="application/ld+json"]');
        if (schemas.length >= 3) {
            health.passed.push(`${schemas.length} structured data schemas found`);
            health.score += 10;
        } else if (schemas.length > 0) {
            health.warnings.push(`Only ${schemas.length} schema found (recommend 3+)`);
            health.score += 5;
        }

        // Check mobile meta tags
        const viewport = document.querySelector('meta[name="viewport"]');
        if (viewport) {
            health.passed.push('Mobile viewport configured');
            health.score += 5;
        } else {
            health.issues.push('Missing viewport meta tag');
        }

        // Check alternate languages
        const alternates = document.querySelectorAll('link[rel="alternate"][hreflang]');
        if (alternates.length >= 6) {
            health.passed.push('Multi-language support configured');
            health.score += 5;
        }

        // Check speculation rules
        const speculationRules = document.querySelector('script[type="speculationrules"]');
        if (speculationRules) {
            health.passed.push('Speculation Rules for prerendering present');
            health.score += 5;
        }

        // Check preload hints
        const preloads = document.querySelectorAll('link[rel="preload"]');
        if (preloads.length >= 3) {
            health.passed.push(`${preloads.length} preload hints configured`);
            health.score += 5;
        }

        // Calculate final score
        health.score = Math.min(100, health.score);
        
        return health;
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
window.registerPageSeo = (title, description, keywords = null) => {
    if (window.seoManager) {
        window.seoManager.registerPage(title, description, keywords);
    }
};

// Export SEO health check
window.checkSEOHealth = () => {
    if (window.seoManager) {
        const health = window.seoManager.getSEOHealth();
        console.log('[SEO Health Report]', health);
        return health;
    }
    return null;
};

// Export prefetch hint function
window.addPrefetchHint = (url) => {
    if (window.seoManager) {
        window.seoManager.addPrefetchHint(url);
    }
};

console.log('[SEO] SEO Manager module loaded successfully');
