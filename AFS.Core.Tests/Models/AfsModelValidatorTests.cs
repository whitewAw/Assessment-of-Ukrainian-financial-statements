using AFS.Core.Models;
using Xunit;

namespace AFS.Core.Tests.Models;

public class AfsModelValidatorTests
{
    [Fact]
    public void TryValidateAcceptsAnInitializedModel()
    {
        var model = new AfsModel();

        Assert.True(AfsModelValidator.TryValidate(model, out var error));
        Assert.Empty(error);
    }

    [Fact]
    public void TryValidateRejectsMissingFormSections()
    {
        var model = new AfsModel { F1Base = null! };

        Assert.False(AfsModelValidator.TryValidate(model, out var error));
        Assert.Contains("missing required sections", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TryValidateRejectsNonFiniteFinancialValues()
    {
        var model = new AfsModel();
        model.F1Base.F1001.Begin = double.NaN;

        Assert.False(AfsModelValidator.TryValidate(model, out var error));
        Assert.Contains("invalid numeric values", error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TryValidateRejectsYearsOutsideTheSupportedRange()
    {
        var model = new AfsModel { BaseYear = AfsConstraints.MinYear - 1 };

        Assert.False(AfsModelValidator.TryValidate(model, out var error));
        Assert.Contains("supported range", error, StringComparison.OrdinalIgnoreCase);
    }
}
