namespace AnotherStockMarketService.Api.Models
{
    public class Distribution
    {
        /// <summary>
        /// ExD
        /// </summary>
        public DateTime ExD { get; set; }

        /// <summary>
        /// Approval Date
        /// </summary>
        public DateTime DD { get; set; }

        /// <summary>
        /// Payment Date
        /// </summary>
        public DateTime PD { get; set; }

        /// <summary>
        /// Position Date
        /// </summary>
        public DateTime? PosD { get; set; }

        /// <summary>
        /// CashAmount
        /// </summary>
        public decimal CA { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string C { get; set; }

        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; set; }
    }
}
