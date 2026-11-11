using AFS.Core.Interfaces;
using AFS.Core.Models;
using Microsoft.AspNetCore.Components;

namespace AFS.ComponentLibrary.Components.Common;

/// <summary>
/// Base class for components that use AI analysis functionality.
/// Provides common state management and methods (DRY principle).
/// </summary>
public abstract class AIAnalysisComponentBase : ComponentBase, IDisposable
{
    [Inject]
    protected IAIFinancialAdvisor AIAdvisor { get; set; } = default!;

    [Inject]
    protected AfsModel MainModel { get; set; } = default!;

    private CancellationTokenSource? _analysisCancellation;
    private string _aiUnavailableMessage = "AI analysis is unavailable.";

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
            var (isAvailable, message) = await AIAdvisor.CheckAvailabilityAsync();
            IsAIAvailable = isAvailable;
            _aiUnavailableMessage = message;
        }
        catch (Exception)
        {
            IsAIAvailable = false;
            _aiUnavailableMessage = "Unable to check AI availability.";
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
            ErrorMessage = _aiUnavailableMessage;
            return;
        }

        IsAnalyzing = true;
        ErrorMessage = string.Empty;
        AIAnalysis = string.Empty;
        _analysisCancellation?.Dispose();
        _analysisCancellation = new CancellationTokenSource();
        StateHasChanged();

        try
        {
            await AIAdvisor.GetStreamingResponseAsync(prompt, chunk =>
            {
                AIAnalysis += chunk;
                _ = InvokeAsync(StateHasChanged);
            }, _analysisCancellation.Token);
        }
        catch (Exception ex)
        {
            HandleAnalysisException(ex);
        }
        finally
        {
            IsAnalyzing = false;
            _analysisCancellation?.Dispose();
            _analysisCancellation = null;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Stops the current AI analysis.
    /// </summary>
    protected Task StopAnalysisAsync()
    {
        _analysisCancellation?.Cancel();
        return Task.CompletedTask;
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
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _analysisCancellation?.Cancel();
            _analysisCancellation?.Dispose();
            _analysisCancellation = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void HandleAnalysisException(Exception ex)
    {
        if (ex is OperationCanceledException || ex.Message.Contains("cancelled", StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("abort", StringComparison.OrdinalIgnoreCase))
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
