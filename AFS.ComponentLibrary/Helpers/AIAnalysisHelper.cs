using AFS.Core.Interfaces;
using Microsoft.JSInterop;

namespace AFS.ComponentLibrary.Helpers;

/// <summary>
/// Helper class for AI analysis functionality.
/// Provides reusable methods to eliminate code duplication (DRY principle).
/// </summary>
public static class AIAnalysisHelper
{
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
    /// Converts markdown-style formatting to HTML.
    /// </summary>
    public static string FormatResponse(string? response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return string.Empty;

        return response
            .Replace("\n\n", "<br/><br/>")
            .Replace("\n", "<br/>")
            .Replace("**", "<strong>")
            .Replace("**", "</strong>")
            .Replace("- ", "• ");
    }

    /// <summary>
    /// Handles exceptions during AI analysis and returns appropriate error message.
    /// </summary>
    public static (string? appendToAnalysis, string? errorMessage) HandleAnalysisException(Exception ex, string currentAnalysis)
    {
        if (ex.Message.Contains("cancelled") || ex.Message.Contains("abort", StringComparison.OrdinalIgnoreCase))
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
