using AFS.Core.Models;
using AFS.Core.Services.DataCalculations;
using Xunit;

namespace AFS.Core.Tests.Services;

public class SolvencyRatiosTests
{
    private static readonly string?[] ExpectedNumbers = ["1.", "2.", "3.", "4.", "5.", "6."];

    // Base-year balance (begin of period):
    //   Cash 1160 = 100, current financial investments 1165 = 50   -> money = 150
    //   Receivables 1125 = 200                                     -> A2 = 200
    //   Inventory 1101 = 300                                       -> A3 = 300
    //   Payables 1600 = 30, 1610 = 20, 1615 = 150                  -> current liabilities 1695 = 200
    private static AfsModel CreateModel()
    {
        var model = new AfsModel();
        model.F1Base.F1160.Begin = 100;
        model.F1Base.F1165.Begin = 50;
        model.F1Base.F1125.Begin = 200;
        model.F1Base.F1101.Begin = 300;
        model.F1Base.F1600.Begin = 30;
        model.F1Base.F1610.Begin = 20;
        model.F1Base.F1615.Begin = 150;
        return model;
    }

    [Fact]
    public void AbsoluteLiquidityRatioDividesMoneyByCurrentLiabilities()
    {
        var ratios = new SolvencyRatios(CreateModel());

        Assert.Equal(0.75, ratios.AbsoluteLiquidityRatio.BaseBegin, 6);
    }

    [Fact]
    public void IntermediateCoverageRatioDividesMoneyAndReceivablesByCurrentLiabilities()
    {
        var ratios = new SolvencyRatios(CreateModel());

        Assert.Equal(1.75, ratios.IntermediateCoverageRatio.BaseBegin, 6);
    }

    [Fact]
    public void CurrentLiquidityFactorDividesCurrentAssetsByCurrentLiabilities()
    {
        var ratios = new SolvencyRatios(CreateModel());

        Assert.Equal(3.25, ratios.CurrentLiquidityFactor.BaseBegin, 6);
    }

    [Fact]
    public void OverallLiquidityRatioUsesWeightedAssetAndLiabilityGroups()
    {
        var ratios = new SolvencyRatios(CreateModel());

        // (A1 150 + 0.5*A2 200 + 0.3*A3 300) / (P1 150 + 0.5*P2 50 + 0.3*P3 0) = 340 / 175
        Assert.Equal(340.0 / 175.0, ratios.OverallLiquidityRatio.BaseBegin, 6);
    }

    [Fact]
    public void RatiosAreNumberedInDisplayOrder()
    {
        var ratios = new SolvencyRatios(CreateModel());

        var numbers = new string?[]
        {
            ratios.OverallLiquidityRatio.Number, ratios.AbsoluteLiquidityRatio.Number,
            ratios.IntermediateCoverageRatio.Number, ratios.CurrentLiquidityFactor.Number,
            ratios.SolvencyRecoveryRatio.Number, ratios.SolvencyLossRatio.Number,
        };

        Assert.Equal(ExpectedNumbers, numbers);
    }
}
