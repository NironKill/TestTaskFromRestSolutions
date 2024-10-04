namespace OrderManagement.Application.Models
{
    public class CompletedOrder
    {
        public Guid Id { get; set; }
        public DateTime CurrentTime { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public decimal TotalAmountInBaseCurrency { get; set; }
        public string BaseCurrency { get; set; }
    }
}
