namespace AFS.Core.Json;

public class ReceivablePayableData
{
    public string? CompanyName { get; set; }
    public int BaseYear { get; set; }
    public int CurrentYear { get; set; }
    public ReceivablePayableSummary? Summary { get; set; }
    public ReceivablePayableCategoryData? BuyersSuppliers { get; set; }
    public ReceivablePayableCategoryData? BudgetFunds { get; set; }
    public ReceivablePayableCategoryData? Advances { get; set; }
    public ReceivablePayableCategoryData? Others { get; set; }
}
