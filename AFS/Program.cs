using AFS;
using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Services;
using AFS.Core.Services.DataCalculations;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Globalization;
using System.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configure for better performance
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Use singleton HttpClient for better performance
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Singleton services (stateful, shared across app)
builder.Services.AddSingleton<AFSModel>();
builder.Services.AddSingleton<AFSConstraints>();
builder.Services.AddSingleton<AppThemeService>();

// Localization - no need to set ResourcesPath since resources are in referenced AFS.ComponentLibrary
builder.Services.AddLocalization();

// Blazored LocalStorage with optimized JSON options
builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.WriteIndented = false;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Radzen services - REQUIRED for Radzen components
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

// Scoped services (per-user session)
builder.Services.AddScoped<ICultureStorageHandler, LocalStorageCultureHandler>();
builder.Services.AddScoped<IModelStorageHandler, LocalStorageModelHandler>();
builder.Services.AddScoped<IModelExportImportHandler, BrowserExportImportHandler>();
builder.Services.AddScoped<JsInterop>();

// Transient services (lightweight calculation services)
builder.Services.AddTransient<CharacteristicsOfCapital>();
builder.Services.AddTransient<IndicatorsOfTurnoverOfCurrentAssets>();
builder.Services.AddTransient<FactorsAffectingTurnoverOfWorkingCapital>();
builder.Services.AddTransient<IndicatorsOfEfficiencyOfWorkingCapital>();
builder.Services.AddTransient<AvailabilityAndMovementOfFixedAssets>();
builder.Services.AddTransient<IndicatorsOfStateAndMovementOfFixedAssets>();
builder.Services.AddTransient<CalculationOfIndicatorsOfEfficiencyOfUseOfFixedAssets>();
builder.Services.AddTransient<FactorAnalysisOfFixedAssets>();
builder.Services.AddTransient<IndicatorsOfEfficiencyOfUseOfIntangibleAssets>();
builder.Services.AddTransient<SourcesOfCapitalFormation>();
builder.Services.AddTransient<AssessmentOfReceivableAndPayable>();
builder.Services.AddTransient<IndicatorsOfBusinessActivity>();
builder.Services.AddTransient<LiquidityIndicatorsOfBalance>();
builder.Services.AddTransient<SolvencyRatios>();
builder.Services.AddTransient<GeneralIndicatorsOfFinancialStability>();
builder.Services.AddTransient<ClassificationOfTypesOfFinancialStability>();
builder.Services.AddTransient<IndicatorsOfFinancialStability>();

// Charts
builder.Services.AddTransient<CompositionOfAssetsChart>();
builder.Services.AddTransient<WorkingCapitalTurnoverTimeChart>();
builder.Services.AddTransient<SourcesOfCapitalFormationChart>();
builder.Services.AddTransient<StructureOfAccountsPayableChart>();

var host = builder.Build();

// Check for ?lang= query parameter and set culture
var navigationManager = host.Services.GetRequiredService<NavigationManager>();
var uri = new Uri(navigationManager.Uri);

if (!string.IsNullOrEmpty(uri.Query))
{
    var query = HttpUtility.ParseQueryString(uri.Query);
    var langParam = query.Get("lang");

    if (!string.IsNullOrEmpty(langParam))
    {
        try
        {
            var cultureStorageService = host.Services.GetRequiredService<ICultureStorageHandler>();
            var requestedCulture = CultureInfo.GetCultureInfo(langParam);

            await cultureStorageService.WriteCultureAsync(requestedCulture);
            CultureInfo.DefaultThreadCurrentCulture = requestedCulture;
            CultureInfo.DefaultThreadCurrentUICulture = requestedCulture;
        }
        catch (Exception ex)
        {
            // Log error but continue - will use default culture
            Console.WriteLine($"Failed to set culture from URL parameter: {ex.Message}");
        }
    }
}

// Initialize culture and model in parallel for faster startup
var cultureTask = Task.Run(async () =>
{
    var cultureStorageService = host.Services.GetService<ICultureStorageHandler>();
    if (cultureStorageService != null)
    {
        await cultureStorageService.InitializeCultureAsync();
    }
});

var modelTask = Task.Run(async () =>
{
    var modelStorageHandler = host.Services.GetService<IModelStorageHandler>();
    if (modelStorageHandler != null)
    {
        await modelStorageHandler.InitializeModelAsync();
    }
});

// Wait for both initialization tasks
await Task.WhenAll(cultureTask, modelTask);

await host.RunAsync();