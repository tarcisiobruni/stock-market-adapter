using AnotherStockMarketService.Api.Models;

namespace AnotherStockMarketService.Api.Services
{
    public interface IQuotationDataService
    {
        Task<List<Quotation>> GetCodePricesBetweenDates(string ticker, DateTime startDate, DateTime endDate);
        Task<List<Quotation>> AddQuotationBatchJobForTickers(string[] ticker, DateTime startDate, DateTime endDate);
    }
}
