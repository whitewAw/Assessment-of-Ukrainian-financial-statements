using AFS.Core.Interfaces;
using AFS.Core.Json;
using AFS.Core.Models;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AFS.Core.Services
{
    /// <summary>
    /// Handles model persistence to browser local storage.
    /// Sealed because it's a DI service not designed for inheritance.
    /// </summary>
    public sealed class LocalStorageModelHandler : IModelStorageHandler
    {
        private readonly ILocalStorageService storage;
        private readonly AfsModel model;
        private readonly ILogger<LocalStorageModelHandler> logger;
        private readonly HttpClient http;

        public LocalStorageModelHandler(ILocalStorageService storage, AfsModel model, ILoggerFactory loggerFactory, HttpClient http)
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
            var readModel = await ReadModelAsync().ConfigureAwait(false);
            if (readModel != null)
            {
                model.Init(readModel);
            }
        }

        public async Task<AfsModel?> ReadModelAsync()
        {
            var jsonModel = await storage.GetItemAsStringAsync(AfsConstraints.ModelJsonName).ConfigureAwait(false);
            if (jsonModel != null)
            {
                try
                {
                    var storedModel = JsonSerializer.Deserialize(jsonModel, AfsJsonSerializerContext.Default.AfsModel);
                    if (AfsModelValidator.TryValidate(storedModel, out var validationError))
                    {
                        return storedModel;
                    }

                    Log.ImportFailed(logger, validationError);
                }
                catch (JsonException ex)
                {
                    Log.JsonDeserializationFailed(logger, ex.Message);
                }
            }
            try
            {
                var sampleModel = await http.GetFromJsonAsync("PJSC_AZOVSTAL_IRON_2019_2020.json", AfsJsonSerializerContext.Default.AfsModel).ConfigureAwait(false);
                if (AfsModelValidator.TryValidate(sampleModel, out var validationError))
                {
                    return sampleModel;
                }

                Log.ImportFailed(logger, validationError);
            }
            catch (HttpRequestException ex)
            {
                Log.HttpRequestFailed(logger, ex.Message);
            }
            catch (JsonException ex)
            {
                Log.JsonDeserializationFailed(logger, ex.Message);
            }
            return null;
        }

        public async Task WriteModelAsync(AfsModel model)
        {
            string jsonString = JsonSerializer.Serialize(model, AfsJsonSerializerContext.Default.AfsModel);
            await storage.SetItemAsStringAsync(AfsConstraints.ModelJsonName, jsonString).ConfigureAwait(false);
        }
    }
}
