using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using OrderManagement.Application.Enums;

namespace OrderManagement.Application.Services.Priorities
{
    public class PrioritisationService : IPrioritisationService
    {
        private readonly IMemoryCache _memoryCache;

        public PrioritisationService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Priority calculation when creating an order
        /// </summary>
        public int PriorityCalculationForCreation(decimal totalAmount, Currency currency)
        {
            int result = default(int);
            string currencyRate = _memoryCache.Get<string>("Currencies");

            if (string.IsNullOrEmpty(currencyRate))
            {
                switch (currency)
                {
                    case Currency.BYN:
                        result = (int)Math.Ceiling((double)totalAmount / 3.26);
                        break;

                    case Currency.PLN:
                        result = (int)Math.Ceiling((double)totalAmount / 3.89);
                        break;

                    case Currency.RUB:
                        result = (int)Math.Ceiling((double)totalAmount / 94.32);
                        break;

                    case Currency.EUR:
                        result = (int)Math.Ceiling((double)totalAmount / 0.9);
                        break;

                    default:
                        result = (int)totalAmount / 10;
                        break;
                }
                return result;
            }

            JObject parse = JObject.Parse(currencyRate);
            JObject currencyData = (JObject)parse["data"];

            decimal currencyValue = decimal.Zero;
            foreach (var data in currencyData)
            {
                if (data.Key == currency.ToString())
                {
                    currencyValue = (decimal)data.Value["value"];
                    break;
                }
            }
            result = (int)Math.Ceiling((double)totalAmount / (double)currencyValue);
            return result;
        }
    }
}
