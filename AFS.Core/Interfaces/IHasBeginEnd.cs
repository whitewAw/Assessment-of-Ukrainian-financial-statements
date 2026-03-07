namespace AFS.Core.Interfaces;

/// <summary>
/// Common interfaces for financial data types to enable AOT-compatible serialization without reflection.
/// </summary>

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

/// <summary>
/// Interface for types with BaseYear and CurrentYear values (e.g., BusinessActivityCalculationRow)
/// </summary>
public interface IHasBaseCurrentYear
{
    double BaseYear { get; }
    double CurrentYear { get; }
    double Deviations { get; }
}

/// <summary>
/// Interface for types with Base and Current period rows.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface IHasBaseCurrent<out T> where T : IHasBeginEnd
{
    T Base { get; }
    T Current { get; }
}

/// <summary>
/// Interface for capital source metrics with percentage calculations.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface ICapitalSourceMetric<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
    T InPercentageOfAssetsBase { get; }
    T InPercentageOfAssetsCurrent { get; }
}

/// <summary>
/// Interface for capital component with equity percentage.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface ICapitalComponentEquity<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
    T InPercentageOfEquityBase { get; }
    T InPercentageOfEquityCurrent { get; }
}

/// <summary>
/// Interface for capital component with borrowed capital percentage.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface ICapitalComponentBorrowed<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
    T InPercentageOfBorrowedCapitalBase { get; }
    T InPercentageOfBorrowedCapitalCurrent { get; }
}

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

/// <summary>
/// Interface for receivable/payable assessment data
/// </summary>
public interface IHasReceivablePayable
{
    double ReceivableBase { get; }
    double ReceivableCurrent { get; }
    double PayableBase { get; }
    double PayableCurrent { get; }
    double ExceedingReceivableBase { get; }
    double ExceedingReceivableCurrent { get; }
    double ExceedingPayableBase { get; }
    double ExceedingPayableCurrent { get; }
}

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

/// <summary>
/// Interface for simple solvency ratio (end values only)
/// </summary>
public interface IHasSolvencyRatioSimple
{
    double BaseEnd { get; }
    double CurrentEnd { get; }
}

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

/// <summary>
/// Interface for general financial stability source data.
/// Covariant (out) because T is only returned, never accepted as input.
/// </summary>
public interface IHasStabilitySource<out T> : IHasBaseCurrent<T> where T : IHasBeginEnd
{
}
