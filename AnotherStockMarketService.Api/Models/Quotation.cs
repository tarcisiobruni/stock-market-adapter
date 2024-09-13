namespace AnotherStockMarketService.Api.Models
{
    public class Quotation
    {
        /// <summary>
        /// Close Price
        /// </summary>
        public string C { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal P { get; set; }
        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime D { get; set; }
    }
}
