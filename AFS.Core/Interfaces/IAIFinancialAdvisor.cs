namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for AI-powered financial advisory services
/// </summary>
public interface IAIFinancialAdvisor
{
    /// <summary>
    /// Check if the AI service is available and ready to use
    /// </summary>
    Task<(bool IsAvailable, string Message)> CheckAvailabilityAsync();

    /// <summary>
    /// Send a prompt to the AI and get a response
    /// </summary>
    /// <param name="prompt">The user's question or prompt</param>
    /// <returns>AI-generated response</returns>
    Task<string> GetResponseAsync(string prompt);

    /// <summary>
    /// Send a prompt with streaming response
    /// </summary>
    /// <param name="prompt">The user's question or prompt</param>
    /// <param name="onChunkReceived">Callback for each chunk of the response</param>
    /// <returns>Complete response</returns>
    Task<string> GetStreamingResponseAsync(string prompt, Action<string> onChunkReceived);

/// <summary>
    /// Generate financial insights based on company data
    /// </summary>
    /// <param name="financialData">JSON string of financial data</param>
    /// <returns>AI-generated insights</returns>
    Task<string> GetFinancialInsightsAsync(string financialData);

    /// <summary>
    /// Get recommendations based on financial ratios
    /// </summary>
    /// <param name="ratios">Dictionary of financial ratios</param>
    /// <returns>AI-generated recommendations</returns>
  Task<string> GetRecommendationsAsync(Dictionary<string, double> ratios);

    /// <summary>
    /// Explain a specific financial ratio or metric
    /// </summary>
    /// <param name="ratioName">Name of the ratio</param>
    /// <param name="value">Value of the ratio</param>
    /// <param name="context">Additional context (optional)</param>
    /// <returns>AI-generated explanation</returns>
    Task<string> ExplainRatioAsync(string ratioName, double value, string? context = null);
}
