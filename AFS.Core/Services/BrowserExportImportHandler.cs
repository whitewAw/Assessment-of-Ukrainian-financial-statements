﻿using AFS.Core.Interfaces;
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
        private JsInterop jsInterop;
        private AFSModel model;
        private AFSConstraints constraints;
        private ILogger<BrowserExportImportHandler> logger;

        public BrowserExportImportHandler(JsInterop jsInterop, AFSModel model, AFSConstraints constraints, ILoggerFactory loggerFactory)
        {
            this.jsInterop = jsInterop;
            this.model = model;
            this.constraints = constraints;
            logger = loggerFactory.CreateLogger<BrowserExportImportHandler>();
        }

        public async Task ExportAsync(AFSModel model)
        {
            var modelJson = JsonSerializer.Serialize(model, AFSJsonSerializerContext.Default.AFSModel);
            var randomBinaryData = Encoding.UTF8.GetBytes(modelJson);
            using var fileStream = new MemoryStream(randomBinaryData);
            var fileName = $"{model.CompanyName}_{model.BaseYear}_{model.CurrentYear}{constraints.FileExtension}";
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await jsInterop.ExportToFile(fileName, streamRef);
        }

        public async Task<AFSModel?> ImportAsync(Stream inputStream)
        {
            try
            {
                return await JsonSerializer.DeserializeAsync(inputStream, AFSJsonSerializerContext.Default.AFSModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public void InitializeModel(AFSModel? model)
        {
            if (model != null)
            {
                this.model.Init(model);
            }
        }
    }
}