namespace AFS.Core.Models.TablsModels
{
    /// <summary>
    /// Represents fixed assets data for financial analysis.
    /// </summary>
    public class FixedAssets
    {
        public double BalanceAtBeginInitialCost { get; set; }
        public double Received { get; set; }
        /// <summary>
        /// Newly acquired fixed assets (as opposed to transferred/used assets).
        /// </summary>
        public double ReceivedNewlyAcquired { get; set; }
        public double Left { get; set; }
        public double Liquidated { get; set; }
        public double BalanceAtEndInitialCost { get; set; }
        public double BalanceAtEndMinusDepreciation { get; set; }
    }
}
