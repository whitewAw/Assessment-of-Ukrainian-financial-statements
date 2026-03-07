using AFS.Core.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace AFS.Core.Services;

/// <summary>
/// Browser-based AI Financial Advisor using Chrome's built-in Gemini Nano model
/// </summary>
public class BrowserAIFinancialAdvisor : IAIFinancialAdvisor, IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private DotNetObjectReference<BrowserAIFinancialAdvisor>? _dotNetReference;

    private Action<string>? _currentStreamCallback;
    private Action<int>? _downloadProgressCallback;

    public BrowserAIFinancialAdvisor(IJSRuntime jsRuntime)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/chromeai.js").AsTask());
    }

    public async Task<(bool IsAvailable, string Message)> CheckAvailabilityAsync()
    {
        try
        {
            var module = await _moduleTask.Value;
            var result = await module.InvokeAsync<JsonElement>("checkAvailability");

            var available = result.GetProperty("available").GetBoolean();
            var reason = result.GetProperty("reason").GetString() ?? "Unknown";

            return (available, reason);
        }
        catch (Exception ex)
        {
            return (false, $"Error checking availability: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a session with download progress monitoring
    /// </summary>
    public async Task<bool> CreateSessionWithProgressAsync(Action<int>? onProgressUpdate = null)
    {
        try
        {
            _downloadProgressCallback = onProgressUpdate;
            _dotNetReference = DotNetObjectReference.Create(this);

            var module = await _moduleTask.Value;
            var result = await module.InvokeAsync<JsonElement>(
                "createSessionWithProgress",
                _dotNetReference,
                nameof(OnDownloadProgress));

            var success = result.GetProperty("success").GetBoolean();
            return success;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            _downloadProgressCallback = null;
        }
    }

    [JSInvokable]
    public void OnDownloadProgress(int progress)
    {
        _downloadProgressCallback?.Invoke(progress);
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        try
        {
            var module = await _moduleTask.Value;
            var result = await module.InvokeAsync<JsonElement>("prompt", prompt);

            var success = result.GetProperty("success").GetBoolean();
            if (!success)
            {
                var error = result.TryGetProperty("error", out var errorProp)
        ? errorProp.GetString()
                : "Unknown error";
                throw new Exception($"AI Error: {error}");
            }

            return result.GetProperty("response").GetString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get AI response: {ex.Message}", ex);
        }
    }

    public async Task GetStreamingResponseAsync(string prompt, Action<string> onChunkReceived)
    {
        try
        {
            _currentStreamCallback = onChunkReceived;
            _dotNetReference = DotNetObjectReference.Create(this);

            var module = await _moduleTask.Value;
            var result = await module.InvokeAsync<JsonElement>(
                "promptStreaming",
                prompt,
                _dotNetReference,
                nameof(ReceiveStreamChunk));

            var success = result.GetProperty("success").GetBoolean();
            if (!success)
            {
                var error = result.TryGetProperty("error", out var errorProp) ? errorProp.GetString() : "Unknown error";
                throw new Exception($"AI Streaming Error: {error}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get streaming AI response: {ex.Message}", ex);
        }
        finally
        {
            _dotNetReference?.Dispose();
            _dotNetReference = null;
            _currentStreamCallback = null;
        }
    }

    [JSInvokable]
    public void ReceiveStreamChunk(string chunk)
    {
        _currentStreamCallback?.Invoke(chunk);
    }

    public async Task<string> GetFinancialInsightsAsync(string financialData)
    {
        var prompt = $@"You are a financial analyst. Analyze the following financial data and provide key insights:

Financial Data:
{financialData}

Provide:
1. Overall financial health assessment
2. Key strengths
3. Areas of concern
4. Notable trends

Keep the response concise and actionable.";

        return await GetResponseAsync(prompt);
    }

    public async Task<string> GetRecommendationsAsync(IDictionary<string, double> ratios)
    {
        var ratiosText = string.Join("\n", ratios.Select(r => $"- {r.Key}: {r.Value:N2}"));

        var prompt = $@"You are a financial advisor. Based on these financial ratios, provide actionable recommendations:

{ratiosText}

Provide:
1. Top 3 priority actions
2. Risk mitigation strategies
3. Growth opportunities

Be specific and practical.";

        return await GetResponseAsync(prompt);
    }

    public async Task<string> ExplainRatioAsync(string ratioName, double value, string? context = null)
    {
        var contextText = !string.IsNullOrEmpty(context)
? $"\nAdditional Context: {context}"
       : string.Empty;

        var prompt = $@"Explain the financial ratio '{ratioName}' with a value of {value:N2} in simple terms.

Include:
1. What this ratio measures
2. What this value indicates (good/bad/neutral)
3. Industry benchmark (if applicable)
4. What actions to consider
{contextText}

Keep it brief and easy to understand.";

        return await GetResponseAsync(prompt);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.InvokeVoidAsync("destroySession");
            await module.DisposeAsync();
        }

        _dotNetReference?.Dispose();
    }
}
