using AFS.Core.Model;

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
            set
            {
                if (!averageNumberOfEmployeesBase.Equals(value))
                {
                    averageNumberOfEmployeesBase = value;
                    NotifyStateChanged();
                }
            }
        }
        public double AverageNumberOfEmployeesCurrent
        {
            get => averageNumberOfEmployeesCurrent;
            set
            {
                if (!averageNumberOfEmployeesCurrent.Equals(value))
                {
                    averageNumberOfEmployeesCurrent = value;
                    NotifyStateChanged();
                }
            }
        }

        internal void Init(AdditionalCompanyInfo info)
        {
            averageNumberOfEmployeesBase = info.AverageNumberOfEmployeesBase;
            averageNumberOfEmployeesCurrent = info.AverageNumberOfEmployeesCurrent;
            FixedAssetsInfoBase.Init(info.FixedAssetsInfoBase);
            FixedAssetsInfoCurrent.Init(info.FixedAssetsInfoCurrent);
        }

        public void SubscribeOnChange(Action onChange)
        {
            OnChange += onChange;
            FixedAssetsInfoBase.OnChange += onChange;
            FixedAssetsInfoCurrent.OnChange += onChange;
        }
        public void UnSubscribeOnChange(Action? onChange)
        {
            if (onChange != null)
            {
                OnChange -= onChange;
                FixedAssetsInfoBase.OnChange -= onChange;
                FixedAssetsInfoCurrent.OnChange -= onChange;
            }
        }
    }

    public class FixedAssetsInfo : TrackedEntity
    {
        private double receivedFixedAssets;
        private double receivedNewFixedAssets;
        private double withdrawnFixedAssets;
        private double liquidatedFixedAssets;
        public double ReceivedFixedAssets
        {
            get => receivedFixedAssets;
            set
            {
                if (!receivedFixedAssets.Equals(value))
                {
                    receivedFixedAssets = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }
        public double ReceivedNewFixedAssets
        {
            get => receivedNewFixedAssets;
            set
            {
                if (!receivedNewFixedAssets.Equals(value))
                {
                    receivedNewFixedAssets = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }
        public double LeftFixedAssets
        {
            get => withdrawnFixedAssets;
            set
            {
                if (!withdrawnFixedAssets.Equals(value))
                {
                    withdrawnFixedAssets = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }
        public double LiquidatedFixedAssets
        {
            get => liquidatedFixedAssets;
            set
            {
                if (!liquidatedFixedAssets.Equals(value))
                {
                    liquidatedFixedAssets = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }

        internal void Init(FixedAssetsInfo info)
        {
            receivedFixedAssets = info.ReceivedFixedAssets;
            receivedNewFixedAssets = info.ReceivedNewFixedAssets;
            withdrawnFixedAssets = info.LeftFixedAssets;
            liquidatedFixedAssets = info.LiquidatedFixedAssets;
        }
    }
}