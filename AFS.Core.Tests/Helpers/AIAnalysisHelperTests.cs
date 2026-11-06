using AFS.ComponentLibrary.Helpers;
using Xunit;

namespace AFS.Core.Tests.Helpers;

public class AIAnalysisHelperTests
{
    [Fact]
    public void FormatResponseEncodesHtml()
    {
        var result = AIAnalysisHelper.FormatResponse("<script>alert(1)</script>");

        Assert.Equal("&lt;script&gt;alert(1)&lt;/script&gt;", result);
    }

    [Fact]
    public void FormatResponseWrapsBoldPairsInStrongTags()
    {
        var result = AIAnalysisHelper.FormatResponse("a **b** c **d**");

        Assert.Equal("a <strong>b</strong> c <strong>d</strong>", result);
    }

    [Fact]
    public void FormatResponseConvertsLineStartDashToBullet()
    {
        var result = AIAnalysisHelper.FormatResponse("- one\n- two");

        Assert.Equal("\u2022 one<br/>\u2022 two", result);
    }

    [Fact]
    public void FormatResponseKeepsDashInsideText()
    {
        var result = AIAnalysisHelper.FormatResponse("profit - loss");

        Assert.Equal("profit - loss", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void FormatResponseReturnsEmptyForBlankInput(string? input)
    {
        Assert.Equal(string.Empty, AIAnalysisHelper.FormatResponse(input));
    }
}
