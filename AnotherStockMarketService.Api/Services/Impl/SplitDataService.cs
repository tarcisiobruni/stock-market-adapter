using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.ThirdPartyServices.StockMarketAPI;
using System.Collections.Concurrent;

namespace AnotherStockMarketService.Api.Services.Impl
{
    public class SplitDataService : ISplitDataService
    {

        private readonly IServiceProvider _serviceProvider;

        public SplitDataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<CorporateAction>> GetSplitEventsBetweenDates(DateTime startExecutionDate, DateTime endExecutionDate, string? ticker = null)
        {
            var degreeOfParallelism = 10;

            var finantialApi = _serviceProvider.GetRequiredService<StockMarketService>();

            var data = await finantialApi.GetSplits(startExecutionDate, endExecutionDate, ticker);

            await Parallel.ForEachAsync(data, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, async (splitDTO, cancellationToken) =>
            {
                var result = await finantialApi.GetQuotations(code: splitDTO.C, startDate: splitDTO.ExD.AddDays(-10), endDate: splitDTO.ExD.AddDays(-1));
                splitDTO.PosD = result.Count != 0 ? result.OrderByDescending(_ => _.D).First().D : DateTime.MinValue;
            });

            return data.Select(_ => _ as CorporateAction).ToList();
        }

        public async Task<List<CorporateAction>> AddSplitsBatchJobForTickers(DateTime startExecutionDate, DateTime endExecutionDate, string[] tickers)
        {
            var degreeOfParallelism = 10;

            var finantialApi = _serviceProvider.GetRequiredService<StockMarketService>();
            var resultCollection = new ConcurrentBag<CorporateAction>();

            await Parallel.ForEachAsync(tickers, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, async (code, cancellationToken) =>
            {
                var data = await finantialApi.GetSplits(startExecutionDate, endExecutionDate, code);

                // Assuming response is already a collection of Quotation
                foreach (var splitDTO in data)
                    resultCollection.Add(splitDTO);
            });

            await Parallel.ForEachAsync(resultCollection, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, async (splitDTO, cancellationToken) =>
            {
                var result = await finantialApi.GetQuotations(code: splitDTO.C, startDate: splitDTO.ExD.AddDays(-10), endDate: splitDTO.ExD.AddDays(-1));
                splitDTO.PosD = result.Count != 0 ? result.OrderByDescending(_ => _.D).First().D : DateTime.MinValue;
            });

            return [.. resultCollection];
        }
    }
}
