using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.Services;
using MassTransit;
using System.Text;
using System.Text.Json;

namespace AnotherStockMarketService.Api.Consumers
{
    public class QuotationOrderConsumer :
    IConsumer<QuotationOrder>
    {
        readonly ILogger<QuotationOrderConsumer> _logger;
        private readonly IQuotationDataService _quotationDataService;
        private readonly IConfiguration _configuration;

        public QuotationOrderConsumer(ILogger<QuotationOrderConsumer> logger, IQuotationDataService quotationDataService, IConfiguration configuration)
        {
            _logger = logger;
            _quotationDataService = quotationDataService;
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<QuotationOrder> context)
        {
            _logger.LogInformation("Received Text: {Text}", context.Message);

            var data = await _quotationDataService.AddQuotationBatchJobForTickers(context.Message.Tickers, context.Message.StartDate, context.Message.EndDate);

            await SendParsedData(data);
        }


        private async Task SendParsedData(List<Quotation> quotations)
        {
            try
            {
                HttpRequestMessage request = HttpRequestHelper.PrepareRequestMessage(_configuration, quotations, "Quotations");
                var client = new HttpClient();
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }

    public record QuotationOrder()
    {
        public string[] Tickers { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
