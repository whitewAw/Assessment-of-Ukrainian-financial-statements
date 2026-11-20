using AFS.Core.Models;
using AFS.Core.Services;
using AFS.Core.Services.DataCalculations;
using System.Text.Json;
using Xunit;

namespace AFS.Core.Tests.Services;

public class FinancialBusinessRulesTests
{
    [Fact]
    public void FinancialContextSendsSignedResultsAndExcludesEquityFromLiabilities()
    {
        var model = new AfsModel();
        model.F2Base.F2000.Current = 100;
        model.F2Base.F2050.Current = 120;
        model.F2Base.F2130.Current = 5;
        model.F2Base.F2305.Current = 3;
        model.F2Current.F2000.Current = 200;
        model.F2Current.F2050.Current = 150;
        model.F2Current.F2300.Current = 10;
        model.F1Base.F1400.Begin = 80;
        model.F1Base.F1510.Begin = 15;
        model.F1Base.F1615.Begin = 5;
        model.F1Base.F1400.End = 90;
        model.F1Base.F1510.End = 25;
        model.F1Base.F1615.End = 10;
        model.F1Base.F1700.End = 3;

        using var document = JsonDocument.Parse(JsonSerializationHelper.SerializeFinancialContext(model));
        var income = document.RootElement.GetProperty("IncomeStatement");
        var loss = income.GetProperty("BaseYear");
        var profit = income.GetProperty("CurrentYear");
        Assert.Equal(-20, loss.GetProperty("GrossProfit").GetDouble());
        Assert.Equal(-25, loss.GetProperty("OperatingProfit").GetDouble());
        Assert.Equal(-22, loss.GetProperty("NetProfit").GetDouble());
        Assert.Equal(50, profit.GetProperty("GrossProfit").GetDouble());
        Assert.Equal(40, profit.GetProperty("NetProfit").GetDouble());
        var liabilities = document.RootElement.GetProperty("BalanceSheet").GetProperty("BaseYear").GetProperty("TotalLiabilities");
        Assert.Equal(20, liabilities.GetProperty("Beginning").GetDouble());
        Assert.Equal(38, liabilities.GetProperty("End").GetDouble());
    }

    [Fact]
    public void WorkingCapitalProfitabilityExcludesOtherComprehensiveIncome()
    {
        var model = new AfsModel();
        model.F1Base.F1165.Begin = model.F1Base.F1165.End = 50;
        model.F2Base.F2000.Current = 100;
        model.F2Base.F2050.Current = 110;
        model.F2Base.F2400.Current = 30;
        var result = new IndicatorsOfEfficiencyOfWorkingCapital(model);

        Assert.Equal(-10, result.NetProfit.BaseYear);
        Assert.Equal(-20, result.ProfitabilityOfCurrentAssets.BaseYear, 6);
        Assert.True(double.IsNaN(result.ProfitabilityOfCurrentAssets.CurrentYear));
    }

    [Theory]
    [InlineData(120, -0.2, 60)]
    [InlineData(100, 0, 72)]
    [InlineData(80, 0.2, 90)]
    public void BusinessActivityUsesSignedMarginAndBothPartsOfTheOperatingCycle(double cost, double margin, double inventoryDays)
    {
        var model = new AfsModel();
        model.F2Base.F2000.Current = 100;
        model.F2Base.F2050.Current = cost;
        model.F1Base.F1101.Begin = model.F1Base.F1101.End = 20;
        model.F1Base.F1125.Begin = model.F1Base.F1125.End = 10;
        var result = new IndicatorsOfBusinessActivity(model);

        Assert.Equal(margin, result.GrossProfitPerSoldProducts.BaseYear, 6);
        Assert.Equal(inventoryDays, result.TurnoverOfInventoriesDays.BaseYear, 6);
        Assert.Equal(36, result.ReceivablesTurnover.BaseYear, 6);
        Assert.Equal(36 + inventoryDays, result.OperatingCycleDuration.BaseYear, 6);
        Assert.True(double.IsNaN(result.GrossProfitPerSoldProducts.CurrentYear));
        Assert.True(double.IsNaN(result.GrossProfitPerSoldProducts.Deviations));
    }

