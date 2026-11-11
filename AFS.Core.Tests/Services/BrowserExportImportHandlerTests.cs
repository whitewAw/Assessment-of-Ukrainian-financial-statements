using AFS.Core.Json;
using AFS.Core.Models;
using AFS.Core.Services;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;
using Xunit;

namespace AFS.Core.Tests.Services;

public class BrowserExportImportHandlerTests
{
    private static BrowserExportImportHandler CreateHandler() =>
        new(null!, new AfsModel(), new AfsConstraints(), NullLoggerFactory.Instance);

    [Fact]
    public async Task ImportAsyncRejectsInvalidJson()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{"));

        var result = await CreateHandler().ImportAsync(stream);

        Assert.Null(result);
    }

    [Fact]
    public async Task ImportAsyncRejectsModelsWithMissingSections()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"F1Base\":null}"));

        var result = await CreateHandler().ImportAsync(stream);

        Assert.Null(result);
    }

    [Fact]
    public async Task ImportAsyncAcceptsAValidModel()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(new AfsModel(), AfsJsonSerializerContext.Default.AfsModel);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

        var result = await CreateHandler().ImportAsync(stream);

        Assert.NotNull(result);
        Assert.Equal(AfsConstraints.MaxYear - 1, result.BaseYear);
    }
}
