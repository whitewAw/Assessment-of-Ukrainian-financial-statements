namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for capital component with equity percentage.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface ICapitalComponentEquity<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
    T InPercentageOfEquityBase { get; }
    T InPercentageOfEquityCurrent { get; }
}
