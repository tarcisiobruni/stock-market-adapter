namespace AnotherStockMarketService.Api.ThirdPartyServices.StockMarketAPI
{
    public class StockMarketService
    {
        readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        
        public StockMarketService(ILogger<StockMarketService> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<PriceDTO>> GetQuotations(string code, DateTime startDate, DateTime endDate)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var urlBase = _configuration.GetValue<string>("StockMarketConfiguration:urlBaseQuotation");
                var apiKey = _configuration.GetValue<string>("StockMarketConfiguration:apiKey");

                var requestUri = $"{urlBase}/{code}/range/1/day/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}?adjusted=false&apiKey={apiKey}&limit=50000";

                var response = await httpClient.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                var aggregateApiResponse = await response.Content.ReadFromJsonAsync<AggregateApiResponse>();

                var data = ConvertAggregateToPriceDTO(code, aggregateApiResponse);

                var nextUrl = aggregateApiResponse.NextUrl;

                while (!string.IsNullOrEmpty(nextUrl))
                {
                    response = await httpClient.GetAsync($"{nextUrl}&apiKey={apiKey}");
                    aggregateApiResponse = await response.Content.ReadFromJsonAsync<AggregateApiResponse>();
                    List<PriceDTO> nextUrlData = ConvertAggregateToPriceDTO(code, aggregateApiResponse);
                    data.AddRange(nextUrlData);
                    nextUrl = aggregateApiResponse.NextUrl;
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new List<PriceDTO>();
            }
        }

        public async Task<List<SplitDTO>> GetSplits(DateTime startDate, DateTime endDate, string? ticker = null)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var urlBase = _configuration.GetValue<string>("StockMarketConfiguration:urlBaseSplits");
                var apiKey = _configuration.GetValue<string>("StockMarketConfiguration:apiKey");

                var requestUri = $"{urlBase}/reference/splits?limit=1000&apiKey={apiKey}&execution_date.gte={startDate:yyyy-MM-dd}&execution_date.lte={endDate:yyyy-MM-dd}";

                if (!string.IsNullOrEmpty(ticker))
                {
                    requestUri += $"&ticker={ticker}";
                }

                var response = await httpClient.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                var stockSplitResponse = await response.Content.ReadFromJsonAsync<StockSplitResponse>();

                var data = ConvertToSplitDTO(stockSplitResponse);

                var nextUrl = stockSplitResponse.NextUrl;

                while (!string.IsNullOrEmpty(nextUrl))
                {
                    response = await httpClient.GetAsync($"{nextUrl}&apiKey={apiKey}");
                    stockSplitResponse = await response.Content.ReadFromJsonAsync<StockSplitResponse>();
                    var nextUrlData = ConvertToSplitDTO(stockSplitResponse);
                    data.AddRange(nextUrlData);
                    nextUrl = stockSplitResponse.NextUrl;
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new List<SplitDTO>();
            }
        }

        public async Task<List<DividendDTO>> GetDividends(DateTime startDate, DateTime endDate, string? ticker = null)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var urlBase = _configuration.GetValue<string>("StockMarketConfiguration:urlBaseDistributions");
                var apiKey = _configuration.GetValue<string>("StockMarketConfiguration:apiKey");

                var requestUri = $"{urlBase}/reference/dividends?limit=1000&apiKey={apiKey}&ex_dividend_date.gte={startDate:yyyy-MM-dd}&ex_dividend_date.lte={endDate:yyyy-MM-dd}";

                if (!string.IsNullOrEmpty(ticker))
                {
                    requestUri += $"&ticker={ticker}";
                }

                var response = await httpClient.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                var stockDividendResponse = await response.Content.ReadFromJsonAsync<DividendResponse>();

                var data = FilterAndConvertToDividendDTO(stockDividendResponse);

                var nextUrl = stockDividendResponse.NextUrl;

                while (!string.IsNullOrEmpty(nextUrl))
                {
                    response = await httpClient.GetAsync($"{nextUrl}&apiKey={apiKey}");
                    stockDividendResponse = await response.Content.ReadFromJsonAsync<DividendResponse>();
                    var nextUrlData = FilterAndConvertToDividendDTO(stockDividendResponse);
                    data.AddRange(nextUrlData);
                    nextUrl = stockDividendResponse.NextUrl;
                }

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return new List<DividendDTO>();
            }
        }


        #region Private Methods

        private static List<PriceDTO> ConvertAggregateToPriceDTO(string code, AggregateApiResponse? aggregateApiResponse)
        {
            return aggregateApiResponse.Results.Select(summary => new PriceDTO
            {
                C = code,
                D = DateTimeOffset.FromUnixTimeMilliseconds(summary.T).Date,
                P = summary.C,
            }).ToList();
        }

        private static List<SplitDTO> ConvertToSplitDTO(StockSplitResponse? stockSplitResponse)
        {
            return stockSplitResponse.Results.Select(summary => new SplitDTO
            {
                C = summary.Ticker,
                ExD = summary.ExecutionDate,
                F = summary.SplitFrom,
                T = summary.SplitTo,
                Id = summary.Id,
            }).ToList();
        }

        private static List<DividendDTO> FilterAndConvertToDividendDTO(DividendResponse? dividendResponse)
        {
            return dividendResponse.Results
                .Where( _ => _.PayDate.HasValue && _.DeclarationDate.HasValue)
                .Select(summary => new DividendDTO
            {
                C = summary.Ticker,
                ExD = summary.ExDividendDate,
                CA = summary.CashAmount,

                DD = summary.DeclarationDate.Value,
                PD = summary.PayDate.Value,
                Id = summary.Id,
            }).ToList();
        }

        #endregion
    }
}
