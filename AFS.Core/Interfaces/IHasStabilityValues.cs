namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for stability classification values
/// </summary>
public interface IHasStabilityValues
{
    string CurrentBVal { get; }
    string ShortBVal { get; }
    string LongBVal { get; }
    string CurrentCVal { get; }
    string ShortCVal { get; }
    string LongCVal { get; }
}
