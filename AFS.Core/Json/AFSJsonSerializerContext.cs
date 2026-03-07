using AFS.Core.Models;
using System.Text.Json.Serialization;

namespace AFS.Core.Json;

/// <summary>
/// Main AOT-compatible JSON serialization context.
/// All types that need JSON serialization must be registered here.
/// </summary>
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
[JsonSerializable(typeof(IReadOnlyList<ChartDataItemDto>))]
[JsonSerializable(typeof(IReadOnlyList<TurnoverDataPoint>))]
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
    GenerationMode = JsonSourceGenerationMode.Default,
    UseStringEnumConverter = true)]
public partial class AFSJsonSerializerContext : JsonSerializerContext
{
}
