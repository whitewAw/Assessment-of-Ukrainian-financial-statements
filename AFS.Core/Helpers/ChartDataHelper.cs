using AFS.Core.Models;

namespace AFS.Core.Helpers;

/// <summary>
/// Helper class for chart data operations.
/// Eliminates code duplication across chart services (DRY principle).
/// </summary>
public static class ChartDataHelper
{
    /// <summary>
    /// Adds a chart data item to the collection if the value is valid (non-zero and not NaN/Infinity).
    /// </summary>
    /// <param name="items">The collection to add the item to.</param>
    /// <param name="itemName">The name/key of the chart item.</param>
    /// <param name="value">The nullable value to check and add.</param>
    public static void AddIfValid(ICollection<ChartDataItem> items, string itemName, double? value)
    {
        var val = value.GetValueOrDefault(0);
        if (!AfsConstraints.IsZeroOrInvalid(val))
        {
            items.Add(new ChartDataItem { Item = itemName, Value = val });
        }
    }

    /// <summary>
    /// Sorts chart data items by value in descending order and returns as IReadOnlyList.
    /// </summary>
    public static IReadOnlyList<ChartDataItem> SortDescending(IEnumerable<ChartDataItem> items) =>
        items.OrderByDescending(item => item.Value).ToList();

    /// <summary>
    /// Calculates percentage of a value relative to total for base or current year.
    /// Returns 0 if total is zero or invalid to avoid division errors.
    /// </summary>
    /// <param name="value">The value to calculate percentage for.</param>
    /// <param name="total">The total value to calculate percentage against.</param>
    /// <returns>Percentage (0-100) or 0 if calculation is invalid.</returns>
    public static double CalculatePercentage(double? value, double? total)
    {
        var totalVal = total.GetValueOrDefault(0);
        if (AfsConstraints.IsZeroOrInvalid(totalVal))
        {
            return 0;
        }

        var val = value.GetValueOrDefault(0);
        return val / totalVal * 100;
    }
}
