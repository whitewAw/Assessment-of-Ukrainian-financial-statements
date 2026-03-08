namespace AFS.Core.Json;

public class SolvencyRatiosData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public SolvencyRatioItem? OverallLiquidityRatio { get; set; }
    public SolvencyRatioItem? AbsoluteLiquidityRatio { get; set; }
    public SolvencyRatioItem? IntermediateCoverageRatio { get; set; }
    public SolvencyRatioItem? CurrentLiquidityRatio { get; set; }
    public SolvencyRatioSimpleItem? RecoverySolvencyRatio { get; set; }
    public SolvencyRatioSimpleItem? LossSolvencyRatio { get; set; }
}
