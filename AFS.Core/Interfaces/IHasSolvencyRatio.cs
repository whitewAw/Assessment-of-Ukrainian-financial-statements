namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for solvency ratio data with begin/end values for both years
/// </summary>
public interface IHasSolvencyRatio
{
    double BaseBegin { get; }
    double BaseEnd { get; }
    double CurrentBegin { get; }
    double CurrentEnd { get; }
}
