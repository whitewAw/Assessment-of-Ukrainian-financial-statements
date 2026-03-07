using AFS.Core.Exceptions;
using AFS.Core.Interfaces;
using AFS.Core.Json;
using System.Net.Http.Json;

namespace AFS.Core.Services;

/// <summary>
/// OpenAI-based Financial Advisor (fallback option).
/// Sealed because it's a DI service not designed for inheritance.
/// </summary>
public sealed class OpenAIFinancialAdvisor : IAIFinancialAdvisor
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public OpenAIFinancialAdvisor(HttpClient httpClient, string apiKey, string model = "gpt-4o-mini")
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _model = model;

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<(bool IsAvailable, string Message)> CheckAvailabilityAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_apiKey) || string.Equals(_apiKey, "your-api-key-here", StringComparison.Ordinal))
            {
                return (false, "OpenAI API key not configured. Please set it in appsettings.json");
            }

            // Simple validation that API key format is correct
            if (!_apiKey.StartsWith("sk-", StringComparison.Ordinal))
            {
                return (false, "Invalid OpenAI API key format");
            }

            return (true, "OpenAI is ready to use");
        }
        catch (Exception ex)
        {
            return (false, $"Error checking OpenAI availability: {ex.Message}");
        }
    }

    public async Task<string> GetResponseAsync(string prompt)
    {
        try
        {
            var requestBody = new OpenAIRequest
            {
                Model = _model,
                Messages = new[]
                {
                    new OpenAIMessage
                    {
                        Role = "system",
                        Content = "You are a helpful financial advisor with expertise in analyzing financial statements and ratios."
                    },
                    new OpenAIMessage
                    {
                        Role = "user",
                        Content = prompt
                    }
                },
                Temperature = 0.7,
                MaxTokens = 1000
            };

            var content = JsonContent.Create(requestBody, AFSJsonSerializerContext.Default.OpenAIRequest);
            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                content);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync(AFSJsonSerializerContext.Default.OpenAIResponse);

            return result?.Choices?[0]?.Message?.Content ?? string.Empty;
        }
        catch (AIServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new AIServiceException($"Failed to get OpenAI response: {ex.Message}", ex);
        }
    }

    public async Task GetStreamingResponseAsync(string prompt, Action<string> onChunkReceived)
    {
        // For simplicity, we'll use non-streaming for OpenAI
        // Implementing true streaming would require SSE (Server-Sent Events) handling
        var response = await GetResponseAsync(prompt);
        onChunkReceived?.Invoke(response);
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
}
