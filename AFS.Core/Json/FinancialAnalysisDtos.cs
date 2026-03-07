using System.Text.Json.Serialization;

namespace AFS.Core.Json;

/// <summary>
/// DTOs for financial analysis data serialization to AI prompts.
/// </summary>

// AI Chat Financial Context DTOs
public class FinancialContextData
{
    public CompanyInfoData? Company { get; set; }
    public BalanceSheetData? BalanceSheet { get; set; }
    public IncomeStatementData? IncomeStatement { get; set; }
}

public class CompanyInfoData
{
    public string? Name { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
}

public class BalanceSheetData
{
    public YearBalanceData? BaseYear { get; set; }
    public YearBalanceData? CurrentYear { get; set; }
}

public class YearBalanceData
{
    public BalanceItemData? TotalAssets { get; set; }
    public BalanceItemData? NonCurrentAssets { get; set; }
    public BalanceItemData? CurrentAssets { get; set; }
    public BalanceItemData? Equity { get; set; }
    public BalanceItemData? TotalLiabilities { get; set; }
    public BalanceItemData? CurrentLiabilities { get; set; }
}

public class BalanceItemData
{
    public double Beginning { get; set; }
    public double End { get; set; }
}

public class IncomeStatementData
{
    public YearIncomeData? BaseYear { get; set; }
    public YearIncomeData? CurrentYear { get; set; }
}

public class YearIncomeData
{
    public double Revenue { get; set; }
    public double GrossProfit { get; set; }
    public double OperatingProfit { get; set; }
    public double NetProfit { get; set; }
}

// Financial Stability Classification DTOs
public class StabilityClassificationData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public StabilityTypeData? AbsoluteStability { get; set; }
    public StabilityTypeData? NormalStability { get; set; }
    public StabilityTypeData? PreCrisisStability { get; set; }
    public StabilityTypeData? CrisisStability { get; set; }
}

public class StabilityTypeData
{
    public string? Type { get; set; }

    [JsonPropertyName("Base_Current")]
    public bool BaseCurrent { get; set; }

    [JsonPropertyName("Base_ShortTerm")]
    public bool BaseShortTerm { get; set; }

    [JsonPropertyName("Base_LongTerm")]
    public bool BaseLongTerm { get; set; }

    [JsonPropertyName("Current_Current")]
    public bool CurrentCurrent { get; set; }

    [JsonPropertyName("Current_ShortTerm")]
    public bool CurrentShortTerm { get; set; }

    [JsonPropertyName("Current_LongTerm")]
    public bool CurrentLongTerm { get; set; }
}

// Receivable and Payable Assessment DTOs
public class ReceivablePayableData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public ReceivablePayableSummary? Summary { get; set; }
    public ReceivablePayableCategoryData? BuyersSuppliers { get; set; }
    public ReceivablePayableCategoryData? BudgetFunds { get; set; }
    public ReceivablePayableCategoryData? Advances { get; set; }
    public ReceivablePayableCategoryData? Others { get; set; }
}

public class ReceivablePayableSummary
{
    [JsonPropertyName("TotalReceivables_Base")]
    public double TotalReceivablesBase { get; set; }

    [JsonPropertyName("TotalReceivables_Current")]
    public double TotalReceivablesCurrent { get; set; }

    [JsonPropertyName("TotalPayables_Base")]
    public double TotalPayablesBase { get; set; }

    [JsonPropertyName("TotalPayables_Current")]
    public double TotalPayablesCurrent { get; set; }

    [JsonPropertyName("NetPosition_Base")]
    public double NetPositionBase { get; set; }

    [JsonPropertyName("NetPosition_Current")]
    public double NetPositionCurrent { get; set; }

    [JsonPropertyName("ReceivableToPayableRatio_Base")]
    public double ReceivableToPayableRatioBase { get; set; }

    [JsonPropertyName("ReceivableToPayableRatio_Current")]
    public double ReceivableToPayableRatioCurrent { get; set; }
}

public class ReceivablePayableCategoryData
{
    [JsonPropertyName("Receivable_Base")]
    public double ReceivableBase { get; set; }

    [JsonPropertyName("Receivable_Current")]
    public double ReceivableCurrent { get; set; }

    [JsonPropertyName("Payable_Base")]
    public double PayableBase { get; set; }

    [JsonPropertyName("Payable_Current")]
    public double PayableCurrent { get; set; }

    [JsonPropertyName("ExcessReceivable_Base")]
    public double ExcessReceivableBase { get; set; }

    [JsonPropertyName("ExcessReceivable_Current")]
    public double ExcessReceivableCurrent { get; set; }

    [JsonPropertyName("ExcessPayable_Base")]
    public double ExcessPayableBase { get; set; }

    [JsonPropertyName("ExcessPayable_Current")]
    public double ExcessPayableCurrent { get; set; }
}

