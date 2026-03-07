namespace AFS.Core.Json;

/// <summary>
/// DTOs for chart data serialization - optimized for AI prompts.
/// Using IReadOnlyList with init setters for AOT compatibility.
/// </summary>
public class ChartDataItemDto
{
    public string? Item { get; set; }
    public double Value { get; set; }
}

public class AssetCompositionData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public IReadOnlyList<ChartDataItemDto>? BeginningOfYear { get; init; }
    public IReadOnlyList<ChartDataItemDto>? EndOfYear { get; init; }
}

public class CapitalSourcesData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public int PreviousYear { get; set; }
    public IReadOnlyList<ChartDataItemDto>? CapitalSources { get; init; }
}

public class PayableStructureData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public int PreviousYear { get; set; }
    public IReadOnlyList<ChartDataItemDto>? PayableStructure { get; init; }
}

public class TurnoverTimeData
{
    public string? CompanyName { get; set; }
    public IReadOnlyList<TurnoverDataPoint>? Money { get; init; }
    public IReadOnlyList<TurnoverDataPoint>? Receivables { get; init; }
    public IReadOnlyList<TurnoverDataPoint>? MaterialValues { get; init; }
}

public class TurnoverDataPoint
{
    public string? Date { get; set; }
    public double Value { get; set; }
}
