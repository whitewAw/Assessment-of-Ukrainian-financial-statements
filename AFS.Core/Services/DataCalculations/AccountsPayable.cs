using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class AccountsPayable : ICapitalComponentBorrowed<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "2.3.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfBorrowedCapitalBase { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfBorrowedCapitalCurrent { get; private set; } = new();

    public void Init(AfsModel model, RaisedCapital raisedCapital)
    {
        Base.BeginningOfyear = model.F1Base.GetAccountsPayable(true);
        Base.EndOfYear = model.F1Base.GetAccountsPayable(false);

        Current.BeginningOfyear = model.F1Current.GetAccountsPayable(true);
        Current.EndOfYear = model.F1Current.GetAccountsPayable(false);

        InPercentageOfBorrowedCapitalBase.BeginningOfyear = Base.BeginningOfyear / raisedCapital.Base.BeginningOfyear * 100;
        InPercentageOfBorrowedCapitalBase.EndOfYear = Base.EndOfYear / raisedCapital.Base.EndOfYear * 100;

        InPercentageOfBorrowedCapitalCurrent.BeginningOfyear = Current.BeginningOfyear / raisedCapital.Current.BeginningOfyear * 100;
        InPercentageOfBorrowedCapitalCurrent.EndOfYear = Current.EndOfYear / raisedCapital.Current.EndOfYear * 100;
    }
}
