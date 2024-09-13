using System.Text.Json.Serialization;

namespace AnotherStockMarketService.Api.ThirdPartyServices.StockMarketAPI
{
    public class AggregateApiResponse
    {
        [JsonPropertyName("adjusted")]
        public bool Adjusted { get; set; }
        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
        [JsonPropertyName("queryCount")]
        public int QueryCount { get; set; }
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
        [JsonPropertyName("resultsCount")]
        public int ResultsCount { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
        [JsonPropertyName("results")]
        public List<AggregateStockResult> Results { get; set; }
    }

    public class AggregateStockResult
    {
        [JsonPropertyName("c")]
        public decimal C { get; set; }  // Close price
        [JsonPropertyName("h")]
        public decimal H { get; set; }  // High price
        [JsonPropertyName("l")]
        public decimal L { get; set; }  // Low price
        [JsonPropertyName("n")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? N { get; set; }  // Number of transactions
        [JsonPropertyName("o")]
        public decimal O { get; set; }  // Open price
        [JsonPropertyName("t")]
        public long T { get; set; }  // Unix timestamp (in milliseconds)
        [JsonPropertyName("v")]
        public decimal V { get; set; }  // Trading volume
        [JsonPropertyName("vw")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Vw { get; set; }  // Volume weighted average price
    }

    public class MarketDailyResponse
    {
        [JsonPropertyName("adjusted")]
        public bool Adjusted { get; set; }
        [JsonPropertyName("queryCount")]
        public int QueryCount { get; set; }
        [JsonPropertyName("results")]
        public List<MarketDailyResult> Results { get; set; }
        [JsonPropertyName("resultsCount")]
        public int ResultsCount { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class MarketDailyResult
    {
        [JsonPropertyName("T")]
        public string T { get; set; }  // Ticker symbol
        public decimal C { get; set; }  // Close price
        public decimal H { get; set; }  // High price
        public decimal L { get; set; }  // Low price
        public int N { get; set; }  // Number of transactions
        public decimal O { get; set; }  // Open price
        [JsonPropertyName("t")]
        public long t { get; set; }  // Unix Msec timestamp for the end of the aggregate window
        public decimal V { get; set; }  // Trading volume
        [JsonPropertyName("vw")]
        public decimal Vw { get; set; }  // Volume weighted average price
    }

    public class TickerResponse
    {
        public int Count { get; set; }
        public string NextUrl { get; set; }
        public string RequestId { get; set; }
        public List<TickerResult> Results { get; set; }
        public string Status { get; set; }
    }

    public class TickerResult
    {
        public bool Active { get; set; }
        public string Cik { get; set; }
        public string CompositeFigi { get; set; }
        public string CurrencyName { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public string Locale { get; set; }
        public string Market { get; set; }
        public string Name { get; set; }
        public string PrimaryExchange { get; set; }
        public string ShareClassFigi { get; set; }
        public string Ticker { get; set; }
        public string Type { get; set; }
    }

    public class StockSplitResponse
    {
        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
        [JsonPropertyName("results")]
        public List<StockSplitResult> Results { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class StockSplitResult
    {
        [JsonPropertyName("execution_date")]
        public DateTime ExecutionDate { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("split_from")]
        public decimal SplitFrom { get; set; }
        [JsonPropertyName("split_to")]
        public decimal SplitTo { get; set; }
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
    }

    public class DividendResponse
    {
        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
        [JsonPropertyName("results")]
        public List<DividendResult> Results { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class DividendResult
    {
        [JsonPropertyName("cash_amount")]
        public decimal CashAmount { get; set; }

        [JsonPropertyName("declaration_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? DeclarationDate { get; set; }

        [JsonPropertyName("dividend_type")]
        public string DividendType { get; set; }

        [JsonPropertyName("ex_dividend_date")]
        public DateTime ExDividendDate { get; set; }

        [JsonPropertyName("frequency")]
        public int Frequency { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("pay_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? PayDate { get; set; }

        [JsonPropertyName("record_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? RecordDate { get; set; }

        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
    }
}
