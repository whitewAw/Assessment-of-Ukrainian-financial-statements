namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for capital component with borrowed capital percentage.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface ICapitalComponentBorrowed<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
    T InPercentageOfBorrowedCapitalBase { get; }
    T InPercentageOfBorrowedCapitalCurrent { get; }
}
