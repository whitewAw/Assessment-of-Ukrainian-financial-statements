using Microsoft.JSInterop;

namespace AFS.Core.Services;

/// <summary>
/// Service for managing application theme (light/dark mode).
/// Sealed because it's a DI service not designed for inheritance.
/// </summary>
public sealed class AppThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = "light";

    public event EventHandler? OnThemeChanged;

    public AppThemeService(IJSRuntime jsRuntime)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
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
            var savedTheme = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "theme").ConfigureAwait(false);

            if (!string.IsNullOrEmpty(savedTheme) &&
                (string.Equals(savedTheme, "light", StringComparison.Ordinal) ||
                 string.Equals(savedTheme, "dark", StringComparison.Ordinal)))
            {
                _currentTheme = savedTheme;
            }
            else
            {
                // Check system preference
                var prefersDark = await _jsRuntime.InvokeAsync<bool>("ufinInterop.prefersDarkScheme").ConfigureAwait(false);
                _currentTheme = prefersDark ? "dark" : "light";
            }

            await ApplyThemeAsync(_currentTheme).ConfigureAwait(false);
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
        await SetThemeAsync(newTheme).ConfigureAwait(false);
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
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme).ConfigureAwait(false);

        // Apply theme
        await ApplyThemeAsync(theme).ConfigureAwait(false);

        // Notify subscribers
        OnThemeChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Applies the theme to the document
    /// </summary>
    private async Task ApplyThemeAsync(string theme)
    {
        try
        {
            // Also updates meta theme-color for mobile browsers
            var themeColor = string.Equals(theme, "dark", StringComparison.Ordinal) ? "#1a1a1a" : "#512BD4";
            await _jsRuntime.InvokeVoidAsync("ufinInterop.applyTheme", theme, themeColor).ConfigureAwait(false);

            // Fix inline styles for dark mode
            if (string.Equals(theme, "dark", StringComparison.Ordinal))
            {
                await FixInlineStylesAsync().ConfigureAwait(false);
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
            await _jsRuntime.InvokeVoidAsync("ufinInterop.fixInlineDarkStyles").ConfigureAwait(false);
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