    [Fact]
    public void OperatingCycleIsUnavailableIfEitherDurationIsUndefined()
    {
        var model = new AfsModel();
        model.F2Base.F2000.Current = 100;
        model.F1Base.F1101.Begin = model.F1Base.F1101.End = 20;
        model.F1Base.F1125.Begin = model.F1Base.F1125.End = 10;
        model.F2Current.F2050.Current = 100;
        model.F1Current.F1101.Begin = model.F1Current.F1101.End = 20;
        var result = new IndicatorsOfBusinessActivity(model);

        Assert.Equal(0, result.TurnoverOfInventoriesRevolutions.BaseYear);
        Assert.True(double.IsNaN(result.TurnoverOfInventoriesDays.BaseYear));
        Assert.Equal(72, result.TurnoverOfInventoriesDays.CurrentYear, 6);
        Assert.True(double.IsNaN(result.ReceivablesTurnover.CurrentYear));
        Assert.True(double.IsNaN(result.OperatingCycleDuration.BaseYear));
        Assert.True(double.IsNaN(result.OperatingCycleDuration.CurrentYear));

        using var json = JsonDocument.Parse(JsonSerializationHelper.SerializeBusinessActivity(
            "Company", model.BaseYear, model.CurrentYear, result.GrossProfitPerSoldProducts,
            result.CoefficientOfBusinessActivity, result.CoefficientOfEfficiencFinancialResources,
            result.CoefficientOfOwnFundsUtilization, result.ProfitabilityRatioOfEnterprise,
            result.ProductivityOfOneAverageEmployee, result.ReturnOnFixedAssets,
            result.RevolutionsFromReceivables, result.ReceivablesTurnover,
            result.TurnoverOfInventoriesRevolutions, result.TurnoverOfInventoriesDays,
            result.OperatingCycleDuration, result.NumberOfRevolutionsOfCurrentAssets,
            result.TurnoverOfWorkingCapital, result.TurnoverOfEquity, result.TurnoverOfTotalCapital,
            result.CoefficientOfStabilityOfEconomicGrowth, result.ReturnOnEquityPeriod));
        var inventory = json.RootElement.GetProperty("InventoryTurnover");
        Assert.Equal(0, inventory.GetProperty("Revolutions_Base").GetDouble());
        Assert.Equal(JsonValueKind.Null, inventory.GetProperty("Days_Base").ValueKind);
        Assert.Equal(JsonValueKind.Null, json.RootElement.GetProperty("OperatingCycle").GetProperty("CurrentYear").ValueKind);
    }

    [Fact]
    public void StabilityMarksOnlyTheMissingClosingPeriodAsUnavailable()
    {
        var model = new AfsModel();
        model.F1Base.F1165.Begin = 100;
        model.F1Current.F1165.End = 75;
        model.F1Current.F1615.End = 50;
        model.F1Current.F1400.End = 25;
        var result = new ClassificationOfTypesOfFinancialStability(model);
        var rows = new[] { result.AbsoluteFinancialStability, result.NormalFinancialStability, result.PreCrisisFinancialStability, result.CrisisFinancialStability };
        Assert.All(rows, row =>
        {
            Assert.Equal("—", row.CurrentBVal);
            Assert.Equal("—", row.ShortBVal);
            Assert.Equal("—", row.LongBVal);
        });
        Assert.Equal("+", result.AbsoluteFinancialStability.CurrentCVal);
        Assert.Equal("-", result.CrisisFinancialStability.CurrentCVal);
        var empty = new ClassificationOfTypesOfFinancialStability(new AfsModel());
        Assert.Equal("—", empty.AbsoluteFinancialStability.CurrentCVal);

        using var document = JsonDocument.Parse(JsonSerializationHelper.SerializeStabilityClassification(
            "Company", model.BaseYear, model.CurrentYear, rows[0], rows[1], rows[2], rows[3]));
        var absolute = document.RootElement.GetProperty("AbsoluteStability");
        Assert.Equal(JsonValueKind.Null, absolute.GetProperty("Base_Current").ValueKind);
        Assert.True(absolute.GetProperty("Current_Current").GetBoolean());
    }
}
