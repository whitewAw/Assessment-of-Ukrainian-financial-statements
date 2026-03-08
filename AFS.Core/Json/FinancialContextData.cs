namespace AFS.Core.Json;

public class FinancialContextData
{
    public CompanyInfoData? Company { get; set; }
    public BalanceSheetData? BalanceSheet { get; set; }
    public IncomeStatementData? IncomeStatement { get; set; }
}
