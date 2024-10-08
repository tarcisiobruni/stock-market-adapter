﻿using AnotherStockMarketService.Api.Models;

namespace AnotherStockMarketService.Api.Services
{
    public interface IDistributionDataService
    {
        Task<List<Distribution>> GetCodeDistributionsBetweenDates(DateTime startExecutionDate, DateTime endExecutionDate, string? ticker, bool loadPositionDate = false);
        Task<List<Distribution>> AddSplitsBatchJobForTickers(DateTime startExecutionDate, DateTime endExecutionDate, string[] tickers);
    }
}
