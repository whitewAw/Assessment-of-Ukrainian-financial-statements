namespace AFS.Core.Json;

public class PayableStructureData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public int PreviousYear { get; set; }
    public IReadOnlyList<ChartDataItemDto>? PayableStructure { get; init; }
}
