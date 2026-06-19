using AFS.Core.Interfaces;
using AFS.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AFS.ComponentLibrary.Components.Common;

/// <summary>
/// Base class for components that use AI analysis functionality.
/// Provides common state management and methods (DRY principle).
/// </summary>
public abstract class AIAnalysisComponentBase : ComponentBase
{
    [Inject]
    protected IAIFinancialAdvisor AIAdvisor { get; set; } = default!;

    [Inject]
    protected IJSRuntime JS { get; set; } = default!;

    [Inject]
    protected AfsModel MainModel { get; set; } = default!;

    protected string AIAnalysis { get; set; } = string.Empty;
    protected string ErrorMessage { get; set; } = string.Empty;
    protected bool IsAnalyzing { get; set; }
    protected bool IsCheckingAvailability { get; set; } = true;
    protected bool IsAIAvailable { get; set; }

    /// <summary>
    /// Checks if AI is available. Call in OnInitializedAsync.
    /// </summary>
    protected async Task CheckAIAvailabilityAsync()
    {
        IsCheckingAvailability = true;
        StateHasChanged();

        try
        {
            var (isAvailable, _) = await AIAdvisor.CheckAvailabilityAsync().ConfigureAwait(false);
            IsAIAvailable = isAvailable;
        }
        catch (Exception)
        {
            IsAIAvailable = false;
        }
        finally
        {
            IsCheckingAvailability = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Performs AI analysis with the given prompt.
    /// </summary>
    protected async Task AnalyzeAsync(string prompt)
    {
        if (!IsAIAvailable)
        {
            ErrorMessage = "Chrome AI is not available. Please check the setup instructions.";
            return;
        }

        IsAnalyzing = true;
        ErrorMessage = string.Empty;
        AIAnalysis = string.Empty;
        StateHasChanged();

        try
        {
            await AIAdvisor.GetStreamingResponseAsync(prompt, chunk =>
            {
                AIAnalysis += chunk;
                _ = InvokeAsync(StateHasChanged);
            }).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            HandleAnalysisException(ex);
        }
        finally
        {
            IsAnalyzing = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Stops the current AI analysis.
    /// </summary>
    protected async Task StopAnalysisAsync()
    {
        try
        {
            var module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/chromeai.js").ConfigureAwait(false);
            await module.InvokeVoidAsync("abortCurrentOperation").ConfigureAwait(false);

            IsAnalyzing = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Error stopping analysis: {ex.Message}").ConfigureAwait(false);
            IsAnalyzing = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Clears the AI analysis results.
    /// </summary>
    protected void ClearAnalysis()
    {
        AIAnalysis = string.Empty;
        ErrorMessage = string.Empty;
    }

    /// <summary>
    /// Override to build the component-specific prompt.
    /// </summary>
    protected abstract string BuildPrompt();

    /// <summary>
    /// Handles exceptions during analysis.
    /// </summary>
    private void HandleAnalysisException(Exception ex)
    {
        if (ex.Message.Contains("cancelled", StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("abort", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(AIAnalysis))
            {
                AIAnalysis += "\n\n[Analysis stopped by user]";
            }
            else
            {
                ErrorMessage = "Analysis was stopped by user.";
            }
        }
        else
        {
            ErrorMessage = $"Failed to analyze data: {ex.Message}";
        }
    }
}
