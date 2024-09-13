using AnotherStockMarketService.Api.Consumers;
using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AnotherStockMarketService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SplitsController(IBus bus) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<CorporateAction>> Get([FromQuery(Name = "code")] string? code, [FromQuery(Name = "startDate")] DateTime startDate, [FromQuery(Name = "endDate")] DateTime endDate, [FromServices] ISplitDataService splitDataService, [FromQuery(Name = "loadPositionDate")] bool loadPositionDate = false)
        {
            var data = await splitDataService.GetSplitEventsBetweenDates(startDate, endDate, code, loadPositionDate);

            return data;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddBatchJob([FromBody] SplitsOrder splitsOrder)
        {
            await bus.Publish(splitsOrder);

            return Ok();
        }
    }
}
