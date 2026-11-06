using AFS.Core.Interfaces;
using AFS.Core.Models;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace AFS.Core.Services
{
    /// <summary>
    /// Handles culture/language persistence to browser local storage.
    /// Sealed because it's a DI service not designed for inheritance.
    /// </summary>
    public sealed class LocalStorageCultureHandler : ICultureStorageHandler
    {
        private readonly ILocalStorageService storage;
        private readonly ILogger<LocalStorageCultureHandler> logger;

        public LocalStorageCultureHandler(ILocalStorageService storage, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(storage);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            this.storage = storage;
            logger = loggerFactory.CreateLogger<LocalStorageCultureHandler>();
        }

        public async Task<CultureInfo> ReadCultureAsync()
        {
            var langCulture = await storage.GetItemAsStringAsync(AfsConstraints.LangCultureName).ConfigureAwait(false);
            if (langCulture != null)
            {
                try
                {
                    return new CultureInfo(langCulture);
                }
                catch (CultureNotFoundException ex)
                {
                    Log.InvalidCultureName(logger, ex, langCulture);
                }
            }
            // Default to English if no culture is stored
            return new CultureInfo("en");
        }

        public async Task InitializeCultureAsync()
        {
            CultureInfo defaultCulture = await ReadCultureAsync().ConfigureAwait(false);
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;
        }

        public async Task WriteCultureAsync(CultureInfo cultureInfo)
        {
            await storage.SetItemAsStringAsync(AfsConstraints.LangCultureName, cultureInfo.Name).ConfigureAwait(false);
        }
    }
}
