using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace AFS.ComponentLibrary.Resources;

/// <summary>
/// AOT-safe chart label lookup using strongly-typed Resource properties.
/// Maps dynamic keys to Resource class properties at runtime without reflection.
/// </summary>
#pragma warning disable S3963 // Static fields should be initialized inline - FrozenDictionary requires runtime initialization
public static class ChartLabelHelper
{
    // Frozen dictionary for O(1) lookup - built once at startup
    private static readonly FrozenDictionary<string, Func<string>> _labelGetters;

    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Resource))]
    static ChartLabelHelper()
    {
        // Map chart data keys to strongly-typed Resource property accessors
        var dict = new Dictionary<string, Func<string>>(StringComparer.Ordinal)
        {
            // Composition of Assets
            ["NonCurrentImmobilizedAssets"] = () => Resource.NonCurrentImmobilizedAssets,
            ["TangibleCurrentAssets"] = () => Resource.TangibleCurrentAssets,
            ["AccountsReceivable"] = () => Resource.AccountsReceivable,
            ["CashCurrentFinancialInvestments"] = () => Resource.CashCurrentFinancialInvestments,
            ["OtherCurrentAssets"] = () => Resource.OtherCurrentAssets,
            ["NonCurrentAssetsHeldForSale"] = () => Resource.NonCurrentAssetsHeldForSale,
            ["FutureExpenses"] = () => Resource.FutureExpenses,

            // Sources of Capital Formation
            ["RegisteredCapital"] = () => Resource.RegisteredCapital,
            ["AdditionalCapital"] = () => Resource.AdditionalCapital,
            ["ReserveCapital"] = () => Resource.ReserveCapital,
            ["RetainedEarnings"] = () => Resource.RetainedEarnings,
            ["UnpaidCapital"] = () => Resource.UnpaidCapital,
            ["WithdrawnCapital"] = () => Resource.WithdrawnCapital,

            // Structure of Accounts Payable
            ["WithBuyersOrSuppliers"] = () => Resource.WithBuyersOrSuppliers,
            ["WithLongTermLiabilities"] = () => Resource.WithLongTermLiabilities,
            ["ForBills"] = () => Resource.ForBills,
            ["FromInsurance"] = () => Resource.FromInsurance,
            ["WithBudgetAndExtraBudgetaryFunds"] = () => Resource.WithBudgetAndExtraBudgetaryFunds,
            ["WithAccruedIncome"] = () => Resource.WithAccruedIncome,
            ["WithPayroll"] = () => Resource.WithPayroll,
            ["WithAdvances"] = () => Resource.WithAdvances,
            ["WithParticipants"] = () => Resource.WithParticipants,
            ["WithInternalCashSettlements"] = () => Resource.WithInternalCashSettlements,
            ["WithOther"] = () => Resource.WithOther,

            // Common
            ["Value"] = () => Resource.Value,
            ["Unknown"] = () => "Unknown",
        };

        _labelGetters = dict.ToFrozenDictionary(StringComparer.Ordinal);
    }
#pragma warning restore S3963

    /// <summary>
    /// Gets a localized chart label. AOT-safe - no reflection.
    /// </summary>
    public static string Get(string? key)
    {
        if (string.IsNullOrEmpty(key))
            return "Unknown";

        // Fast O(1) lookup in frozen dictionary
        if (_labelGetters.TryGetValue(key, out var getter))
        {
            return getter();
        }

        // Fallback to key itself for unmapped keys
        return key;
    }
}
