namespace AFS.Core.Models
{
    public class AdditionalCompanyInfo : TrackedEntity
    {
        private double averageNumberOfEmployeesBase;
        private double averageNumberOfEmployeesCurrent;

        public FixedAssetsInfo FixedAssetsInfoBase { get; set; } = new();
        public FixedAssetsInfo FixedAssetsInfoCurrent { get; set; } = new();

        public double AverageNumberOfEmployeesBase
        {
            get => averageNumberOfEmployeesBase;
            set => SetProperty(ref averageNumberOfEmployeesBase, AfsConstraints.RoundStat(value));
        }
        public double AverageNumberOfEmployeesCurrent
        {
            get => averageNumberOfEmployeesCurrent;
            set => SetProperty(ref averageNumberOfEmployeesCurrent, AfsConstraints.RoundStat(value));
        }

        internal void Init(AdditionalCompanyInfo info)
        {
            averageNumberOfEmployeesBase = info.AverageNumberOfEmployeesBase;
            averageNumberOfEmployeesCurrent = info.AverageNumberOfEmployeesCurrent;
            FixedAssetsInfoBase.Init(info.FixedAssetsInfoBase);
            FixedAssetsInfoCurrent.Init(info.FixedAssetsInfoCurrent);
        }

        public void SubscribeOnChange(EventHandler propertyChanged)
        {
            PropertyChanged += propertyChanged;
            FixedAssetsInfoBase.PropertyChanged += propertyChanged;
            FixedAssetsInfoCurrent.PropertyChanged += propertyChanged;
        }
        public void UnSubscribeOnChange(EventHandler? propertyChanged)
        {
            if (propertyChanged != null)
            {
                PropertyChanged -= propertyChanged;
                FixedAssetsInfoBase.PropertyChanged -= propertyChanged;
                FixedAssetsInfoCurrent.PropertyChanged -= propertyChanged;
            }
        }
    }
}
