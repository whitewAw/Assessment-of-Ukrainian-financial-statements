using AFS.Core.Interfaces;
using AFS.Core.Json;
using AFS.Core.Models;
using System.Text.Json;

namespace AFS.Core.Services;

/// <summary>
/// Helper service for AOT-compatible JSON serialization without reflection.
/// All methods use strongly-typed interfaces instead of reflection.
/// </summary>
public static class JsonSerializationHelper
{
    /// <summary>
    /// Serialize asset composition data for AI prompts
    /// </summary>
    public static string SerializeAssetComposition(
        string companyName,
        int year,
        List<ChartDataItem>? beginningOfYear,
        List<ChartDataItem>? endOfYear)
    {
        var data = new AssetCompositionData
        {
            CompanyName = companyName,
            Year = year,
            BeginningOfYear = beginningOfYear?.Select(item => new ChartDataItemDto
            {
                Item = item.Item,
                Value = item.Value ?? 0
            }).ToList(),
            EndOfYear = endOfYear?.Select(item => new ChartDataItemDto
            {
                Item = item.Item,
                Value = item.Value ?? 0
            }).ToList()
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.AssetCompositionData);
    }

    /// <summary>
    /// Serialize capital sources data for AI prompts
    /// </summary>
    public static string SerializeCapitalSources(
        string companyName,
        int year,
        int previousYear,
        List<ChartDataItem>? capitalSources)
    {
        var data = new CapitalSourcesData
        {
            CompanyName = companyName,
            Year = year,
            PreviousYear = previousYear,
            CapitalSources = capitalSources?.Select(item => new ChartDataItemDto
            {
                Item = item.Item,
                Value = item.Value ?? 0
            }).ToList()
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.CapitalSourcesData);
    }

    /// <summary>
    /// Serialize payable structure data for AI prompts
    /// </summary>
    public static string SerializePayableStructure(
        string companyName,
        int year,
        int previousYear,
        List<ChartDataItem>? payableStructure)
    {
        var data = new PayableStructureData
        {
            CompanyName = companyName,
            Year = year,
            PreviousYear = previousYear,
            PayableStructure = payableStructure?.Select(item => new ChartDataItemDto
            {
                Item = item.Item,
                Value = item.Value ?? 0
            }).ToList()
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.PayableStructureData);
    }

    /// <summary>
    /// Serialize turnover time data for AI prompts
    /// </summary>
    public static string SerializeTurnoverTime(
        string companyName,
        List<ChartDateTimeItem>? money,
        List<ChartDateTimeItem>? receivables,
        List<ChartDateTimeItem>? materialValues)
    {
        var data = new TurnoverTimeData
        {
            CompanyName = companyName,
            Money = money?.Select(item => new TurnoverDataPoint
            {
                Date = item.Date.ToString("MM/yyyy"),
                Value = item.Value ?? 0
            }).ToList(),
            Receivables = receivables?.Select(item => new TurnoverDataPoint
            {
                Date = item.Date.ToString("MM/yyyy"),
                Value = item.Value ?? 0
            }).ToList(),
            MaterialValues = materialValues?.Select(item => new TurnoverDataPoint
            {
                Date = item.Date.ToString("MM/yyyy"),
                Value = item.Value ?? 0
            }).ToList()
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.TurnoverTimeData);
    }

