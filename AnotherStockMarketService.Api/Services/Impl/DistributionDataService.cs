using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.ThirdPartyServices.StockMarketAPI;
using System.Collections.Concurrent;

namespace AnotherStockMarketService.Api.Services.Impl
{
    public class DistributionDataService : IDistributionDataService
    {

        private readonly IServiceProvider _serviceProvider;

        public DistributionDataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<Distribution>> AddSplitsBatchJobForTickers(DateTime startExecutionDate, DateTime endExecutionDate, string[] tickers)
        {
            var degreeOfParallelism = 10;

            var finantialApi = _serviceProvider.GetRequiredService<StockMarketService>();
            var resultCollection = new ConcurrentBag<Distribution>();

            await Parallel.ForEachAsync(tickers, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, async (code, cancellationToken) =>
            {
                var data = await finantialApi.GetDividends(startExecutionDate, endExecutionDate, code);

                // Assuming response is already a collection of Quotation
                foreach (var splitDTO in data)
                    resultCollection.Add(splitDTO);
            });

            await Parallel.ForEachAsync(resultCollection, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, async (splitDTO, cancellationToken) =>
            {
                var result = await finantialApi.GetQuotations(code: splitDTO.C, startDate: splitDTO.ExD.AddDays(-10), endDate: splitDTO.ExD.AddDays(-1));
                splitDTO.PosD = result.Count != 0 ? result.OrderByDescending(_ => _.D).First().D : null;
            });

            return resultCollection.ToList();
        }

        public async Task<List<Distribution>> GetCodeDistributionsBetweenDates(DateTime startDate, DateTime endDate, string? ticker, bool loadPositionDate = false)
        {
            int degreeOfParallelism = 10;

            var finantialApi = _serviceProvider.GetRequiredService<StockMarketService>();

            var data = await finantialApi.GetDividends(startDate, endDate, ticker);

            if (loadPositionDate)
            {

                await Parallel.ForEachAsync(data, new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism }, async (dividendDTO, cancellationToken) =>
                {
                    var result = await finantialApi.GetQuotations(code: dividendDTO.C, startDate: dividendDTO.ExD.AddDays(-10), endDate: dividendDTO.ExD.AddDays(-1));
                    dividendDTO.PosD = result.Count != 0 ? result.OrderByDescending(_ => _.D).First().D : null;
                });
            }

            return data.Select(_ => _ as Distribution).ToList();
        }
    }
}
