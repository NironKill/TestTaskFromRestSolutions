namespace OrderManagement.Persistence.Settings
{
    public class CurrencyApiSet
    {
        public static readonly string Configuration = "CurrencyApi";

        public string URL { get; set; } = string.Empty;
        public string APIKey { get; set; } = string.Empty;
        public string BaseCurrency { get; set; } = string.Empty;
        public string Currencies { get; set; } = string.Empty;
    }
}