namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for simple solvency ratio (end values only)
/// </summary>
public interface IHasSolvencyRatioSimple
{
    double BaseEnd { get; }
    double CurrentEnd { get; }
}
