namespace AFS.Core.Json;

public class CapitalSourcesData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public int PreviousYear { get; set; }
    public IReadOnlyList<ChartDataItemDto>? CapitalSources { get; init; }
}
