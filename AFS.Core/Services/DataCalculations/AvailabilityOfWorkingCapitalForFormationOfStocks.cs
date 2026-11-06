using AFS.Core.Interfaces;
using AFS.Core.Models.TablsModels;

namespace AFS.Core.Services.DataCalculations;

public class AvailabilityOfWorkingCapitalForFormationOfStocks : IHasBaseCurrent<CharacteristicsOfCapitalCalculationRow>
{
    public string Number { get; private set; } = "1.";
    public CharacteristicsOfCapitalCalculationRow Base { get; private set; } = new();
    public CharacteristicsOfCapitalCalculationRow Current { get; private set; } = new();

    public void Init(SourcesOfCapitalFormation sOCF)
    {
        Base.BeginningOfyear = sOCF.OwnCurrentAssets.Base.BeginningOfyear;
        Base.EndOfYear = sOCF.OwnCurrentAssets.Base.EndOfYear;

        Current.BeginningOfyear = sOCF.OwnCurrentAssets.Current.BeginningOfyear;
        Current.EndOfYear = sOCF.OwnCurrentAssets.Current.EndOfYear;
    }
}
