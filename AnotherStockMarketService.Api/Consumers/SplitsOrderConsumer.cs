using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.Services;
using MassTransit;

namespace AnotherStockMarketService.Api.Consumers
{
    public class SplitsOrderConsumer :
    IConsumer<SplitsOrder>
    {
        readonly ILogger<SplitsOrderConsumer> _logger;
        private readonly ISplitDataService _splitDataService;
        private readonly IConfiguration _configuration;

        public SplitsOrderConsumer(ILogger<SplitsOrderConsumer> logger, ISplitDataService splitDataService, IConfiguration configuration)
        {
            _logger = logger;
            _splitDataService = splitDataService;
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<SplitsOrder> context)
        {
            _logger.LogInformation("Received Text: {Text}", context.Message);

            var data = await _splitDataService.AddSplitsBatchJobForTickers(context.Message.StartDate, context.Message.EndDate, context.Message.Tickers);

            await SendParsedData(data);
        }


        private async Task SendParsedData(List<CorporateAction> events)
        {
            try
            {
                HttpRequestMessage request = HttpRequestHelper.PrepareRequestMessage(_configuration, events, "Events");
                var client = new HttpClient();
                var response = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }

    public record SplitsOrder()
    {
        public string[] Tickers { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
