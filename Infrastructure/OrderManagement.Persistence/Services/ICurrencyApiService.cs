namespace OrderManagement.Persistence.Services
{
    public interface ICurrencyApiService
    {
        Task BuildingCurrencyConverter(string url, string apiKey, string baseCurrency, string currencies, CancellationToken cancellationToken);
    }
}
