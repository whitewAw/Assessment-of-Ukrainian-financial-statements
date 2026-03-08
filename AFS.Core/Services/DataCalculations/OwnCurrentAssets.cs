using AFS.Core.Interfaces;
using AFS.Core.Models;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class OwnCurrentAssets : ICapitalComponentEquity<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "1.1";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfEquityBase { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow InPercentageOfEquityCurrent { get; private set; } = new();

    public void Init(AfsModel model, Equity equity)
    {
        Base.BeginningOfyear = equity.Base.BeginningOfyear - model.F1Base.GetF1095Begin();
        Base.EndOfYear = equity.Base.EndOfYear - model.F1Base.GetF1095End();

        Current.BeginningOfyear = equity.Current.BeginningOfyear - model.F1Current.GetF1095Begin();
        Current.EndOfYear = equity.Current.EndOfYear - model.F1Current.GetF1095End();

        InPercentageOfEquityBase.BeginningOfyear = Base.BeginningOfyear / equity.Base.BeginningOfyear * 100;
        InPercentageOfEquityBase.EndOfYear = Base.EndOfYear / equity.Base.EndOfYear * 100;

        InPercentageOfEquityCurrent.BeginningOfyear = Current.BeginningOfyear / equity.Current.BeginningOfyear * 100;
        InPercentageOfEquityCurrent.EndOfYear = Current.EndOfYear / equity.Current.EndOfYear * 100;
    }
}
