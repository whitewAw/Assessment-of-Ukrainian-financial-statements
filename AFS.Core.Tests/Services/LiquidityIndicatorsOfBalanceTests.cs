using AFS.Core.Models;
using AFS.Core.Services.DataCalculations;
using Xunit;

namespace AFS.Core.Tests.Services;

public class LiquidityIndicatorsOfBalanceTests
{
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
    public void A1IsMostLiquidAssetsCashPlusCurrentInvestments()
    {
        var liquidity = new LiquidityIndicatorsOfBalance(CreateModel());

        Assert.Equal(150, liquidity.A1P1Bace.ABegin);
    }

    [Fact]
    public void P1IsCurrentLiabilitiesExcludingShortTermLoansAndPayables()
    {
        var liquidity = new LiquidityIndicatorsOfBalance(CreateModel());

        Assert.Equal(150, liquidity.A1P1Bace.PBegin);
    }

    [Fact]
    public void A2IsReceivables()
    {
        var liquidity = new LiquidityIndicatorsOfBalance(CreateModel());

        Assert.Equal(200, liquidity.A2P2Bace.ABegin);
    }

    [Fact]
    public void P2IsShortTermLoansAndPayables()
    {
        var liquidity = new LiquidityIndicatorsOfBalance(CreateModel());

        Assert.Equal(50, liquidity.A2P2Bace.PBegin);
    }

    [Fact]
    public void A3IsSlowMovingTangibleAssets()
    {
        var liquidity = new LiquidityIndicatorsOfBalance(CreateModel());

        Assert.Equal(300, liquidity.A3P3Bace.ABegin);
    }

    [Fact]
    public void TotalAssetsEqualSumOfGroups()
    {
        var liquidity = new LiquidityIndicatorsOfBalance(CreateModel());

        Assert.Equal(650, liquidity.TotalBace.ABegin);
    }

    [Fact]
    public void EmptyModelProducesZeroTotals()
    {
        var liquidity = new LiquidityIndicatorsOfBalance(new AfsModel());

        Assert.Equal(0, liquidity.TotalCurrent.AEnd);
    }
}
