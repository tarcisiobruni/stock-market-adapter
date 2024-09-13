using System.Text;
using System.Text.Json;

namespace AnotherStockMarketService.Api.Consumers
{
    public static class HttpRequestHelper
    {
        public static HttpRequestMessage PrepareRequestMessage<T>(IConfiguration configuration, List<T> list, string path)
        {
            var destinationUrlBase = configuration.GetValue<string>("DestinationService:baseUrl");
            var destinationCustomHeaderName = configuration.GetValue<string>("DestinationService:headerName");
            var destinationCustomHeaderValue = configuration.GetValue<string>("DestinationService:headerValue");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{destinationUrlBase}/{path}");
            request.Headers.Add(destinationCustomHeaderName, destinationCustomHeaderValue);

            var content = new StringContent(JsonSerializer.Serialize(list), Encoding.UTF8, "application/json");

            request.Content = content;

            return request;
        }
    }
}
