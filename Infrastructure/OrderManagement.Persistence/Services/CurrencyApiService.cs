using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Repositories.Orders;
using RestSharp;
using System.Net;

namespace OrderManagement.Persistence.Services
{
    public class CurrencyApiService : ICurrencyApiService
    {
        private readonly ILogger<CurrencyApiService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IOrderRepository _repository;

        public CurrencyApiService(ILogger<CurrencyApiService> logger, IMemoryCache memoryCache, IOrderRepository repository)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _repository = repository;
        }

        public async Task BuildingCurrencyConverter(string url, string apiKey, string baseCurrency, string currencies, CancellationToken cancellationToken)
        {
            RestClient client = new RestClient($"{url}?apikey={apiKey}&base_currency={baseCurrency}&currencies={currencies}");

            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("apikey", apiKey);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError($"HTTP {request.Method} https://{response.ResponseUri.Authority} - {response.StatusCode} {response.Content}");
                return;
            }

            if (!_memoryCache.TryGetValue("Currencies", out string currencyRate))
                _memoryCache.Set("Currencies", response.Content, TimeSpan.FromHours(12));

            _logger.LogInformation($"HTTP {request.Method} https://{response.ResponseUri.Authority} - {response.StatusCode} {response.Content}");

            await _repository.OrderHandling(response.Content, cancellationToken);
        }
    }
}
