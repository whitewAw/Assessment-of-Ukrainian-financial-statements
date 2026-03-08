namespace AFS.Core.Json;

public class StabilityClassificationData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public StabilityTypeData? AbsoluteStability { get; set; }
    public StabilityTypeData? NormalStability { get; set; }
    public StabilityTypeData? PreCrisisStability { get; set; }
    public StabilityTypeData? CrisisStability { get; set; }
}
