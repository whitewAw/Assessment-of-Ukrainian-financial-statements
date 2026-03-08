namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for general financial stability source data.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface IHasStabilitySource<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
}
