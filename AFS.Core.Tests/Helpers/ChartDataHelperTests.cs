using AFS.Core.Helpers;
using AFS.Core.Models;
using Xunit;

namespace AFS.Core.Tests.Helpers;

public class ChartDataHelperTests
{
    private static readonly string[] ExpectedSortedOrder = ["B", "C", "A"];

    [Fact]
    public void AddIfValidAddsItemWhenValueIsNonZero()
    {
        var items = new List<ChartDataItem>();
        ChartDataHelper.AddIfValid(items, "A", 10);
        Assert.Single(items);
        Assert.Equal("A", items[0].Item);
        Assert.Equal(10, items[0].Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0.0)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void AddIfValidSkipsZeroOrInvalid(double? value)
    {
        var items = new List<ChartDataItem>();
        ChartDataHelper.AddIfValid(items, "A", value);
        Assert.Empty(items);
    }

    [Fact]
    public void SortDescendingOrdersByValueHighToLow()
    {
        var input = new[]
        {
            new ChartDataItem { Item = "A", Value = 1 },
            new ChartDataItem { Item = "B", Value = 5 },
            new ChartDataItem { Item = "C", Value = 3 },
        };

        var sorted = ChartDataHelper.SortDescending(input);

        Assert.Equal(ExpectedSortedOrder, sorted.Select(i => i.Item).ToArray());
    }

    [Fact]
    public void CalculatePercentageReturnsZeroWhenTotalIsZero()
    {
        Assert.Equal(0, ChartDataHelper.CalculatePercentage(10, 0));
    }

    [Fact]
    public void CalculatePercentageReturnsRatioTimesHundred()
    {
        Assert.Equal(25, ChartDataHelper.CalculatePercentage(25, 100));
    }

    [Fact]
    public void CalculatePercentageReturnsZeroWhenTotalIsNull()
    {
        Assert.Equal(0, ChartDataHelper.CalculatePercentage(10, null));
    }
}