// Solvency Ratios DTOs
public class SolvencyRatiosData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public SolvencyRatioItem? OverallLiquidityRatio { get; set; }
    public SolvencyRatioItem? AbsoluteLiquidityRatio { get; set; }
    public SolvencyRatioItem? IntermediateCoverageRatio { get; set; }
    public SolvencyRatioItem? CurrentLiquidityRatio { get; set; }
    public SolvencyRatioSimpleItem? RecoverySolvencyRatio { get; set; }
    public SolvencyRatioSimpleItem? LossSolvencyRatio { get; set; }
}

public class SolvencyRatioItem
{
    [JsonPropertyName("Base_Begin")]
    public double BaseBegin { get; set; }

    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Current_Begin")]
    public double CurrentBegin { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    [JsonPropertyName("Deviation_Base")]
    public double DeviationBase { get; set; }

    [JsonPropertyName("Deviation_Current")]
    public double DeviationCurrent { get; set; }
}

public class SolvencyRatioSimpleItem
{
    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    public double Deviation { get; set; }
}

// Factor Analysis of Fixed Assets DTOs
public class FactorAnalysisData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public FactorMetricData? NetRevenueFromSales { get; set; }
    public FactorMetricData? AverageNumberOfEmployees { get; set; }
    public FactorMetricData? LaborProductivity { get; set; }
    public FactorMetricData? AverageCostOfFixedAssets { get; set; }
    public FactorMetricData? CapitalIntensity { get; set; }
    public FactorMetricData? FixedAssetTurnover { get; set; }
}

public class FactorMetricData
{
    public double BaseYear { get; set; }
    public double CurrentYear { get; set; }
    public double Deviations { get; set; }
    public double PercentageChange { get; set; }
}

// Business Activity Indicators DTOs
public class BusinessActivityData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public BusinessActivityMetricData? GrossProfitMargin { get; set; }
    public BusinessActivityMetricData? BusinessActivityRatio { get; set; }
    public BusinessActivityMetricData? FinancialResourceEfficiency { get; set; }
    public BusinessActivityMetricData? OwnFundsUtilization { get; set; }
    public BusinessActivityMetricData? EnterpriseProfitability { get; set; }
    public BusinessActivityMetricData? LaborProductivity { get; set; }
    public BusinessActivityMetricData? FixedAssetTurnover { get; set; }
    public TurnoverMetricData? ReceivablesTurnover { get; set; }
    public TurnoverMetricData? InventoryTurnover { get; set; }
    public BusinessActivityMetricData? OperatingCycle { get; set; }
    public TurnoverMetricData? CurrentAssetsTurnover { get; set; }
    public BusinessActivityMetricData? EquityTurnover { get; set; }
    public BusinessActivityMetricData? TotalCapitalTurnover { get; set; }
    public BusinessActivityMetricData? EconomicGrowthStability { get; set; }
    public BusinessActivityMetricData? EquityPaybackPeriod { get; set; }
}

public class BusinessActivityMetricData
{
    public double BaseYear { get; set; }
    public double CurrentYear { get; set; }
    public double Change { get; set; }
}

public class TurnoverMetricData
{
    [JsonPropertyName("Revolutions_Base")]
    public double RevolutionsBase { get; set; }

    [JsonPropertyName("Revolutions_Current")]
    public double RevolutionsCurrent { get; set; }

    [JsonPropertyName("Days_Base")]
    public double DaysBase { get; set; }

    [JsonPropertyName("Days_Current")]
    public double DaysCurrent { get; set; }
}

// Intangible Assets Efficiency DTOs
public class IntangibleAssetsData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public IntangibleAssetMetricData? NetRevenueFromSales { get; set; }
    public IntangibleAssetMetricData? AverageCostOfIntangibleAssets { get; set; }

    [JsonPropertyName("IntangibleAssetTurnover_UAH")]
    public IntangibleAssetMetricData? IntangibleAssetTurnoverUah { get; set; }

    [JsonPropertyName("CapitalIntensityOfProduction_UAH")]
    public IntangibleAssetMetricData? CapitalIntensityOfProductionUah { get; set; }
}

public class IntangibleAssetMetricData
{
    public double BaseYear { get; set; }
    public double CurrentYear { get; set; }
    public double Deviations { get; set; }
    public double PercentageChange { get; set; }
}

// Financial Stability Indicators DTOs
public class FinancialStabilityIndicatorsData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public StabilityIndicatorMetricData? TotalReturnOnAssets { get; set; }
    public StabilityIndicatorMetricData? IndependenceRatio { get; set; }
    public StabilityIndicatorMetricData? FinancialLeverageRatio { get; set; }
    public StabilityIndicatorMetricData? FinancialStabilityRatio { get; set; }
    public StabilityIndicatorMetricData? ManeuverabilityRatio { get; set; }
    public StabilityIndicatorMetricData? BorrowedCapitalConcentration { get; set; }
    public StabilityIndicatorMetricData? LongTermInvestmentStructure { get; set; }
    public StabilityIndicatorMetricData? LongTermBorrowingRatio { get; set; }
    public StabilityIndicatorMetricData? CapitalStructureRatio { get; set; }
    public StabilityIndicatorMetricData? DebtToEquityRatio { get; set; }
    public StabilityIndicatorMetricData? OwnFundsInInventories { get; set; }
    public StabilityIndicatorMetricData? MobileToImmobilizedRatio { get; set; }
    public StabilityIndicatorMetricData? TotalCoverageRatio { get; set; }
}

