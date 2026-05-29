using AFS.Core.Models;
using Xunit;

namespace AFS.Core.Tests.Models;

public class AfsConstraintsTests
{
    [Theory]
    [InlineData(0.0, true)]
    [InlineData(1e-11, true)]
    [InlineData(double.NaN, true)]
    [InlineData(double.PositiveInfinity, true)]
    [InlineData(double.NegativeInfinity, true)]
    [InlineData(0.5, false)]
    [InlineData(-1.0, false)]
    public void IsZeroOrInvalidReturnsExpected(double value, bool expected)
    {
        Assert.Equal(expected, AfsConstraints.IsZeroOrInvalid(value));
    }

    [Fact]
    public void SafeDivideReturnsQuotientWhenDenominatorIsNonZero()
    {
        Assert.Equal(2.5, AfsConstraints.SafeDivide(5, 2));
    }

    [Fact]
    public void SafeDivideReturnsDefaultWhenDenominatorIsZero()
    {
        Assert.Equal(0, AfsConstraints.SafeDivide(5, 0));
    }

    [Fact]
    public void SafeDivideReturnsCustomDefaultWhenDenominatorIsInvalid()
    {
        Assert.Equal(-1, AfsConstraints.SafeDivide(1, double.NaN, defaultValue: -1));
    }

    [Fact]
    public void CalculateGrowthRateReturnsZeroWhenBothValuesAreZero()
    {
        Assert.Equal(0, AfsConstraints.CalculateGrowthRate(0, 0));
    }

    [Fact]
    public void CalculateGrowthRateReturnsHundredWhenBaseIsZeroAndCurrentIsNot()
    {
        Assert.Equal(100, AfsConstraints.CalculateGrowthRate(0, 42));
    }

    [Fact]
    public void CalculateGrowthRateReturnsPercentageChange()
    {
        Assert.Equal(50, AfsConstraints.CalculateGrowthRate(100, 150));
    }

    [Fact]
    public void RoundStatRoundsToOneDigitByDefault()
    {
        Assert.Equal(1.2, AfsConstraints.RoundStat(1.25));
    }
}
