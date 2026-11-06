using AFS.Core.Interfaces;

namespace AFS.Core.Models.TablsModels
{
    public class ClassificationOfTypesOfFinancialStabilityRow : IHasStabilityValues
    {
        public string? Number { get; set; }
        public string CurrentBVal { get; set; } = string.Empty;
        public string CurrentCVal { get; set; } = string.Empty;
        public string ShortBVal { get; set; } = string.Empty;
        public string ShortCVal { get; set; } = string.Empty;
        public string LongBVal { get; set; } = string.Empty;
        public string LongCVal { get; set; } = string.Empty;
    }
}
