using AnotherStockMarketService.Api.Consumers;
using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AnotherStockMarketService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistributionsController(IBus bus) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Distribution>> Get([FromQuery(Name = "code")] string? code, [FromQuery(Name = "startDate")] DateTime startDate, [FromQuery(Name = "endDate")] DateTime endDate, [FromServices] IDistributionDataService distributionDataService, [FromQuery(Name = "loadPositionDate")] bool loadPositionDate = false)
        {
            var data = await distributionDataService.GetCodeDistributionsBetweenDates(startDate, endDate, code, loadPositionDate);

            return data;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddBatchJob([FromBody] DividendsOrder dividendsOrder)
        {
            await bus.Publish(dividendsOrder);

            return Ok();
        }
    }
}
