using AFS.Core.Interfaces;
using AFS.Core.Json;
using AFS.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace AFS.Core.Services
{
    public class BrowserExportImportHandler : IModelExportImportHandler
    {
        private readonly JsInterop jsInterop;
        private readonly AfsModel model;
        private readonly AfsConstraints constraints;
        private readonly ILogger<BrowserExportImportHandler> logger;

        public BrowserExportImportHandler(JsInterop jsInterop, AfsModel model, AfsConstraints constraints, ILoggerFactory loggerFactory)
        {
            this.jsInterop = jsInterop;
            this.model = model;
            this.constraints = constraints;
            logger = loggerFactory.CreateLogger<BrowserExportImportHandler>();
        }

        public async Task ExportAsync(AfsModel model)
        {
            var modelJson = JsonSerializer.Serialize(model, AfsJsonSerializerContext.Default.AfsModel);
            var randomBinaryData = Encoding.UTF8.GetBytes(modelJson);
            using var fileStream = new MemoryStream(randomBinaryData);
            var fileName = $"{model.CompanyName}_{model.BaseYear}_{model.CurrentYear}{constraints.FileExtension}";
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await jsInterop.ExportToFileAsync(fileName, streamRef);
        }

        public async Task<AfsModel?> ImportAsync(Stream inputStream)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync(inputStream, AfsJsonSerializerContext.Default.AfsModel);
            }
            catch (Exception ex)
            {
                Log.ImportFailed(logger, ex.Message);
            }
            return null;
        }

        public void InitializeModel(AfsModel? model)
        {
            if (model != null)
            {
                this.model.Init(model);
            }
        }
    }
}
