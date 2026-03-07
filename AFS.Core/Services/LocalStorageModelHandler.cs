using AFS.Core.Interfaces;
using AFS.Core.Json;
using AFS.Core.Models;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace AFS.Core.Services
{
    public class LocalStorageModelHandler : IModelStorageHandler
    {
        private readonly ILocalStorageService storage;
        private readonly AFSModel model;
        private readonly ILogger<LocalStorageModelHandler> logger;
        private readonly HttpClient http;

        public LocalStorageModelHandler(ILocalStorageService storage, AFSModel model, ILoggerFactory loggerFactory, HttpClient http)
        {
            ArgumentNullException.ThrowIfNull(storage);
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(loggerFactory);
            ArgumentNullException.ThrowIfNull(http);

            this.storage = storage;
            this.model = model;
            logger = loggerFactory.CreateLogger<LocalStorageModelHandler>();
            this.http = http;
        }

        public async Task InitializeModelAsync()
        {
            var readModel = await ReadModelAsync();
            if (readModel != null)
            {
                model.Init(readModel);
            }
        }

        public async Task<AFSModel?> ReadModelAsync()
        {
            var jsonModel = await storage.GetItemAsStringAsync(AFSConstraints.ModelJsonName);
            if (jsonModel != null)
            {
                try
                {
                    return JsonSerializer.Deserialize(jsonModel, AFSJsonSerializerContext.Default.AFSModel);
                }
                catch (JsonException ex)
                {
                    logger.LogError("JSON deserialization failed: {Message}", ex.Message);
                }
            }
            try
            {
                return await http.GetFromJsonAsync("PJSC_AZOVSTAL_IRON_2019_2020.json", AFSJsonSerializerContext.Default.AFSModel);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError("HTTP request failed: {Message}", ex.Message);
            }
            catch (JsonException ex)
            {
                logger.LogError("JSON deserialization failed: {Message}", ex.Message);
            }
            return null;
        }

        public async Task WriteModelAsync(AFSModel model)
        {
            string jsonString = JsonSerializer.Serialize(model, AFSJsonSerializerContext.Default.AFSModel);
            await storage.SetItemAsStringAsync(AFSConstraints.ModelJsonName, jsonString);
        }
    }
}
