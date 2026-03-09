using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;

namespace AFS.ComponentLibrary.Resources;

/// <summary>
/// AOT-safe resource helper that ensures ResourceManager is preserved during trimming.
/// Uses direct ResourceManager access instead of IStringLocalizer.
/// </summary>
public static class ResourceHelper
{
    // Reference to ensure ResourceManager types are preserved
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ResourceManager))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Resource))]
    public static string GetString(string? key)
    {
        if (string.IsNullOrEmpty(key))
            return string.Empty;

        try
        {
            return Resource.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        }
        catch
        {
            return key;
        }
    }

    /// <summary>
    /// Gets a localized string with a fallback to the key if not found.
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ResourceManager))]
    public static string GetStringOrKey(string? key)
    {
        if (string.IsNullOrEmpty(key))
            return "Unknown";

        try
        {
            var result = Resource.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
            return string.IsNullOrEmpty(result) ? key : result;
        }
        catch
        {
            return key;
        }
    }
}
