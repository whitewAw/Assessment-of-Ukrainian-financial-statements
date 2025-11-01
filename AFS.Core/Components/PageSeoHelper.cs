using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AFS.Core.Components
{
    /// <summary>
    /// Helper component for managing SEO meta tags in Blazor pages
    /// Usage: @inject PageSeoHelper SeoHelper
    ///        await SeoHelper.SetPageSeoAsync("Page Title", "Page description");
    /// </summary>
    public class PageSeoHelper
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;

        public PageSeoHelper(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Set SEO metadata for the current page
        /// </summary>
        public async Task SetPageSeoAsync(string title, string description)
        {
            try
            {
                // Update title
                await _jsRuntime.InvokeVoidAsync("eval", $"document.title = '{JavaScriptEncode(title)}'");

                // Call SEO manager to update all meta tags
                await _jsRuntime.InvokeVoidAsync("registerPageSeo", title, description);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SEO Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get current canonical URL
        /// </summary>
        public string GetCanonicalUrl()
        {
            return _navigationManager.Uri.Replace(_navigationManager.BaseUri, "");
        }

        /// <summary>
        /// Encode string for JavaScript
        /// </summary>
        private static string JavaScriptEncode(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            return text.Replace("\\", "\\\\")
                .Replace("'", "\\'")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r");
        }
    }
}
