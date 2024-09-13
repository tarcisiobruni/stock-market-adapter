using AnotherStockMarketService.Api.Consumers;
using AnotherStockMarketService.Api.Models;
using AnotherStockMarketService.Api.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AnotherStockMarketService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotationsController(IBus bus) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Quotation>> Get([FromQuery(Name = "code")] string code, [FromQuery(Name = "startDate")] DateTime startDate, [FromQuery(Name = "endDate")] DateTime endDate, [FromServices] IQuotationDataService quotationDataService)
        {
            var data = await quotationDataService.GetCodePricesBetweenDates(code, startDate, endDate);

            return data;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddBatchJob([FromBody] QuotationOrder quotationOrder)
        {
            await bus.Publish(quotationOrder);

            return Ok();
        }
    }
}
