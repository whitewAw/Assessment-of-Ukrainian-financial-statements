using AFS.Core.Interfaces;
using Microsoft.JSInterop;
using System.Net;
using System.Text.RegularExpressions;

namespace AFS.ComponentLibrary.Helpers;

/// <summary>
/// Helper class for AI analysis functionality.
/// Provides reusable methods to eliminate code duplication (DRY principle).
/// </summary>
public static partial class AIAnalysisHelper
{
    // AI output is untrusted, and these responses are rendered as MarkupString.
    private const int MaxFormattedLength = 50_000;

    [GeneratedRegex(@"\*\*(?<text>.+?)\*\*", RegexOptions.Singleline | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex BoldRegex();

    [GeneratedRegex(@"(?m)^- ", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex BulletRegex();
    /// <summary>
    /// Checks if AI is available.
    /// </summary>
    public static async Task<bool> CheckAvailabilityAsync(IAIFinancialAdvisor aiAdvisor)
    {
        try
        {
            var (isAvailable, _) = await aiAdvisor.CheckAvailabilityAsync().ConfigureAwait(false);
            return isAvailable;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Stops the current AI analysis by aborting the Chrome AI operation.
    /// </summary>
    public static async Task StopAnalysisAsync(IJSRuntime js)
    {
        try
        {
            var module = await js.InvokeAsync<IJSObjectReference>("import", "./js/chromeai.js").ConfigureAwait(false);
            await module.InvokeVoidAsync("abortCurrentOperation").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error stopping analysis: {ex.Message}").ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Formats AI response for HTML display.
    /// HTML-encodes the input, then converts a safe subset of markdown (bold, bullets, line breaks).
    /// </summary>
    public static string FormatResponse(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return string.Empty;

        var text = response.Length > MaxFormattedLength ? response[..MaxFormattedLength] : response;
        var encoded = WebUtility.HtmlEncode(text.Replace("\r\n", "\n", StringComparison.Ordinal));
        encoded = BoldRegex().Replace(encoded, "<strong>${text}</strong>");
        encoded = BulletRegex().Replace(encoded, "• ");
        return encoded.Replace("\n", "<br/>", StringComparison.Ordinal);
    }

    /// <summary>
    /// Handles exceptions during AI analysis and returns appropriate error message.
    /// </summary>
    public static (string? appendToAnalysis, string? errorMessage) HandleAnalysisException(Exception ex, string currentAnalysis)
    {
        if (ex.Message.Contains("cancelled", StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("abort", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(currentAnalysis))
            {
                return ("\n\n[Analysis stopped by user]", null);
            }
            return (null, "Analysis was stopped by user.");
        }
        return (null, $"Failed to analyze data: {ex.Message}");
    }
}
