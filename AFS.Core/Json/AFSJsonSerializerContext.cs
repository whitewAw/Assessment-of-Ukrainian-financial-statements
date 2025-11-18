using AFS.Core.Models;
using System.Text.Json.Serialization;

namespace AFS.Core.Json;

// Main AOT-compatible JSON serialization context
[JsonSerializable(typeof(AFSModel))]
[JsonSerializable(typeof(Form1))]
[JsonSerializable(typeof(Form2))]
[JsonSerializable(typeof(AdditionalCompanyInfo))]
[JsonSerializable(typeof(FixedAssetsInfo))]
[JsonSerializable(typeof(BeginEnd))]
[JsonSerializable(typeof(CurrentPrevious))]
[JsonSerializable(typeof(ChartDataItem))]
[JsonSerializable(typeof(ChartDateTimeItem))]
[JsonSerializable(typeof(List<ChartDataItem>))]
[JsonSerializable(typeof(List<ChartDateTimeItem>))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(Dictionary<string, double>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
// Chart data types
[JsonSerializable(typeof(AssetCompositionData))]
[JsonSerializable(typeof(CapitalSourcesData))]
[JsonSerializable(typeof(PayableStructureData))]
[JsonSerializable(typeof(TurnoverTimeData))]
[JsonSerializable(typeof(ChartDataItemDto))]
[JsonSerializable(typeof(TurnoverDataPoint))]
[JsonSerializable(typeof(List<ChartDataItemDto>))]
[JsonSerializable(typeof(List<TurnoverDataPoint>))]
// OpenAI types
[JsonSerializable(typeof(OpenAIRequest))]
[JsonSerializable(typeof(OpenAIResponse))]
[JsonSerializable(typeof(OpenAIMessage))]
[JsonSerializable(typeof(OpenAIChoice))]
[JsonSerializable(typeof(OpenAIMessage[]))]
[JsonSerializable(typeof(OpenAIChoice[]))]
// AI Chat financial context types
[JsonSerializable(typeof(FinancialContextData))]
[JsonSerializable(typeof(CompanyInfoData))]
[JsonSerializable(typeof(BalanceSheetData))]
[JsonSerializable(typeof(YearBalanceData))]
[JsonSerializable(typeof(BalanceItemData))]
[JsonSerializable(typeof(IncomeStatementData))]
[JsonSerializable(typeof(YearIncomeData))]
// Financial Stability Classification types
[JsonSerializable(typeof(StabilityClassificationData))]
[JsonSerializable(typeof(StabilityTypeData))]
// Receivable and Payable Assessment types
[JsonSerializable(typeof(ReceivablePayableData))]
[JsonSerializable(typeof(ReceivablePayableSummary))]
[JsonSerializable(typeof(ReceivablePayableCategoryData))]
// Solvency Ratios types
[JsonSerializable(typeof(SolvencyRatiosData))]
[JsonSerializable(typeof(SolvencyRatioItem))]
[JsonSerializable(typeof(SolvencyRatioSimpleItem))]
// Factor Analysis types
[JsonSerializable(typeof(FactorAnalysisData))]
[JsonSerializable(typeof(FactorMetricData))]
// Business Activity Indicators types
[JsonSerializable(typeof(BusinessActivityData))]
[JsonSerializable(typeof(BusinessActivityMetricData))]
[JsonSerializable(typeof(TurnoverMetricData))]
// Intangible Assets Efficiency types
[JsonSerializable(typeof(IntangibleAssetsData))]
[JsonSerializable(typeof(IntangibleAssetMetricData))]
// Financial Stability Indicators types
[JsonSerializable(typeof(FinancialStabilityIndicatorsData))]
[JsonSerializable(typeof(StabilityIndicatorMetricData))]
// Liquidity Indicators of Balance types
[JsonSerializable(typeof(LiquidityIndicatorsData))]
[JsonSerializable(typeof(LiquidityPeriodData))]
[JsonSerializable(typeof(LiquidityConditionData))]
// General Financial Stability Indicators types
[JsonSerializable(typeof(GeneralFinancialStabilityData))]
[JsonSerializable(typeof(StabilitySourceData))]
// Sources of Capital Formation types
[JsonSerializable(typeof(SourcesOfCapitalData))]
[JsonSerializable(typeof(CapitalSourceMetricData))]
[JsonSerializable(typeof(CapitalComponentData))]
[JsonSourceGenerationOptions(
    WriteIndented = false,
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    GenerationMode = JsonSourceGenerationMode.Default)]
public partial class AFSJsonSerializerContext : JsonSerializerContext
{
}

// DTOs for chart data serialization - optimized for AI prompts
public class ChartDataItemDto
{
    public string? Item { get; set; }
    public double Value { get; set; }
}

public class AssetCompositionData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public List<ChartDataItemDto>? BeginningOfYear { get; set; }
    public List<ChartDataItemDto>? EndOfYear { get; set; }
}

public class CapitalSourcesData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public int PreviousYear { get; set; }
    public List<ChartDataItemDto>? CapitalSources { get; set; }
}

public class PayableStructureData
{
    public string? CompanyName { get; set; }
    public int Year { get; set; }
    public int PreviousYear { get; set; }
    public List<ChartDataItemDto>? PayableStructure { get; set; }
}

