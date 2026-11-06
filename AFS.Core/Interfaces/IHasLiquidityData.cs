namespace AFS.Core.Interfaces;

/// <summary>
/// Interface for liquidity balance assessment data
/// </summary>
public interface IHasLiquidityData
{
    double ABegin { get; }
    double AEnd { get; }
    double PBegin { get; }
    double PEnd { get; }
    double PaymentBalanceBegin { get; }
    double PaymentBalanceEnd { get; }
}
