using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class ShortTermLoans : ICapitalComponentBorrowed<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "2.2.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfBorrowedCapitalBase { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfBorrowedCapitalCurrent { get; private set; } = new();

    public void Init(AfsModel model, RaisedCapital raisedCapital)
    {
        Base.BeginningOfyear = model.F1Base.F1600.Begin + model.F1Base.F1610.Begin;
        Base.EndOfYear = model.F1Base.F1600.End + model.F1Base.F1610.End;

        Current.BeginningOfyear = model.F1Current.F1600.Begin + model.F1Current.F1610.Begin;
        Current.EndOfYear = model.F1Current.F1600.End + model.F1Current.F1610.End;

        InPercentageOfBorrowedCapitalBase.BeginningOfyear = Base.BeginningOfyear / raisedCapital.Base.BeginningOfyear * 100;
        InPercentageOfBorrowedCapitalBase.EndOfYear = Base.EndOfYear / raisedCapital.Base.EndOfYear * 100;

        InPercentageOfBorrowedCapitalCurrent.BeginningOfyear = Current.BeginningOfyear / raisedCapital.Current.BeginningOfyear * 100;
        InPercentageOfBorrowedCapitalCurrent.EndOfYear = Current.EndOfYear / raisedCapital.Current.EndOfYear * 100;
    }
}
