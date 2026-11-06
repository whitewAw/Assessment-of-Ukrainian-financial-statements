namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for types with BaseYear and CurrentYear values (e.g., BusinessActivityCalculationRow)
/// </summary>
public interface IHasBaseCurrentYear
{
    double BaseYear { get; }
    double CurrentYear { get; }
    double Deviations { get; }
}