public class StabilityIndicatorMetricData
{
    public double BaseYear { get; set; }
    public double CurrentYear { get; set; }
    public double Change { get; set; }
}

// Liquidity Indicators of Balance DTOs
public class LiquidityIndicatorsData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }

    [JsonPropertyName("BaseYear_Assessment")]
    public LiquidityPeriodData? BaseYearAssessment { get; set; }

    [JsonPropertyName("CurrentYear_Assessment")]
    public LiquidityPeriodData? CurrentYearAssessment { get; set; }
}

public class LiquidityPeriodData
{
    public LiquidityConditionData? BeginOfYear { get; set; }
    public LiquidityConditionData? EndOfYear { get; set; }
}

public class LiquidityConditionData
{
    public bool IsLiquid { get; set; }

    [JsonPropertyName("A1_MostLiquid")]
    public double A1MostLiquid { get; set; }

    [JsonPropertyName("P1_MostUrgent")]
    public double P1MostUrgent { get; set; }

    [JsonPropertyName("Surplus_A1P1")]
    public double SurplusA1P1 { get; set; }

    [JsonPropertyName("A2_QuickLiquid")]
    public double A2QuickLiquid { get; set; }

    [JsonPropertyName("P2_ShortTerm")]
    public double P2ShortTerm { get; set; }

    [JsonPropertyName("Surplus_A2P2")]
    public double SurplusA2P2 { get; set; }

    [JsonPropertyName("A3_SlowLiquid")]
    public double A3SlowLiquid { get; set; }

    [JsonPropertyName("P3_LongTerm")]
    public double P3LongTerm { get; set; }

    [JsonPropertyName("Surplus_A3P3")]
    public double SurplusA3P3 { get; set; }

    [JsonPropertyName("A4_HardToSell")]
    public double A4HardToSell { get; set; }

    [JsonPropertyName("P4_Permanent")]
    public double P4Permanent { get; set; }

    [JsonPropertyName("Surplus_A4P4")]
    public double SurplusA4P4 { get; set; }
}

// General Financial Stability Indicators DTOs
public class GeneralFinancialStabilityData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public StabilitySourceData? OwnWorkingCapital { get; set; }
    public StabilitySourceData? OwnPlusLongTerm { get; set; }
    public StabilitySourceData? TotalAvailable { get; set; }

    [JsonPropertyName("Stocks_Inventory")]
    public StabilitySourceData? StocksInventory { get; set; }

    [JsonPropertyName("Deficit_OwnCapital")]
    public StabilitySourceData? DeficitOwnCapital { get; set; }

    [JsonPropertyName("Deficit_OwnPlusLongTerm")]
    public StabilitySourceData? DeficitOwnPlusLongTerm { get; set; }

    [JsonPropertyName("Deficit_TotalSources")]
    public StabilitySourceData? DeficitTotalSources { get; set; }
}

public class StabilitySourceData
{
    [JsonPropertyName("Base_Begin")]
    public double BaseBegin { get; set; }

    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Current_Begin")]
    public double CurrentBegin { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }
}

// Sources of Capital Formation DTOs
public class SourcesOfCapitalData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public CapitalSourceMetricData? TotalCapital { get; set; }
    public CapitalSourceMetricData? Equity { get; set; }
    public CapitalComponentData? OwnCurrentAssets { get; set; }
    public CapitalSourceMetricData? BorrowedCapital { get; set; }
    public CapitalComponentData? LongTermLiabilities { get; set; }
    public CapitalComponentData? ShortTermLoans { get; set; }
    public CapitalComponentData? AccountsPayable { get; set; }
    public CapitalComponentData? OtherCurrentLiabilities { get; set; }
}

public class CapitalSourceMetricData
{
    [JsonPropertyName("Base_Begin")]
    public double BaseBegin { get; set; }

    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Base_Change")]
    public double BaseChange { get; set; }

    [JsonPropertyName("Base_PercentBegin")]
    public double BasePercentBegin { get; set; }

    [JsonPropertyName("Base_PercentEnd")]
    public double BasePercentEnd { get; set; }

    [JsonPropertyName("Current_Begin")]
    public double CurrentBegin { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    [JsonPropertyName("Current_Change")]
    public double CurrentChange { get; set; }

    [JsonPropertyName("Current_PercentBegin")]
    public double CurrentPercentBegin { get; set; }

    [JsonPropertyName("Current_PercentEnd")]
    public double CurrentPercentEnd { get; set; }
}

public class CapitalComponentData
{
    [JsonPropertyName("Base_End")]
    public double BaseEnd { get; set; }

    [JsonPropertyName("Base_Percent")]
    public double BasePercent { get; set; }

    [JsonPropertyName("Current_End")]
    public double CurrentEnd { get; set; }

    [JsonPropertyName("Current_Percent")]
    public double CurrentPercent { get; set; }
}