public class TurnoverDataPoint
{
    public string? Date { get; set; }
    public double Value { get; set; }
}

public class TurnoverTimeData
{
    public string? CompanyName { get; set; }
    public List<TurnoverDataPoint>? Money { get; set; }
    public List<TurnoverDataPoint>? Receivables { get; set; }
    public List<TurnoverDataPoint>? MaterialValues { get; set; }
}

// OpenAI API DTOs
public class OpenAIMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class OpenAIRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public OpenAIMessage[] Messages { get; set; } = Array.Empty<OpenAIMessage>();

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
}

public class OpenAIChoice
{
    [JsonPropertyName("message")]
    public OpenAIMessage? Message { get; set; }
}

public class OpenAIResponse
{
    [JsonPropertyName("choices")]
    public OpenAIChoice[] Choices { get; set; } = Array.Empty<OpenAIChoice>();
}

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
    public bool Base_Current { get; set; }
    public bool Base_ShortTerm { get; set; }
    public bool Base_LongTerm { get; set; }
    public bool Current_Current { get; set; }
    public bool Current_ShortTerm { get; set; }
    public bool Current_LongTerm { get; set; }
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
    public double TotalReceivables_Base { get; set; }
    public double TotalReceivables_Current { get; set; }
    public double TotalPayables_Base { get; set; }
    public double TotalPayables_Current { get; set; }
    public double NetPosition_Base { get; set; }
    public double NetPosition_Current { get; set; }
    public double ReceivableToPayableRatio_Base { get; set; }
    public double ReceivableToPayableRatio_Current { get; set; }
}

public class ReceivablePayableCategoryData
{
    public double Receivable_Base { get; set; }
    public double Receivable_Current { get; set; }
    public double Payable_Base { get; set; }
    public double Payable_Current { get; set; }
    public double ExcessReceivable_Base { get; set; }
    public double ExcessReceivable_Current { get; set; }
    public double ExcessPayable_Base { get; set; }
    public double ExcessPayable_Current { get; set; }
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
    public double Base_Begin { get; set; }
    public double Base_End { get; set; }
    public double Current_Begin { get; set; }
    public double Current_End { get; set; }
    public double Deviation_Base { get; set; }
    public double Deviation_Current { get; set; }
}

public class SolvencyRatioSimpleItem
{
    public double Base_End { get; set; }
    public double Current_End { get; set; }
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
    public double Revolutions_Base { get; set; }
    public double Revolutions_Current { get; set; }
    public double Days_Base { get; set; }
    public double Days_Current { get; set; }
}

// Intangible Assets Efficiency DTOs
public class IntangibleAssetsData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public IntangibleAssetMetricData? NetRevenueFromSales { get; set; }
    public IntangibleAssetMetricData? AverageCostOfIntangibleAssets { get; set; }
    public IntangibleAssetMetricData? IntangibleAssetTurnover_UAH { get; set; }
    public IntangibleAssetMetricData? CapitalIntensityOfProduction_UAH { get; set; }
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
    public LiquidityPeriodData? BaseYear_Assessment { get; set; }
    public LiquidityPeriodData? CurrentYear_Assessment { get; set; }
}

public class LiquidityPeriodData
{
    public LiquidityConditionData? BeginOfYear { get; set; }
    public LiquidityConditionData? EndOfYear { get; set; }
}

public class LiquidityConditionData
{
    public bool IsLiquid { get; set; }
    public double A1_MostLiquid { get; set; }
    public double P1_MostUrgent { get; set; }
    public double Surplus_A1P1 { get; set; }
    public double A2_QuickLiquid { get; set; }
    public double P2_ShortTerm { get; set; }
    public double Surplus_A2P2 { get; set; }
    public double A3_SlowLiquid { get; set; }
    public double P3_LongTerm { get; set; }
    public double Surplus_A3P3 { get; set; }
    public double A4_HardToSell { get; set; }
    public double P4_Permanent { get; set; }
    public double Surplus_A4P4 { get; set; }
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
    public StabilitySourceData? Stocks_Inventory { get; set; }
    public StabilitySourceData? Deficit_OwnCapital { get; set; }
    public StabilitySourceData? Deficit_OwnPlusLongTerm { get; set; }
    public StabilitySourceData? Deficit_TotalSources { get; set; }
}

public class StabilitySourceData
{
    public double Base_Begin { get; set; }
    public double Base_End { get; set; }
    public double Current_Begin { get; set; }
    public double Current_End { get; set; }
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
    public double Base_Begin { get; set; }
    public double Base_End { get; set; }
    public double Base_Change { get; set; }
    public double Base_PercentBegin { get; set; }
    public double Base_PercentEnd { get; set; }
    public double Current_Begin { get; set; }
    public double Current_End { get; set; }
    public double Current_Change { get; set; }
    public double Current_PercentBegin { get; set; }
    public double Current_PercentEnd { get; set; }
}

public class CapitalComponentData
{
    public double Base_End { get; set; }
    public double Base_Percent { get; set; }
    public double Current_End { get; set; }
    public double Current_Percent { get; set; }
}
