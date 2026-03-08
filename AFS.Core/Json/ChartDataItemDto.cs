namespace AFS.Core.Json;

/// <summary>
/// DTO for chart data item - optimized for AI prompts.
/// </summary>
public class ChartDataItemDto
{
    public string? Item { get; set; }
    public double Value { get; set; }
}
