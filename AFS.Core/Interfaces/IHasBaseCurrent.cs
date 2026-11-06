namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for types with Base and Current period rows.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface IHasBaseCurrent<out T> where T : IHasBeginEnd
{
    T Base { get; }
    T Current { get; }
}
