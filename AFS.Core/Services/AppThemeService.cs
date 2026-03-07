using Microsoft.JSInterop;

namespace AFS.Core.Services;

/// <summary>
/// Service for managing application theme (light/dark mode)
/// </summary>
public class AppThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = "light";

    public event Action? OnThemeChanged;

    public AppThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Gets the current theme
    /// </summary>
    public string CurrentTheme => _currentTheme;

    /// <summary>
    /// Initializes the theme from browser storage or system preference
    /// </summary>
    public async Task InitializeThemeAsync()
    {
        try
        {
            // Try to get saved theme from localStorage
            var savedTheme = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "theme");

            if (!string.IsNullOrEmpty(savedTheme) &&
                (string.Equals(savedTheme, "light", StringComparison.Ordinal) ||
                 string.Equals(savedTheme, "dark", StringComparison.Ordinal)))
            {
                _currentTheme = savedTheme;
            }
            else
            {
                // Check system preference
                var prefersDark = await _jsRuntime.InvokeAsync<bool>("eval",
          "window.matchMedia('(prefers-color-scheme: dark)').matches");
                _currentTheme = prefersDark ? "dark" : "light";
            }

            await ApplyThemeAsync(_currentTheme);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing theme: {ex.Message}");
            _currentTheme = "light";
        }
    }

    /// <summary>
    /// Toggles between light and dark theme
    /// </summary>
    public async Task ToggleThemeAsync()
    {
        var newTheme = string.Equals(_currentTheme, "light", StringComparison.Ordinal) ? "dark" : "light";
        await SetThemeAsync(newTheme);
    }

    /// <summary>
    /// Sets a specific theme
    /// </summary>
    /// <param name="theme">Theme name: "light" or "dark"</param>
    public async Task SetThemeAsync(string theme)
    {
        if (!string.Equals(theme, "light", StringComparison.Ordinal) &&
            !string.Equals(theme, "dark", StringComparison.Ordinal))
        {
            throw new ArgumentException("Theme must be 'light' or 'dark'", nameof(theme));
        }

        _currentTheme = theme;

        // Save to localStorage
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme);

        // Apply theme
        await ApplyThemeAsync(theme);

        // Notify subscribers
        OnThemeChanged?.Invoke();
    }

    /// <summary>
    /// Applies the theme to the document
    /// </summary>
    private async Task ApplyThemeAsync(string theme)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('data-theme', '{theme}')");

            // Update meta theme-color for mobile browsers
            var themeColor = string.Equals(theme, "dark", StringComparison.Ordinal) ? "#1a1a1a" : "#512BD4";
            await _jsRuntime.InvokeVoidAsync("eval",
         $"document.querySelector('meta[name=\"theme-color\"]').setAttribute('content', '{themeColor}')");

            // Fix inline styles for dark mode
            if (string.Equals(theme, "dark", StringComparison.Ordinal))
            {
                await FixInlineStylesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying theme: {ex.Message}");
        }
    }

    /// <summary>
    /// Fixes inline styles that prevent dark mode from working
    /// </summary>
    private async Task FixInlineStylesAsync()
    {
        try
        {
            var script = @"
           // Fix all elements with white/light inline backgrounds
       const elements = document.querySelectorAll('[style*=""background""]');
        elements.forEach(el => {
     const style = el.getAttribute('style');
            if (style && (
    style.includes('background: white') ||
          style.includes('background: #fff') ||
        style.includes('background: #ffffff') ||
          style.includes('background-color: white') ||
               style.includes('background-color: #fff') ||
                style.includes('background-color: #ffffff') ||
 style.includes('background-color: rgb(255') ||
      style.includes('background-color: #f') ||
     style.includes('background-color: #e')
         )) {
 el.style.backgroundColor = '#2d2d2d';
        }
                });
          ";

            await _jsRuntime.InvokeVoidAsync("eval", script);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fixing inline styles: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks if the current theme is dark
    /// </summary>
    public bool IsDarkMode => string.Equals(_currentTheme, "dark", StringComparison.Ordinal);

    /// <summary>
    /// Gets the theme icon name for display
    /// </summary>
    public string ThemeIcon => string.Equals(_currentTheme, "dark", StringComparison.Ordinal) ? "☀️" : "🌙";

    /// <summary>
    /// Gets the theme toggle tooltip text
    /// </summary>
    public string ThemeTooltip => string.Equals(_currentTheme, "dark", StringComparison.Ordinal) ? "Switch to Light Mode" : "Switch to Dark Mode";
}
