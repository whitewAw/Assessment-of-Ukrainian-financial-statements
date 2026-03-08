namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for capital source metrics with percentage calculations.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface ICapitalSourceMetric<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
    T InPercentageOfAssetsBase { get; }
    T InPercentageOfAssetsCurrent { get; }
}
