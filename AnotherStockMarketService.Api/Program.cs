using AnotherStockMarketService.Api.Consumers;
using AnotherStockMarketService.Api.Services;
using AnotherStockMarketService.Api.Services.Impl;
using AnotherStockMarketService.Api.ThirdPartyServices.StockMarketAPI;
using MassTransit;

namespace AnotherStockMarketService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IQuotationDataService, QuotationDataService>();
            builder.Services.AddScoped<ISplitDataService, SplitDataService>();
            builder.Services.AddScoped<IDistributionDataService, DistributionDataService>();

            builder.Services.AddScoped<StockMarketService>();


            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<QuotationOrderConsumer>();
                x.AddConsumer<DividendsOrderConsumer>();
                x.AddConsumer<SplitsOrderConsumer>();

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.ConcurrentMessageLimit = 1;
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
