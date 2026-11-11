namespace AFS.Core.Models;

public static class AfsModelValidator
{
    public static bool TryValidate(AfsModel? model, out string error)
    {
        if (model is null)
        {
            error = "The file does not contain a financial model.";
            return false;
        }

        if (model.F1Base is null || model.F1Current is null || model.F2Base is null ||
            model.F2Current is null || model.AdditionalInfo is null)
        {
            error = "The financial model is missing required sections.";
            return false;
        }

        if (model.BaseYear < AfsConstraints.MinYear || model.CurrentYear > AfsConstraints.MaxYear ||
            model.CurrentYear != model.BaseYear + 1)
        {
            error = "The model years must be consecutive and within the supported range.";
            return false;
        }

        if (!model.F1Base.HasFiniteValues() || !model.F1Current.HasFiniteValues() ||
            !model.F2Base.HasFiniteValues() || !model.F2Current.HasFiniteValues() ||
            !model.AdditionalInfo.HasFiniteValues())
        {
            error = "The financial model contains invalid numeric values.";
            return false;
        }

        error = string.Empty;
        return true;
    }
}
