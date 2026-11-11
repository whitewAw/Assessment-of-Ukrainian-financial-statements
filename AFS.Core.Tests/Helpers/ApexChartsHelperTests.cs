using AFS.ComponentLibrary.Helpers;
using AFS.Core.Models;
using Xunit;

namespace AFS.Core.Tests.Helpers;

public class ApexChartsHelperTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PercentageChartsUseBuiltInFormatterWithoutJavaScriptEvaluation(bool donut)
    {
        var options = donut
            ? ApexChartsHelper.GetDonutChartOptions<ChartDataItem>()
            : ApexChartsHelper.GetPieChartOptions<ChartDataItem>();

        Assert.NotNull(options.DataLabels);
        Assert.Null(options.DataLabels.Formatter);
        Assert.Equal(["#fff"], options.DataLabels.Style.Colors);

        if (donut)
        {
            Assert.True(options.PlotOptions.Pie.Donut.Labels.Show);
            Assert.True(options.PlotOptions.Pie.Donut.Labels.Total.Show);
        }
    }
}
