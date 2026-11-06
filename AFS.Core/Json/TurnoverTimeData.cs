namespace AFS.Core.Json;

public class TurnoverTimeData
{
    public string? CompanyName { get; set; }
    public IReadOnlyList<TurnoverDataPoint>? Money { get; init; }
    public IReadOnlyList<TurnoverDataPoint>? Receivables { get; init; }
    public IReadOnlyList<TurnoverDataPoint>? MaterialValues { get; init; }
}
