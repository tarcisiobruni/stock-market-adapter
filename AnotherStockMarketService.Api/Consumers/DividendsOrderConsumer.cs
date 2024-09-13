using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.Services;
using MassTransit;

namespace AnotherStockMarketService.Api.Consumers
{
    public class DividendsOrderConsumer :
    IConsumer<DividendsOrder>
    {
        readonly ILogger<DividendsOrderConsumer> _logger;
        private readonly IDistributionDataService _distributionDataService;
        private readonly IConfiguration _configuration;

        public DividendsOrderConsumer(ILogger<DividendsOrderConsumer> logger, IDistributionDataService distributionDataService, IConfiguration configuration)
        {
            _logger = logger;
            _distributionDataService = distributionDataService;
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<DividendsOrder> context)
        {
            _logger.LogInformation("Received Text: {Text}", context.Message);

            var data = await _distributionDataService.AddSplitsBatchJobForTickers(context.Message.StartDate, context.Message.EndDate, context.Message.Tickers);

            await SendParsedData(data);
        }


        private async Task SendParsedData(List<Distribution> events)
        {
            try
            {
                HttpRequestMessage request = HttpRequestHelper.PrepareRequestMessage<Distribution>(_configuration, events, "Proceeds");
                var client = new HttpClient();
                var response = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }

    public record DividendsOrder()
    {
        public string[] Tickers { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
