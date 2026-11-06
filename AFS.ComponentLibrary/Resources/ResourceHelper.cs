using System.Globalization;

namespace AFS.ComponentLibrary.Resources;

/// <summary>
/// AOT-safe resource helper that uses the generated Resource class directly.
/// Falls back to the key if resource not found.
/// </summary>
public static class ResourceHelper
{
    /// <summary>
    /// Gets a localized string. Uses Resource.ResourceManager with current UI culture.
    /// If trimming breaks ResourceManager, returns the key itself as fallback.
    /// </summary>
    public static string GetString(string? key)
    {
        if (string.IsNullOrEmpty(key))
            return string.Empty;

        // Try to get the localized string
        try
        {
            // Set the culture for the Resource class
            Resource.Culture = CultureInfo.CurrentUICulture;
            var result = Resource.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
            if (!string.IsNullOrEmpty(result))
                return result;
        }
        catch
        {
            // ResourceManager failed, fall through to fallback
        }

        // Fallback: return the key itself (better than "series-1")
        return key;
    }

    /// <summary>
    /// Gets a localized string with key as fallback.
    /// </summary>
    public static string GetStringOrKey(string? key)
    {
        if (string.IsNullOrEmpty(key))
            return "Unknown";

        try
        {
            Resource.Culture = CultureInfo.CurrentUICulture;
            var result = Resource.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
            if (!string.IsNullOrEmpty(result))
                return result;
        }
        catch
        {
            // Fall through to return key
        }

        return key;
    }
}
