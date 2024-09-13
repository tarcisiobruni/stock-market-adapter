using AnotherStockMarketService.Api.Models;

namespace AnotherStockMarketService.Api.Services
{
    public interface ISplitDataService
    {
        Task<List<CorporateAction>> GetSplitEventsBetweenDates(DateTime startExecutionDate, DateTime endExecutionDate, string? ticker);
        Task<List<CorporateAction>> AddSplitsBatchJobForTickers(DateTime startDate, DateTime endDate, string[] tickers);
    }
}
