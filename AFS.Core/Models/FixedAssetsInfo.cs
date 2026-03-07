namespace AFS.Core.Models
{
    public class FixedAssetsInfo : TrackedEntity
    {
        private double receivedFixedAssets;
        private double receivedNewFixedAssets;
        private double withdrawnFixedAssets;
        private double liquidatedFixedAssets;

        public double ReceivedFixedAssets
        {
            get => receivedFixedAssets;
            set => SetProperty(ref receivedFixedAssets, AFSConstraints.RoundStat(value));
        }

        public double ReceivedNewFixedAssets
        {
            get => receivedNewFixedAssets;
            set => SetProperty(ref receivedNewFixedAssets, AFSConstraints.RoundStat(value));
        }

        public double LeftFixedAssets
        {
            get => withdrawnFixedAssets;
            set => SetProperty(ref withdrawnFixedAssets, AFSConstraints.RoundStat(value));
        }

        public double LiquidatedFixedAssets
        {
            get => liquidatedFixedAssets;
            set => SetProperty(ref liquidatedFixedAssets, AFSConstraints.RoundStat(value));
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
