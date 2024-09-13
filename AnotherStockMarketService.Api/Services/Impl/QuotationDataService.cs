using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.ThirdPartyServices.StockMarketAPI;
using System.Collections.Concurrent;

namespace AnotherStockMarketService.Api.Services.Impl
{
    public class QuotationDataService : IQuotationDataService
    {

        private readonly IServiceProvider _serviceProvider;

        public QuotationDataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Quotation>> GetCodePricesBetweenDates(string ticker, DateTime startDate, DateTime endDate)
        {
            var finantialApi = _serviceProvider.GetRequiredService<StockMarketService>();

            var data = await finantialApi.GetQuotations(ticker, startDate, endDate);

            return data.Select(_ => _ as Quotation).ToList();
        }

        public async Task<List<Quotation>> AddQuotationBatchJobForTickers(string[] tickers, DateTime startDate, DateTime endDate)
        {
            var degreeOfParallelism = 10; // Adjust as needed
            var finantialApi = _serviceProvider.GetRequiredService<StockMarketService>();

            var resultCollection = new ConcurrentBag<Quotation>();

            await Parallel.ForEachAsync(tickers, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, async (code, cancellationToken) =>
            {
                var response = await finantialApi.GetQuotations(code, startDate, endDate);

                // Assuming response is already a collection of Quotation
                foreach (var quotation in response)
                {
                    resultCollection.Add(quotation);
                }
            });

            return resultCollection.ToList();
        }

        //public async Task<List<Quotation>> AddQuotationBatchJobForTickers(string[] tickers, DateTime startDate, DateTime endDate)
        //{
        //    var batchSize = 2;
        //    List<Task> tasks = new List<Task>();
        //    var finantialApi = _serviceProvider.GetRequiredService<StockMarketService>();
        //    var resultCollection = new ConcurrentBag<Quotation>();

        //    for (int i = 0; i < tickers.Count(); i += batchSize)
        //    {
        //        var tickersChunk = tickers.Skip(i).Take(batchSize).ToList();

        //        foreach (var productBundle in tickersChunk)
        //            tasks.Add(ProcessQuotationAsync(productBundle));

        //        await Task.WhenAll(tasks);

        //        tasks = new List<Task>();
        //    }

        //    async Task ProcessQuotationAsync(string code)
        //    {
        //        var response = await finantialApi.GetAggregateStockResults(code, startDate, endDate);

        //        var quotations = response.Select(_ => _ as Quotation).ToList();
        //        foreach (var productBundle in quotations)
        //            resultCollection.Add(productBundle);
        //    }

        //    return resultCollection.ToList();
        //}
    }
}
