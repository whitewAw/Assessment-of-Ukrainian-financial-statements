namespace AFS.Core.Json;

public class AssetCompositionData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public IReadOnlyList<ChartDataItemDto>? BeginningOfYear { get; init; }
    public IReadOnlyList<ChartDataItemDto>? EndOfYear { get; init; }
}
