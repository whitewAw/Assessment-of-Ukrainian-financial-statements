using AFS.Core.Json;
using AFS.Core.Models;
using System.Text.Json;

namespace AFS.Core.Services;

/// <summary>
/// Helper service for AOT-compatible JSON serialization
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
        object absoluteStability,
        object normalStability,
        object precrisisStability,
        object crisisStability)
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
                Base_Current = IsPass(GetPropertyValue(absoluteStability, "CurrentBVal")),
                Base_ShortTerm = IsPass(GetPropertyValue(absoluteStability, "ShortBVal")),
                Base_LongTerm = IsPass(GetPropertyValue(absoluteStability, "LongBVal")),
                Current_Current = IsPass(GetPropertyValue(absoluteStability, "CurrentCVal")),
                Current_ShortTerm = IsPass(GetPropertyValue(absoluteStability, "ShortCVal")),
                Current_LongTerm = IsPass(GetPropertyValue(absoluteStability, "LongCVal"))
            },
            NormalStability = new StabilityTypeData
            {
                Type = "Type 2: Normal Financial Stability",
                Base_Current = IsPass(GetPropertyValue(normalStability, "CurrentBVal")),
                Base_ShortTerm = IsPass(GetPropertyValue(normalStability, "ShortBVal")),
                Base_LongTerm = IsPass(GetPropertyValue(normalStability, "LongBVal")),
                Current_Current = IsPass(GetPropertyValue(normalStability, "CurrentCVal")),
                Current_ShortTerm = IsPass(GetPropertyValue(normalStability, "ShortCVal")),
                Current_LongTerm = IsPass(GetPropertyValue(normalStability, "LongCVal"))
            },
            PreCrisisStability = new StabilityTypeData
            {
                Type = "Type 3: Pre-Crisis Financial Stability",
                Base_Current = IsPass(GetPropertyValue(precrisisStability, "CurrentBVal")),
                Base_ShortTerm = IsPass(GetPropertyValue(precrisisStability, "ShortBVal")),
                Base_LongTerm = IsPass(GetPropertyValue(precrisisStability, "LongBVal")),
                Current_Current = IsPass(GetPropertyValue(precrisisStability, "CurrentCVal")),
                Current_ShortTerm = IsPass(GetPropertyValue(precrisisStability, "ShortCVal")),
                Current_LongTerm = IsPass(GetPropertyValue(precrisisStability, "LongCVal"))
            },
            CrisisStability = new StabilityTypeData
            {
                Type = "Type 4: Crisis Financial Stability",
                Base_Current = IsPass(GetPropertyValue(crisisStability, "CurrentBVal")),
                Base_ShortTerm = IsPass(GetPropertyValue(crisisStability, "ShortBVal")),
                Base_LongTerm = IsPass(GetPropertyValue(crisisStability, "LongBVal")),
                Current_Current = IsPass(GetPropertyValue(crisisStability, "CurrentCVal")),
                Current_ShortTerm = IsPass(GetPropertyValue(crisisStability, "ShortCVal")),
                Current_LongTerm = IsPass(GetPropertyValue(crisisStability, "LongCVal"))
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
        object totalRow,
        object buyersSuppliers,
        object budgetFunds,
        object advances,
        object others)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        var totalRecBase = SafeValue(GetDoubleProperty(totalRow, "ReceivableBase"));
        var totalRecCurrent = SafeValue(GetDoubleProperty(totalRow, "ReceivableCurrent"));
        var totalPayBase = SafeValue(GetDoubleProperty(totalRow, "PayableBase"));
        var totalPayCurrent = SafeValue(GetDoubleProperty(totalRow, "PayableCurrent"));

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
            BuyersSuppliers = CreateReceivablePayableCategory(buyersSuppliers, SafeValue),
            BudgetFunds = CreateReceivablePayableCategory(budgetFunds, SafeValue),
            Advances = CreateReceivablePayableCategory(advances, SafeValue),
            Others = CreateReceivablePayableCategory(others, SafeValue)
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
        object overallLiquidity,
        object absoluteLiquidity,
        object intermediateCoverage,
        object currentLiquidity,
        object recoverySolvency,
        object lossSolvency)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        var data = new SolvencyRatiosData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            OverallLiquidityRatio = CreateSolvencyRatioItem(overallLiquidity, SafeValue),
            AbsoluteLiquidityRatio = CreateSolvencyRatioItem(absoluteLiquidity, SafeValue),
            IntermediateCoverageRatio = CreateSolvencyRatioItem(intermediateCoverage, SafeValue),
            CurrentLiquidityRatio = CreateSolvencyRatioItem(currentLiquidity, SafeValue),
            RecoverySolvencyRatio = CreateSolvencyRatioSimpleItem(recoverySolvency, SafeValue),
            LossSolvencyRatio = CreateSolvencyRatioSimpleItem(lossSolvency, SafeValue)
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
        object netRevenue,
        object avgEmployees,
        object laborProductivity,
        object avgFixedAssets,
        object capitalIntensity,
        object fixedAssetTurnover)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        double CalculatePercentageChange(double baseValue, double currentValue)
        {
            if (baseValue == 0 || double.IsNaN(baseValue) || double.IsInfinity(baseValue)) return 0;
            if (double.IsNaN(currentValue) || double.IsInfinity(currentValue)) return 0;
            var result = ((currentValue - baseValue) / baseValue) * 100;
            if (double.IsNaN(result) || double.IsInfinity(result)) return 0;
            return result;
        }

        var data = new FactorAnalysisData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            NetRevenueFromSales = CreateFactorMetric(netRevenue, SafeValue, CalculatePercentageChange),
            AverageNumberOfEmployees = CreateFactorMetric(avgEmployees, SafeValue, CalculatePercentageChange),
            LaborProductivity = CreateFactorMetric(laborProductivity, SafeValue, CalculatePercentageChange),
            AverageCostOfFixedAssets = CreateFactorMetric(avgFixedAssets, SafeValue, CalculatePercentageChange),
            CapitalIntensity = CreateFactorMetric(capitalIntensity, SafeValue, CalculatePercentageChange),
            FixedAssetTurnover = CreateFactorMetric(fixedAssetTurnover, SafeValue, CalculatePercentageChange)
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
        object grossProfitMargin,
        object businessActivityRatio,
        object financialResourceEfficiency,
        object ownFundsUtilization,
        object enterpriseProfitability,
        object laborProductivity,
        object fixedAssetTurnover,
        object receivablesTurnoverRevolutions,
        object receivablesTurnoverDays,
        object inventoryTurnoverRevolutions,
        object inventoryTurnoverDays,
        object operatingCycle,
        object currentAssetsTurnoverRevolutions,
        object currentAssetsTurnoverDays,
        object equityTurnover,
        object totalCapitalTurnover,
        object economicGrowthStability,
        object equityPaybackPeriod)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        double CalculatePercentageChange(double baseValue, double currentValue)
        {
            if (baseValue == 0 || double.IsNaN(baseValue) || double.IsInfinity(baseValue)) return 0;
            if (double.IsNaN(currentValue) || double.IsInfinity(currentValue)) return 0;
            var result = ((currentValue - baseValue) / baseValue) * 100;
            if (double.IsNaN(result) || double.IsInfinity(result)) return 0;
            return result;
        }

        var data = new BusinessActivityData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            GrossProfitMargin = CreateBusinessActivityMetric(grossProfitMargin, SafeValue, CalculatePercentageChange),
            BusinessActivityRatio = CreateBusinessActivityMetric(businessActivityRatio, SafeValue, CalculatePercentageChange),
            FinancialResourceEfficiency = CreateBusinessActivityMetric(financialResourceEfficiency, SafeValue, CalculatePercentageChange),
            OwnFundsUtilization = CreateBusinessActivityMetric(ownFundsUtilization, SafeValue, CalculatePercentageChange),
            EnterpriseProfitability = CreateBusinessActivityMetric(enterpriseProfitability, SafeValue, CalculatePercentageChange),
            LaborProductivity = CreateBusinessActivityMetric(laborProductivity, SafeValue, CalculatePercentageChange),
            FixedAssetTurnover = CreateBusinessActivityMetric(fixedAssetTurnover, SafeValue, CalculatePercentageChange),
            ReceivablesTurnover = CreateTurnoverMetric(receivablesTurnoverRevolutions, receivablesTurnoverDays, SafeValue),
            InventoryTurnover = CreateTurnoverMetric(inventoryTurnoverRevolutions, inventoryTurnoverDays, SafeValue),
            OperatingCycle = CreateBusinessActivityMetric(operatingCycle, SafeValue, CalculatePercentageChange),
            CurrentAssetsTurnover = CreateTurnoverMetric(currentAssetsTurnoverRevolutions, currentAssetsTurnoverDays, SafeValue),
            EquityTurnover = CreateBusinessActivityMetric(equityTurnover, SafeValue, CalculatePercentageChange),
            TotalCapitalTurnover = CreateBusinessActivityMetric(totalCapitalTurnover, SafeValue, CalculatePercentageChange),
            EconomicGrowthStability = CreateBusinessActivityMetric(economicGrowthStability, SafeValue, CalculatePercentageChange),
            EquityPaybackPeriod = CreateBusinessActivityMetric(equityPaybackPeriod, SafeValue, CalculatePercentageChange)
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
        object netRevenue,
        object avgCostIntangibles,
        object intangibleTurnover,
        object capitalIntensity)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        double CalculatePercentageChange(double baseValue, double currentValue)
        {
            if (baseValue == 0 || double.IsNaN(baseValue) || double.IsInfinity(baseValue)) return 0;
            if (double.IsNaN(currentValue) || double.IsInfinity(currentValue)) return 0;
            var result = ((currentValue - baseValue) / baseValue) * 100;
            if (double.IsNaN(result) || double.IsInfinity(result)) return 0;
            return result;
        }

        var data = new IntangibleAssetsData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            NetRevenueFromSales = CreateIntangibleAssetMetric(netRevenue, SafeValue, CalculatePercentageChange),
            AverageCostOfIntangibleAssets = CreateIntangibleAssetMetric(avgCostIntangibles, SafeValue, CalculatePercentageChange),
            IntangibleAssetTurnover_UAH = CreateIntangibleAssetMetric(intangibleTurnover, SafeValue, CalculatePercentageChange),
            CapitalIntensityOfProduction_UAH = CreateIntangibleAssetMetric(capitalIntensity, SafeValue, CalculatePercentageChange)
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
        object totalReturnOnAssets,
        object independenceRatio,
        object financialLeverage,
        object financialStability,
        object maneuverability,
        object borrowedCapitalConcentration,
        object longTermInvestmentStructure,
        object longTermBorrowing,
        object capitalStructureRatio,
        object debtToEquity,
        object ownFundsInInventories,
        object mobileToImmobilized,
        object totalCoverage)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        double CalculatePercentageChange(double baseValue, double currentValue)
        {
            if (baseValue == 0 || double.IsNaN(baseValue) || double.IsInfinity(baseValue)) return 0;
            if (double.IsNaN(currentValue) || double.IsInfinity(currentValue)) return 0;
            var result = ((currentValue - baseValue) / baseValue) * 100;
            if (double.IsNaN(result) || double.IsInfinity(result)) return 0;
            return result;
        }

        var data = new FinancialStabilityIndicatorsData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            TotalReturnOnAssets = CreateStabilityIndicatorMetric(totalReturnOnAssets, SafeValue, CalculatePercentageChange),
            IndependenceRatio = CreateStabilityIndicatorMetric(independenceRatio, SafeValue, CalculatePercentageChange),
            FinancialLeverageRatio = CreateStabilityIndicatorMetric(financialLeverage, SafeValue, CalculatePercentageChange),
            FinancialStabilityRatio = CreateStabilityIndicatorMetric(financialStability, SafeValue, CalculatePercentageChange),
            ManeuverabilityRatio = CreateStabilityIndicatorMetric(maneuverability, SafeValue, CalculatePercentageChange),
            BorrowedCapitalConcentration = CreateStabilityIndicatorMetric(borrowedCapitalConcentration, SafeValue, CalculatePercentageChange),
            LongTermInvestmentStructure = CreateStabilityIndicatorMetric(longTermInvestmentStructure, SafeValue, CalculatePercentageChange),
            LongTermBorrowingRatio = CreateStabilityIndicatorMetric(longTermBorrowing, SafeValue, CalculatePercentageChange),
            CapitalStructureRatio = CreateStabilityIndicatorMetric(capitalStructureRatio, SafeValue, CalculatePercentageChange),
            DebtToEquityRatio = CreateStabilityIndicatorMetric(debtToEquity, SafeValue, CalculatePercentageChange),
            OwnFundsInInventories = CreateStabilityIndicatorMetric(ownFundsInInventories, SafeValue, CalculatePercentageChange),
            MobileToImmobilizedRatio = CreateStabilityIndicatorMetric(mobileToImmobilized, SafeValue, CalculatePercentageChange),
            TotalCoverageRatio = CreateStabilityIndicatorMetric(totalCoverage, SafeValue, CalculatePercentageChange)
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
        object a1p1Base,
        object a2p2Base,
        object a3p3Base,
        object a4p4Base,
        object a1p1Current,
        object a2p2Current,
        object a3p3Current,
        object a4p4Current)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        bool IsLiquidBegin(object a1, object a2, object a3, object a4)
        {
            var a1ABegin = SafeValue(GetDoubleProperty(a1, "ABegin"));
            var a1PBegin = SafeValue(GetDoubleProperty(a1, "PBegin"));
            var a2ABegin = SafeValue(GetDoubleProperty(a2, "ABegin"));
            var a2PBegin = SafeValue(GetDoubleProperty(a2, "PBegin"));
            var a3ABegin = SafeValue(GetDoubleProperty(a3, "ABegin"));
            var a3PBegin = SafeValue(GetDoubleProperty(a3, "PBegin"));
            var a4ABegin = SafeValue(GetDoubleProperty(a4, "ABegin"));
            var a4PBegin = SafeValue(GetDoubleProperty(a4, "PBegin"));

            return a1ABegin >= a1PBegin && a2ABegin >= a2PBegin && a3ABegin >= a3PBegin && a4ABegin <= a4PBegin;
        }

        bool IsLiquidEnd(object a1, object a2, object a3, object a4)
        {
            var a1AEnd = SafeValue(GetDoubleProperty(a1, "AEnd"));
            var a1PEnd = SafeValue(GetDoubleProperty(a1, "PEnd"));
            var a2AEnd = SafeValue(GetDoubleProperty(a2, "AEnd"));
            var a2PEnd = SafeValue(GetDoubleProperty(a2, "PEnd"));
            var a3AEnd = SafeValue(GetDoubleProperty(a3, "AEnd"));
            var a3PEnd = SafeValue(GetDoubleProperty(a3, "PEnd"));
            var a4AEnd = SafeValue(GetDoubleProperty(a4, "AEnd"));
            var a4PEnd = SafeValue(GetDoubleProperty(a4, "PEnd"));

            return a1AEnd >= a1PEnd && a2AEnd >= a2PEnd && a3AEnd >= a3PEnd && a4AEnd <= a4PEnd;
        }

        var data = new LiquidityIndicatorsData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            BaseYear_Assessment = new LiquidityPeriodData
            {
                BeginOfYear = CreateLiquidityCondition(a1p1Base, a2p2Base, a3p3Base, a4p4Base, true, SafeValue, IsLiquidBegin),
                EndOfYear = CreateLiquidityCondition(a1p1Base, a2p2Base, a3p3Base, a4p4Base, false, SafeValue, IsLiquidEnd)
            },
            CurrentYear_Assessment = new LiquidityPeriodData
            {
                BeginOfYear = CreateLiquidityCondition(a1p1Current, a2p2Current, a3p3Current, a4p4Current, true, SafeValue, IsLiquidBegin),
                EndOfYear = CreateLiquidityCondition(a1p1Current, a2p2Current, a3p3Current, a4p4Current, false, SafeValue, IsLiquidEnd)
            }
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.LiquidityIndicatorsData);
    }

    /// <summary>
    /// Serialize general financial stability indicators for AI analysis
    /// </summary>
    public static string SerializeGeneralFinancialStability(
        string companyName,
        int baseYear,
        int currentYear,
        object ownWorkingCapital,
        object ownPlusLongTerm,
        object totalAvailable,
        object stocks,
        object deficitOwnCapital,
        object deficitOwnPlusLongTerm,
        object deficitTotalSources)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        var data = new GeneralFinancialStabilityData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            OwnWorkingCapital = CreateStabilitySource(ownWorkingCapital, SafeValue),
            OwnPlusLongTerm = CreateStabilitySource(ownPlusLongTerm, SafeValue),
            TotalAvailable = CreateStabilitySource(totalAvailable, SafeValue),
            Stocks_Inventory = CreateStabilitySource(stocks, SafeValue),
            Deficit_OwnCapital = CreateStabilitySource(deficitOwnCapital, SafeValue),
            Deficit_OwnPlusLongTerm = CreateStabilitySource(deficitOwnPlusLongTerm, SafeValue),
            Deficit_TotalSources = CreateStabilitySource(deficitTotalSources, SafeValue)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.GeneralFinancialStabilityData);
    }

    /// <summary>
    /// Serialize sources of capital formation for AI analysis
    /// </summary>
    public static string SerializeSourcesOfCapital(
        string companyName,
        int baseYear,
        int currentYear,
        object totalCapital,
        object equity,
        object ownCurrentAssets,
        object borrowedCapital,
        object longTermLiabilities,
        object shortTermLoans,
        object accountsPayable,
        object otherCurrentLiabilities)
    {
        double SafeValue(double value) => double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;

        var data = new SourcesOfCapitalData
        {
            CompanyName = companyName,
            BaseYear = baseYear,
            CurrentYear = currentYear,
            TotalCapital = CreateCapitalSourceMetric(totalCapital, SafeValue),
            Equity = CreateCapitalSourceMetric(equity, SafeValue),
            OwnCurrentAssets = CreateCapitalComponent(ownCurrentAssets, SafeValue),
            BorrowedCapital = CreateCapitalSourceMetric(borrowedCapital, SafeValue),
            LongTermLiabilities = CreateCapitalComponent(longTermLiabilities, SafeValue),
            ShortTermLoans = CreateCapitalComponent(shortTermLoans, SafeValue),
            AccountsPayable = CreateCapitalComponent(accountsPayable, SafeValue),
            OtherCurrentLiabilities = CreateCapitalComponent(otherCurrentLiabilities, SafeValue)
        };

        return JsonSerializer.Serialize(data, AFSJsonSerializerContext.Default.SourcesOfCapitalData);
    }

    private static CapitalSourceMetricData CreateCapitalSourceMetric(object obj, Func<double, double> safeValue)
    {
        var baseObj = obj.GetType().GetProperty("Base")?.GetValue(obj);
        var currentObj = obj.GetType().GetProperty("Current")?.GetValue(obj);
        var basePercentObj = obj.GetType().GetProperty("InPercentageOfAssetsBase")?.GetValue(obj);
        var currentPercentObj = obj.GetType().GetProperty("InPercentageOfAssetsCurrent")?.GetValue(obj);
        
        var baseBegin = baseObj != null ? safeValue(GetDoubleProperty(baseObj, "BeginningOfyear")) : 0;
        var baseEnd = baseObj != null ? safeValue(GetDoubleProperty(baseObj, "EndOfYear")) : 0;
        var currentBegin = currentObj != null ? safeValue(GetDoubleProperty(currentObj, "BeginningOfyear")) : 0;
        var currentEnd = currentObj != null ? safeValue(GetDoubleProperty(currentObj, "EndOfYear")) : 0;
        
        return new CapitalSourceMetricData
        {
            Base_Begin = baseBegin,
            Base_End = baseEnd,
            Base_Change = baseEnd - baseBegin,
            Base_PercentBegin = basePercentObj != null ? safeValue(GetDoubleProperty(basePercentObj, "BeginningOfyear")) : 0,
            Base_PercentEnd = basePercentObj != null ? safeValue(GetDoubleProperty(basePercentObj, "EndOfYear")) : 0,
            Current_Begin = currentBegin,
            Current_End = currentEnd,
            Current_Change = currentEnd - currentBegin,
            Current_PercentBegin = currentPercentObj != null ? safeValue(GetDoubleProperty(currentPercentObj, "BeginningOfyear")) : 0,
            Current_PercentEnd = currentPercentObj != null ? safeValue(GetDoubleProperty(currentPercentObj, "EndOfYear")) : 0
        };
    }

    private static CapitalComponentData CreateCapitalComponent(object obj, Func<double, double> safeValue)
    {
        var baseObj = obj.GetType().GetProperty("Base")?.GetValue(obj);
        var currentObj = obj.GetType().GetProperty("Current")?.GetValue(obj);
        
        // Get percent properties - they vary by component type
        var basePercentProp = obj.GetType().GetProperty("InPercentageOfEquityBase") 
                           ?? obj.GetType().GetProperty("InPercentageOfBorrowedCapitalBase");
        var currentPercentProp = obj.GetType().GetProperty("InPercentageOfEquityCurrent") 
                              ?? obj.GetType().GetProperty("InPercentageOfBorrowedCapitalCurrent");
        
        var basePercentObj = basePercentProp?.GetValue(obj);
        var currentPercentObj = currentPercentProp?.GetValue(obj);
        
        return new CapitalComponentData
        {
            Base_End = baseObj != null ? safeValue(GetDoubleProperty(baseObj, "EndOfYear")) : 0,
            Base_Percent = basePercentObj != null ? safeValue(GetDoubleProperty(basePercentObj, "EndOfYear")) : 0,
            Current_End = currentObj != null ? safeValue(GetDoubleProperty(currentObj, "EndOfYear")) : 0,
            Current_Percent = currentPercentObj != null ? safeValue(GetDoubleProperty(currentPercentObj, "EndOfYear")) : 0
        };
    }

    private static StabilitySourceData CreateStabilitySource(object obj, Func<double, double> safeValue)
    {
        var baseObj = obj.GetType().GetProperty("Base")?.GetValue(obj);
        var currentObj = obj.GetType().GetProperty("Current")?.GetValue(obj);
        
        return new StabilitySourceData
        {
            Base_Begin = baseObj != null ? safeValue(GetDoubleProperty(baseObj, "BeginningOfyear")) : 0,
            Base_End = baseObj != null ? safeValue(GetDoubleProperty(baseObj, "EndOfYear")) : 0,
            Current_Begin = currentObj != null ? safeValue(GetDoubleProperty(currentObj, "BeginningOfyear")) : 0,
            Current_End = currentObj != null ? safeValue(GetDoubleProperty(currentObj, "EndOfYear")) : 0
        };
    }

    private static LiquidityConditionData CreateLiquidityCondition(
        object a1, object a2, object a3, object a4,
        bool isBegin,
        Func<double, double> safeValue,
        Func<object, object, object, object, bool> isLiquid)
    {
        var suffix = isBegin ? "Begin" : "End";

        return new LiquidityConditionData
        {
            IsLiquid = isLiquid(a1, a2, a3, a4),
            A1_MostLiquid = safeValue(GetDoubleProperty(a1, $"A{suffix}")),
            P1_MostUrgent = safeValue(GetDoubleProperty(a1, $"P{suffix}")),
            Surplus_A1P1 = safeValue(GetDoubleProperty(a1, $"PaymentBalance{suffix}")),
            A2_QuickLiquid = safeValue(GetDoubleProperty(a2, $"A{suffix}")),
            P2_ShortTerm = safeValue(GetDoubleProperty(a2, $"P{suffix}")),
            Surplus_A2P2 = safeValue(GetDoubleProperty(a2, $"PaymentBalance{suffix}")),
            A3_SlowLiquid = safeValue(GetDoubleProperty(a3, $"A{suffix}")),
            P3_LongTerm = safeValue(GetDoubleProperty(a3, $"P{suffix}")),
            Surplus_A3P3 = safeValue(GetDoubleProperty(a3, $"PaymentBalance{suffix}")),
            A4_HardToSell = safeValue(GetDoubleProperty(a4, $"A{suffix}")),
            P4_Permanent = safeValue(GetDoubleProperty(a4, $"P{suffix}")),
            Surplus_A4P4 = safeValue(GetDoubleProperty(a4, $"PaymentBalance{suffix}"))
        };
    }

    private static StabilityIndicatorMetricData CreateStabilityIndicatorMetric(
        object obj,
        Func<double, double> safeValue,
        Func<double, double, double> calcPercentChange)
    {
        var baseYear = safeValue(GetDoubleProperty(obj, "BaseYear"));
        var currentYear = safeValue(GetDoubleProperty(obj, "CurrentYear"));

        return new StabilityIndicatorMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Change = calcPercentChange(baseYear, currentYear)
        };
    }

    private static BusinessActivityMetricData CreateBusinessActivityMetric(
        object obj,
        Func<double, double> safeValue,
        Func<double, double, double> calcPercentChange)
    {
        var baseYear = safeValue(GetDoubleProperty(obj, "BaseYear"));
        var currentYear = safeValue(GetDoubleProperty(obj, "CurrentYear"));

        return new BusinessActivityMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Change = calcPercentChange(baseYear, currentYear)
        };
    }

    private static IntangibleAssetMetricData CreateIntangibleAssetMetric(
        object obj,
        Func<double, double> safeValue,
        Func<double, double, double> calcPercentChange)
    {
        var baseYear = safeValue(GetDoubleProperty(obj, "BaseYear"));
        var currentYear = safeValue(GetDoubleProperty(obj, "CurrentYear"));

        return new IntangibleAssetMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Deviations = safeValue(GetDoubleProperty(obj, "Deviations")),
            PercentageChange = calcPercentChange(baseYear, currentYear)
        };
    }

    private static TurnoverMetricData CreateTurnoverMetric(
        object revolutionsObj,
        object daysObj,
        Func<double, double> safeValue)
    {
        return new TurnoverMetricData
        {
            Revolutions_Base = safeValue(GetDoubleProperty(revolutionsObj, "BaseYear")),
            Revolutions_Current = safeValue(GetDoubleProperty(revolutionsObj, "CurrentYear")),
            Days_Base = safeValue(GetDoubleProperty(daysObj, "BaseYear")),
            Days_Current = safeValue(GetDoubleProperty(daysObj, "CurrentYear"))
        };
    }

    private static FactorMetricData CreateFactorMetric(
        object obj,
        Func<double, double> safeValue,
        Func<double, double, double> calcPercentChange)
    {
        var baseYear = safeValue(GetDoubleProperty(obj, "BaseYear"));
        var currentYear = safeValue(GetDoubleProperty(obj, "CurrentYear"));

        return new FactorMetricData
        {
            BaseYear = baseYear,
            CurrentYear = currentYear,
            Deviations = safeValue(GetDoubleProperty(obj, "Deviations")),
            PercentageChange = calcPercentChange(baseYear, currentYear)
        };
    }

    private static ReceivablePayableCategoryData CreateReceivablePayableCategory(object obj, Func<double, double> safeValue)
    {
        return new ReceivablePayableCategoryData
        {
            Receivable_Base = safeValue(GetDoubleProperty(obj, "ReceivableBase")),
            Receivable_Current = safeValue(GetDoubleProperty(obj, "ReceivableCurrent")),
            Payable_Base = safeValue(GetDoubleProperty(obj, "PayableBase")),
            Payable_Current = safeValue(GetDoubleProperty(obj, "PayableCurrent")),
            ExcessReceivable_Base = safeValue(GetDoubleProperty(obj, "ExceedingReceivableBase")),
            ExcessReceivable_Current = safeValue(GetDoubleProperty(obj, "ExceedingReceivableCurrent")),
            ExcessPayable_Base = safeValue(GetDoubleProperty(obj, "ExceedingPayableBase")),
            ExcessPayable_Current = safeValue(GetDoubleProperty(obj, "ExceedingPayableCurrent"))
        };
    }

    private static SolvencyRatioItem CreateSolvencyRatioItem(object obj, Func<double, double> safeValue)
    {
        var baseBegin = safeValue(GetDoubleProperty(obj, "BaseBegin"));
        var baseEnd = safeValue(GetDoubleProperty(obj, "BaseEnd"));
        var currentBegin = safeValue(GetDoubleProperty(obj, "CurrentBegin"));
        var currentEnd = safeValue(GetDoubleProperty(obj, "CurrentEnd"));

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

    private static SolvencyRatioSimpleItem CreateSolvencyRatioSimpleItem(object obj, Func<double, double> safeValue)
    {
        var baseEnd = safeValue(GetDoubleProperty(obj, "BaseEnd"));
        var currentEnd = safeValue(GetDoubleProperty(obj, "CurrentEnd"));

        return new SolvencyRatioSimpleItem
        {
            Base_End = baseEnd,
            Current_End = currentEnd,
            Deviation = currentEnd - baseEnd
        };
    }

    private static string GetPropertyValue(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        return property?.GetValue(obj)?.ToString() ?? string.Empty;
    }

    private static double GetDoubleProperty(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        var value = property?.GetValue(obj);

        if (value is double d)
            return d;
        if (value != null && double.TryParse(value.ToString(), out var result))
            return result;

        return 0;
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
