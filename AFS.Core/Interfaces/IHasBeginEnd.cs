namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for types with Beginning and End of year values (e.g., CharacteristicsOfCapitalCalculationRow)
/// </summary>
public interface IHasBeginEnd
{
    double BeginningOfyear { get; }
    double EndOfYear { get; }
    double Deviations { get; }
    double GrowthRate { get; }
}
