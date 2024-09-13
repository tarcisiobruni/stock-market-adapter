namespace AnotherStockMarketService.Api.Models
{
    public class CorporateAction
    {
        /// <summary>
        /// PositionDate
        /// </summary>
        public DateTime PosD { get; set; }

        /// <summary>
        /// Execution Date
        /// </summary>
        public DateTime ExD { get; set; }

        /// <summary>
        /// SplitFrom 
        /// </summary>
        public decimal F { get; set; }

        /// <summary>
        /// SplitTo
        /// </summary>
        public decimal T { get; set; }

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
