namespace AFS.Core.Models
{
    public class FixedAssetsInfo : TrackedEntity
    {
        private double receivedFixedAssets;
        private double receivedNewlyAcquiredFixedAssets;
        private double withdrawnFixedAssets;
        private double liquidatedFixedAssets;

        public double ReceivedFixedAssets
        {
            get => receivedFixedAssets;
            set => SetProperty(ref receivedFixedAssets, AfsConstraints.RoundStat(value));
        }

        /// <summary>
        /// Newly acquired fixed assets (as opposed to transferred/used assets).
        /// </summary>
        public double ReceivedNewlyAcquiredFixedAssets
        {
            get => receivedNewlyAcquiredFixedAssets;
            set => SetProperty(ref receivedNewlyAcquiredFixedAssets, AfsConstraints.RoundStat(value));
        }

        public double LeftFixedAssets
        {
            get => withdrawnFixedAssets;
            set => SetProperty(ref withdrawnFixedAssets, AfsConstraints.RoundStat(value));
        }

        public double LiquidatedFixedAssets
        {
            get => liquidatedFixedAssets;
            set => SetProperty(ref liquidatedFixedAssets, AfsConstraints.RoundStat(value));
        }

        internal void Init(FixedAssetsInfo info)
        {
            receivedFixedAssets = info.ReceivedFixedAssets;
            receivedNewlyAcquiredFixedAssets = info.ReceivedNewlyAcquiredFixedAssets;
            withdrawnFixedAssets = info.LeftFixedAssets;
            liquidatedFixedAssets = info.LiquidatedFixedAssets;
        }
    }
}