    /// <summary>
    /// Serialize financial context data for AI chat
    /// </summary>
    public static string SerializeFinancialContext(AFSModel model)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        var context = new FinancialContextData
        {
            Company = new CompanyInfoData
            {
                Name = model.CompanyName,
                BaseYear = model.BaseYear,
                CurrentYear = model.CurrentYear
            },
            BalanceSheet = new BalanceSheetData
            {
                BaseYear = new YearBalanceData
                {
                    TotalAssets = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Base.GetF1300Begin()),
                        End = SafeValue(model.F1Base.GetF1300End())
                    },
                    NonCurrentAssets = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Base.GetF1095Begin()),
                        End = SafeValue(model.F1Base.GetF1095End())
                    },
                    CurrentAssets = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Base.GetF1195Begin()),
                        End = SafeValue(model.F1Base.GetF1195End())
                    },
                    Equity = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Base.GetF1495Begin()),
                        End = SafeValue(model.F1Base.GetF1495End())
                    },
                    TotalLiabilities = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Base.GetF1900Begin()),
                        End = SafeValue(model.F1Base.GetF1900End())
                    },
                    CurrentLiabilities = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Base.GetF1695Begin()),
                        End = SafeValue(model.F1Base.GetF1695End())
                    }
                },
                CurrentYear = new YearBalanceData
                {
                    TotalAssets = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Current.GetF1300Begin()),
                        End = SafeValue(model.F1Current.GetF1300End())
                    },
                    NonCurrentAssets = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Current.GetF1095Begin()),
                        End = SafeValue(model.F1Current.GetF1095End())
                    },
                    CurrentAssets = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Current.GetF1195Begin()),
                        End = SafeValue(model.F1Current.GetF1195End())
                    },
                    Equity = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Current.GetF1495Begin()),
                        End = SafeValue(model.F1Current.GetF1495End())
                    },
                    TotalLiabilities = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Current.GetF1900Begin()),
                        End = SafeValue(model.F1Current.GetF1900End())
                    },
                    CurrentLiabilities = new BalanceItemData
                    {
                        Beginning = SafeValue(model.F1Current.GetF1695Begin()),
                        End = SafeValue(model.F1Current.GetF1695End())
                    }
                }
            },
            IncomeStatement = new IncomeStatementData
            {
                BaseYear = new YearIncomeData
                {
                    Revenue = SafeValue(model.F2Base.F2000.Current),
                    GrossProfit = SafeValue(model.F2Base.F2050.Current),
                    OperatingProfit = SafeValue(model.F2Base.GetF2190Current()),
                    NetProfit = SafeValue(model.F2Base.GetF2350Current())
                },
                CurrentYear = new YearIncomeData
                {
                    Revenue = SafeValue(model.F2Current.F2000.Current),
                    GrossProfit = SafeValue(model.F2Current.F2050.Current),
                    OperatingProfit = SafeValue(model.F2Current.GetF2190Current()),
                    NetProfit = SafeValue(model.F2Current.GetF2350Current())
                }
            }
        };

        return JsonSerializer.Serialize(context, AFSJsonSerializerContext.Default.FinancialContextData);
    }

    /// <summary>
    /// Serialize stability classification data for AI analysis
    /// </summary>
    public static string SerializeStabilityClassification(
        string companyName,
        int baseYear,
        int currentYear,
        IHasStabilityValues absoluteStability,
        IHasStabilityValues normalStability,
        IHasStabilityValues precrisisStability,
        IHasStabilityValues crisisStability)
    {
        bool IsPass(string value) => value == "+";

        var data = new StabilityClassificationData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            AbsoluteStability = new StabilityTypeData
            {
                Type = "Type 1: Absolute Financial Stability",
                Base_Current = IsPass(absoluteStability.CurrentBVal),
                Base_ShortTerm = IsPass(absoluteStability.ShortBVal),
                Base_LongTerm = IsPass(absoluteStability.LongBVal),
                Current_Current = IsPass(absoluteStability.CurrentCVal),
                Current_ShortTerm = IsPass(absoluteStability.ShortCVal),
                Current_LongTerm = IsPass(absoluteStability.LongCVal)
            },
            NormalStability = new StabilityTypeData
            {
                Type = "Type 2: Normal Financial Stability",
                Base_Current = IsPass(normalStability.CurrentBVal),
                Base_ShortTerm = IsPass(normalStability.ShortBVal),
                Base_LongTerm = IsPass(normalStability.LongBVal),
                Current_Current = IsPass(normalStability.CurrentCVal),
                Current_ShortTerm = IsPass(normalStability.ShortCVal),
                Current_LongTerm = IsPass(normalStability.LongCVal)
            },
            PreCrisisStability = new StabilityTypeData
            {
                Type = "Type 3: Pre-Crisis Financial Stability",
                Base_Current = IsPass(precrisisStability.CurrentBVal),
                Base_ShortTerm = IsPass(precrisisStability.ShortBVal),
                Base_LongTerm = IsPass(precrisisStability.LongBVal),
                Current_Current = IsPass(precrisisStability.CurrentCVal),
                Current_ShortTerm = IsPass(precrisisStability.ShortCVal),
                Current_LongTerm = IsPass(precrisisStability.LongCVal)
            },
            CrisisStability = new StabilityTypeData
            {
                Type = "Type 4: Crisis Financial Stability",
                Base_Current = IsPass(crisisStability.CurrentBVal),
                Base_ShortTerm = IsPass(crisisStability.ShortBVal),
                Base_LongTerm = IsPass(crisisStability.LongBVal),
                Current_Current = IsPass(crisisStability.CurrentCVal),
                Current_ShortTerm = IsPass(crisisStability.ShortCVal),
                Current_LongTerm = IsPass(crisisStability.LongCVal)
            }
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.StabilityClassificationData);
    }

    /// <summary>
    /// Serialize receivable and payable assessment data for AI analysis
    /// </summary>
    public static string SerializeReceivablePayable(
        string companyName,
        int baseYear,
        int currentYear,
        IHasReceivablePayable totalRow,
        IHasReceivablePayable buyersSuppliers,
        IHasReceivablePayable budgetFunds,
        IHasReceivablePayable advances,
        IHasReceivablePayable others)
    {
        var totalRecBase = SafeValue(totalRow.ReceivableBase);
        var totalRecCurrent = SafeValue(totalRow.ReceivableCurrent);
        var totalPayBase = SafeValue(totalRow.PayableBase);
        var totalPayCurrent = SafeValue(totalRow.PayableCurrent);

        var data = new ReceivablePayableData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Summary = new ReceivablePayableSummary
            {
                TotalReceivables_Base = totalRecBase,
                TotalReceivables_Current = totalRecCurrent,
                TotalPayables_Base = totalPayBase,
                TotalPayables_Current = totalPayCurrent,
                NetPosition_Base = totalRecBase - totalPayBase,
                NetPosition_Current = totalRecCurrent - totalPayCurrent,
                ReceivableToPayableRatio_Base = totalPayBase > 0 ? totalRecBase / totalPayBase : 0,
                ReceivableToPayableRatio_Current = totalPayCurrent > 0 ? totalRecCurrent / totalPayCurrent : 0
            },
            BuyersSuppliers = CreateReceivablePayableCategory(buyersSuppliers),
            BudgetFunds = CreateReceivablePayableCategory(budgetFunds),
            Advances = CreateReceivablePayableCategory(advances),
            Others = CreateReceivablePayableCategory(others)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.ReceivablePayableData);
    }

    /// <summary>
    /// Serialize solvency ratios data for AI analysis
    /// </summary>
    public static string SerializeSolvencyRatios(
        string companyName,
        int baseYear,
        int currentYear,
        IHasSolvencyRatio overallLiquidity,
        IHasSolvencyRatio absoluteLiquidity,
        IHasSolvencyRatio intermediateCoverage,
        IHasSolvencyRatio currentLiquidity,
        IHasSolvencyRatio recoverySolvency,
        IHasSolvencyRatio lossSolvency)
    {
        var data = new SolvencyRatiosData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            OverallLiquidityRatio = CreateSolvencyRatioItem(overallLiquidity),
            AbsoluteLiquidityRatio = CreateSolvencyRatioItem(absoluteLiquidity),
            IntermediateCoverageRatio = CreateSolvencyRatioItem(intermediateCoverage),
            CurrentLiquidityRatio = CreateSolvencyRatioItem(currentLiquidity),
            RecoverySolvencyRatio = CreateSolvencyRatioSimpleItem(recoverySolvency),
            LossSolvencyRatio = CreateSolvencyRatioSimpleItem(lossSolvency)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.SolvencyRatiosData);
    }

    /// <summary>
    /// Serialize factor analysis data for AI analysis
    /// </summary>
    public static string SerializeFactorAnalysis(
        string companyName,
        int baseYear,
        int currentYear,
        IHasBaseCurrentYear netRevenue,
        IHasBaseCurrentYear avgEmployees,
        IHasBaseCurrentYear laborProductivity,
        IHasBaseCurrentYear avgFixedAssets,
        IHasBaseCurrentYear capitalIntensity,
        IHasBaseCurrentYear fixedAssetTurnover)
    {
        var data = new FactorAnalysisData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            NetRevenueFromSales = CreateFactorMetric(netRevenue),
            AverageNumberOfEmployees = CreateFactorMetric(avgEmployees),
            LaborProductivity = CreateFactorMetric(laborProductivity),
            AverageCostOfFixedAssets = CreateFactorMetric(avgFixedAssets),
            CapitalIntensity = CreateFactorMetric(capitalIntensity),
            FixedAssetTurnover = CreateFactorMetric(fixedAssetTurnover)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.FactorAnalysisData);
    }

    /// <summary>
    /// Serialize business activity indicators for AI analysis
    /// </summary>
    public static string SerializeBusinessActivity(
        string companyName,
        int baseYear,
        int currentYear,
        IHasBaseCurrentYear grossProfitMargin,
        IHasBaseCurrentYear businessActivityRatio,
        IHasBaseCurrentYear financialResourceEfficiency,
        IHasBaseCurrentYear ownFundsUtilization,
        IHasBaseCurrentYear enterpriseProfitability,
        IHasBaseCurrentYear laborProductivity,
        IHasBaseCurrentYear fixedAssetTurnover,
        IHasBaseCurrentYear receivablesTurnoverRevolutions,
        IHasBaseCurrentYear receivablesTurnoverDays,
        IHasBaseCurrentYear inventoryTurnoverRevolutions,
        IHasBaseCurrentYear inventoryTurnoverDays,
        IHasBaseCurrentYear operatingCycle,
        IHasBaseCurrentYear currentAssetsTurnoverRevolutions,
        IHasBaseCurrentYear currentAssetsTurnoverDays,
        IHasBaseCurrentYear equityTurnover,
        IHasBaseCurrentYear totalCapitalTurnover,
        IHasBaseCurrentYear economicGrowthStability,
        IHasBaseCurrentYear equityPaybackPeriod)
    {
        var data = new BusinessActivityData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            GrossProfitMargin = CreateBusinessActivityMetric(grossProfitMargin),
            BusinessActivityRatio = CreateBusinessActivityMetric(businessActivityRatio),
            FinancialResourceEfficiency = CreateBusinessActivityMetric(financialResourceEfficiency),
            OwnFundsUtilization = CreateBusinessActivityMetric(ownFundsUtilization),
            EnterpriseProfitability = CreateBusinessActivityMetric(enterpriseProfitability),
            LaborProductivity = CreateBusinessActivityMetric(laborProductivity),
            FixedAssetTurnover = CreateBusinessActivityMetric(fixedAssetTurnover),
            ReceivablesTurnover = CreateTurnoverMetric(receivablesTurnoverRevolutions, receivablesTurnoverDays),
            InventoryTurnover = CreateTurnoverMetric(inventoryTurnoverRevolutions, inventoryTurnoverDays),
            OperatingCycle = CreateBusinessActivityMetric(operatingCycle),
            CurrentAssetsTurnover = CreateTurnoverMetric(currentAssetsTurnoverRevolutions, currentAssetsTurnoverDays),
            EquityTurnover = CreateBusinessActivityMetric(equityTurnover),
            TotalCapitalTurnover = CreateBusinessActivityMetric(totalCapitalTurnover),
            EconomicGrowthStability = CreateBusinessActivityMetric(economicGrowthStability),
            EquityPaybackPeriod = CreateBusinessActivityMetric(equityPaybackPeriod)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.BusinessActivityData);
    }

    /// <summary>
    /// Serialize intangible assets efficiency data for AI analysis
    /// </summary>
    public static string SerializeIntangibleAssets(
        string companyName,
        int baseYear,
        int currentYear,
        IHasBaseCurrentYear netRevenue,
        IHasBaseCurrentYear avgCostIntangibles,
        IHasBaseCurrentYear intangibleTurnover,
        IHasBaseCurrentYear capitalIntensity)
    {
        var data = new IntangibleAssetsData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            NetRevenueFromSales = CreateIntangibleAssetMetric(netRevenue),
            AverageCostOfIntangibleAssets = CreateIntangibleAssetMetric(avgCostIntangibles),
            IntangibleAssetTurnover_UAH = CreateIntangibleAssetMetric(intangibleTurnover),
            CapitalIntensityOfProduction_UAH = CreateIntangibleAssetMetric(capitalIntensity)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.IntangibleAssetsData);
    }

    /// <summary>
    /// Serialize financial stability indicators for AI analysis
    /// </summary>
    public static string SerializeFinancialStabilityIndicators(
        string companyName,
        int baseYear,
        int currentYear,
        IHasBaseCurrentYear totalReturnOnAssets,
        IHasBaseCurrentYear independenceRatio,
        IHasBaseCurrentYear financialLeverage,
        IHasBaseCurrentYear financialStability,
        IHasBaseCurrentYear maneuverability,
        IHasBaseCurrentYear borrowedCapitalConcentration,
        IHasBaseCurrentYear longTermInvestmentStructure,
        IHasBaseCurrentYear longTermBorrowing,
        IHasBaseCurrentYear capitalStructureRatio,
        IHasBaseCurrentYear debtToEquity,
        IHasBaseCurrentYear ownFundsInInventories,
        IHasBaseCurrentYear mobileToImmobilized,
        IHasBaseCurrentYear totalCoverage)
    {
        var data = new FinancialStabilityIndicatorsData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            TotalReturnOnAssets = CreateStabilityIndicatorMetric(totalReturnOnAssets),
            IndependenceRatio = CreateStabilityIndicatorMetric(independenceRatio),
            FinancialLeverageRatio = CreateStabilityIndicatorMetric(financialLeverage),
            FinancialStabilityRatio = CreateStabilityIndicatorMetric(financialStability),
            ManeuverabilityRatio = CreateStabilityIndicatorMetric(maneuverability),
            BorrowedCapitalConcentration = CreateStabilityIndicatorMetric(borrowedCapitalConcentration),
            LongTermInvestmentStructure = CreateStabilityIndicatorMetric(longTermInvestmentStructure),
            LongTermBorrowingRatio = CreateStabilityIndicatorMetric(longTermBorrowing),
            CapitalStructureRatio = CreateStabilityIndicatorMetric(capitalStructureRatio),
            DebtToEquityRatio = CreateStabilityIndicatorMetric(debtToEquity),
            OwnFundsInInventories = CreateStabilityIndicatorMetric(ownFundsInInventories),
            MobileToImmobilizedRatio = CreateStabilityIndicatorMetric(mobileToImmobilized),
            TotalCoverageRatio = CreateStabilityIndicatorMetric(totalCoverage)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.FinancialStabilityIndicatorsData);
    }

    /// <summary>
    /// Serialize liquidity indicators of balance for AI analysis
    /// </summary>
    public static string SerializeLiquidityIndicators(
        string companyName,
        int baseYear,
        int currentYear,
        IHasLiquidityData a1p1Base,
        IHasLiquidityData a2p2Base,
        IHasLiquidityData a3p3Base,
        IHasLiquidityData a4p4Base,
        IHasLiquidityData a1p1Current,
        IHasLiquidityData a2p2Current,
        IHasLiquidityData a3p3Current,
        IHasLiquidityData a4p4Current)
    {
        var data = new LiquidityIndicatorsData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            BaseYear_Assessment = new LiquidityPeriodData
            {
                BeginOfYear = CreateLiquidityConditionBegin(a1p1Base, a2p2Base, a3p3Base, a4p4Base),
                EndOfYear = CreateLiquidityConditionEnd(a1p1Base, a2p2Base, a3p3Base, a4p4Base)
            },
            CurrentYear_Assessment = new LiquidityPeriodData
            {
                BeginOfYear = CreateLiquidityConditionBegin(a1p1Current, a2p2Current, a3p3Current, a4p4Current),
                EndOfYear = CreateLiquidityConditionEnd(a1p1Current, a2p2Current, a3p3Current, a4p4Current)
            }
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.LiquidityIndicatorsData);
    }

    /// <summary>
    /// Serialize general financial stability indicators for AI analysis
    /// </summary>
    public static string SerializeGeneralFinancialStability<T>(
        string companyName,
        int baseYear,
        int currentYear,
        IHasBaseCurrent<T> ownWorkingCapital,
        IHasBaseCurrent<T> ownPlusLongTerm,
        IHasBaseCurrent<T> totalAvailable,
        IHasBaseCurrent<T> stocks,
        IHasBaseCurrent<T> deficitOwnCapital,
        IHasBaseCurrent<T> deficitOwnPlusLongTerm,
        IHasBaseCurrent<T> deficitTotalSources)
        where T : IHasBeginEnd
    {
        var data = new GeneralFinancialStabilityData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            OwnWorkingCapital = CreateStabilitySource(ownWorkingCapital),
            OwnPlusLongTerm = CreateStabilitySource(ownPlusLongTerm),
            TotalAvailable = CreateStabilitySource(totalAvailable),
            Stocks_Inventory = CreateStabilitySource(stocks),
            Deficit_OwnCapital = CreateStabilitySource(deficitOwnCapital),
            Deficit_OwnPlusLongTerm = CreateStabilitySource(deficitOwnPlusLongTerm),
            Deficit_TotalSources = CreateStabilitySource(deficitTotalSources)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.GeneralFinancialStabilityData);
    }

    /// <summary>
    /// Serialize sources of capital formation for AI analysis
    /// </summary>
    public static string SerializeSourcesOfCapital<T>(
        string companyName,
        int baseYear,
        int currentYear,
        IHasBaseCurrent<T> totalCapital,
        ICapitalSourceMetric<T> equity,
        ICapitalComponentEquity<T> ownCurrentAssets,
        ICapitalSourceMetric<T> borrowedCapital,
        ICapitalComponentBorrowed<T> longTermLiabilities,
        ICapitalComponentBorrowed<T> shortTermLoans,
        ICapitalComponentBorrowed<T> accountsPayable,
        ICapitalComponentBorrowed<T> otherCurrentLiabilities)
        where T : IHasBeginEnd
    {
        var data = new SourcesOfCapitalData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            TotalCapital = CreateCapitalSourceMetricFromBaseCurrent(totalCapital),
            Equity = CreateCapitalSourceMetric(equity),
            OwnCurrentAssets = CreateCapitalComponentFromEquity(ownCurrentAssets),
            BorrowedCapital = CreateCapitalSourceMetric(borrowedCapital),
            LongTermLiabilities = CreateCapitalComponentFromBorrowed(longTermLiabilities),
            ShortTermLoans = CreateCapitalComponentFromBorrowed(shortTermLoans),
            AccountsPayable = CreateCapitalComponentFromBorrowed(accountsPayable),
            OtherCurrentLiabilities = CreateCapitalComponentFromBorrowed(otherCurrentLiabilities)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.SourcesOfCapitalData);
    }

    // ============ Private Helper Methods - No Reflection ============

    private static double SafeValue(double value) =>
        double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

    private static double CalculatePercentageChange(double baseValue, double currentValue)
    {
        if (baseValue == 0 || double.IsNaN(baseValue) || double.IsInfinity(baseValue)) return 0;
        if (double.IsNaN(currentValue) || double.IsInfinity(currentValue)) return 0;
        var result = ((currentValue - baseValue) / baseValue) * 100;
        if (double.IsNaN(result) || double.IsInfinity(result)) return 0;
        return result;
    }

    private static CapitalSourceMetricData CreateCapitalSourceMetricFromBaseCurrent<T>(IHasBaseCurrent<T> source)
        where T : IHasBeginEnd
    {
        var baseBegin = SafeValue(source.Base.BeginningOfyear);
        var baseEnd = SafeValue(source.Base.EndOfYear);
        var currentBegin = SafeValue(source.Current.BeginningOfyear);
        var currentEnd = SafeValue(source.Current.EndOfYear);

        return new CapitalSourceMetricData
        {
            Base_Begin = baseBegin,
            Base_End = baseEnd,
            Base_Change = baseEnd - baseBegin,
            Base_PercentBegin = 0,
            Base_PercentEnd = 0,
            Current_Begin = currentBegin,
            Current_End = currentEnd,
            Current_Change = currentEnd - currentBegin,
            Current_PercentBegin = 0,
            Current_PercentEnd = 0
        };
    }

    private static CapitalSourceMetricData CreateCapitalSourceMetric<T>(ICapitalSourceMetric<T> source)
        where T : IHasBeginEnd
    {
        return new CapitalSourceMetricData
        {
            Base_Begin = SafeValue(source.Base.BeginningOfyear),
            Base_End = SafeValue(source.Base.EndOfYear),
            Base_Change = SafeValue(source.Base.EndOfYear - source.Base.BeginningOfyear),
            Base_PercentBegin = SafeValue(source.InPercentageOfAssetsBase.BeginningOfyear),
            Base_PercentEnd = SafeValue(source.InPercentageOfAssetsBase.EndOfYear),
            Current_Begin = SafeValue(source.Current.BeginningOfyear),
            Current_End = SafeValue(source.Current.EndOfYear),
            Current_Change = SafeValue(source.Current.EndOfYear - source.Current.BeginningOfyear),
            Current_PercentBegin = SafeValue(source.InPercentageOfAssetsCurrent.BeginningOfyear),
            Current_PercentEnd = SafeValue(source.InPercentageOfAssetsCurrent.EndOfYear)
        };
    }

    private static CapitalComponentData CreateCapitalComponentFromEquity<T>(ICapitalComponentEquity<T> source)
        where T : IHasBeginEnd
    {
        return new CapitalComponentData
        {
            Base_End = SafeValue(source.Base.EndOfYear),
            Base_Percent = SafeValue(source.InPercentageOfEquityBase.EndOfYear),
            Current_End = SafeValue(source.Current.EndOfYear),
            Current_Percent = SafeValue(source.InPercentageOfEquityCurrent.EndOfYear)
        };
    }

    private static CapitalComponentData CreateCapitalComponentFromBorrowed<T>(ICapitalComponentBorrowed<T> source)
        where T : IHasBeginEnd
    {
        return new CapitalComponentData
        {
            Base_End = SafeValue(source.Base.EndOfYear),
            Base_Percent = SafeValue(source.InPercentageOfBorrowedCapitalBase.EndOfYear),
            Current_End = SafeValue(source.Current.EndOfYear),
            Current_Percent = SafeValue(source.InPercentageOfBorrowedCapitalCurrent.EndOfYear)
        };
    }

    private static StabilitySourceData CreateStabilitySource<T>(IHasBaseCurrent<T> source)
        where T : IHasBeginEnd
    {
        return new StabilitySourceData
        {
            Base_Begin = SafeValue(source.Base.BeginningOfyear),
            Base_End = SafeValue(source.Base.EndOfYear),
            Current_Begin = SafeValue(source.Current.BeginningOfyear),
            Current_End = SafeValue(source.Current.EndOfYear)
        };
    }

    private static LiquidityConditionData CreateLiquidityConditionBegin(
        IHasLiquidityData a1, IHasLiquidityData a2, IHasLiquidityData a3, IHasLiquidityData a4)
    {
        bool isLiquid = SafeValue(a1.ABegin) >= SafeValue(a1.PBegin)
                     && SafeValue(a2.ABegin) >= SafeValue(a2.PBegin)
                     && SafeValue(a3.ABegin) >= SafeValue(a3.PBegin)
                     && SafeValue(a4.ABegin) <= SafeValue(a4.PBegin);

        return new LiquidityConditionData
        {
            IsLiquid = isLiquid,
            A1_MostLiquid = SafeValue(a1.ABegin),
            P1_MostUrgent = SafeValue(a1.PBegin),
            Surplus_A1P1 = SafeValue(a1.PaymentBalanceBegin),
            A2_QuickLiquid = SafeValue(a2.ABegin),
            P2_ShortTerm = SafeValue(a2.PBegin),
            Surplus_A2P2 = SafeValue(a2.PaymentBalanceBegin),
            A3_SlowLiquid = SafeValue(a3.ABegin),
            P3_LongTerm = SafeValue(a3.PBegin),
            Surplus_A3P3 = SafeValue(a3.PaymentBalanceBegin),
            A4_HardToSell = SafeValue(a4.ABegin),
            P4_Permanent = SafeValue(a4.PBegin),
            Surplus_A4P4 = SafeValue(a4.PaymentBalanceBegin)
        };
    }

    private static LiquidityConditionData CreateLiquidityConditionEnd(
        IHasLiquidityData a1, IHasLiquidityData a2, IHasLiquidityData a3, IHasLiquidityData a4)
    {
        bool isLiquid = SafeValue(a1.AEnd) >= SafeValue(a1.PEnd)
                     && SafeValue(a2.AEnd) >= SafeValue(a2.PEnd)
                     && SafeValue(a3.AEnd) >= SafeValue(a3.PEnd)
                     && SafeValue(a4.AEnd) <= SafeValue(a4.PEnd);

        return new LiquidityConditionData
        {
            IsLiquid = isLiquid,
            A1_MostLiquid = SafeValue(a1.AEnd),
            P1_MostUrgent = SafeValue(a1.PEnd),
            Surplus_A1P1 = SafeValue(a1.PaymentBalanceEnd),
            A2_QuickLiquid = SafeValue(a2.AEnd),
            P2_ShortTerm = SafeValue(a2.PEnd),
            Surplus_A2P2 = SafeValue(a2.PaymentBalanceEnd),
            A3_SlowLiquid = SafeValue(a3.AEnd),
            P3_LongTerm = SafeValue(a3.PEnd),
            Surplus_A3P3 = SafeValue(a3.PaymentBalanceEnd),
            A4_HardToSell = SafeValue(a4.AEnd),
            P4_Permanent = SafeValue(a4.PEnd),
            Surplus_A4P4 = SafeValue(a4.PaymentBalanceEnd)
        };
    }

    private static StabilityIndicatorMetricData CreateStabilityIndicatorMetric(IHasBaseCurrentYear source)
    {
        var baseYear = SafeValue(source.BaseYear);
        var currentYear = SafeValue(source.CurrentYear);

        return new StabilityIndicatorMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Change = CalculatePercentageChange(baseYear, currentYear)
        };
    }

    private static BusinessActivityMetricData CreateBusinessActivityMetric(IHasBaseCurrentYear source)
    {
        var baseYear = SafeValue(source.BaseYear);
        var currentYear = SafeValue(source.CurrentYear);

        return new BusinessActivityMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Change = CalculatePercentageChange(baseYear, currentYear)
        };
    }

    private static IntangibleAssetMetricData CreateIntangibleAssetMetric(IHasBaseCurrentYear source)
    {
        var baseYear = SafeValue(source.BaseYear);
        var currentYear = SafeValue(source.CurrentYear);

        return new IntangibleAssetMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Deviations = SafeValue(source.Deviations),
            PercentageChange = CalculatePercentageChange(baseYear, currentYear)
        };
    }

    private static TurnoverMetricData CreateTurnoverMetric(
        IHasBaseCurrentYear revolutionsSource,
        IHasBaseCurrentYear daysSource)
    {
        return new TurnoverMetricData
        {
            Revolutions_Base = SafeValue(revolutionsSource.BaseYear),
            Revolutions_Current = SafeValue(revolutionsSource.CurrentYear),
            Days_Base = SafeValue(daysSource.BaseYear),
            Days_Current = SafeValue(daysSource.CurrentYear)
        };
    }

    private static FactorMetricData CreateFactorMetric(IHasBaseCurrentYear source)
    {
        var baseYear = SafeValue(source.BaseYear);
        var currentYear = SafeValue(source.CurrentYear);

        return new FactorMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Deviations = SafeValue(source.Deviations),
            PercentageChange = CalculatePercentageChange(baseYear, currentYear)
        };
    }

    private static ReceivablePayableCategoryData CreateReceivablePayableCategory(IHasReceivablePayable source)
    {
        return new ReceivablePayableCategoryData
        {
            Receivable_Base = SafeValue(source.ReceivableBase),
            Receivable_Current = SafeValue(source.ReceivableCurrent),
            Payable_Base = SafeValue(source.PayableBase),
            Payable_Current = SafeValue(source.PayableCurrent),
            ExcessReceivable_Base = SafeValue(source.ExceedingReceivableBase),
            ExcessReceivable_Current = SafeValue(source.ExceedingReceivableCurrent),
            ExcessPayable_Base = SafeValue(source.ExceedingPayableBase),
            ExcessPayable_Current = SafeValue(source.ExceedingPayableCurrent)
        };
    }

    private static SolvencyRatioItem CreateSolvencyRatioItem(IHasSolvencyRatio source)
    {
        var baseBegin = SafeValue(source.BaseBegin);
        var baseEnd = SafeValue(source.BaseEnd);
        var currentBegin = SafeValue(source.CurrentBegin);
        var currentEnd = SafeValue(source.CurrentEnd);

        return new SolvencyRatioItem
        {
            Base_Begin = baseBegin,
            Base_End = baseEnd,
            Current_Begin = currentBegin,
            Current_End = currentEnd,
            Deviation_Base = baseEnd - baseBegin,
            Deviation_Current = currentEnd - currentBegin
        };
    }

    private static SolvencyRatioSimpleItem CreateSolvencyRatioSimpleItem(IHasSolvencyRatio source)
    {
        var baseEnd = SafeValue(source.BaseEnd);
        var currentEnd = SafeValue(source.CurrentEnd);

        return new SolvencyRatioSimpleItem
        {
            Base_End = baseEnd,
            Current_End = currentEnd,
            Deviation = currentEnd - baseEnd
        };
    }

    /// <summary>
    /// Create JsonSerializerOptions for chart data with proper formatting
    /// </summary>
    public static JsonSerializerOptions CreateChartDataOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
            TypeInfoResolver = AFSJsonSerializerContext.Default
        };
    }
}
