namespace OrderManagement.Application.Requests.Orders.Read.GetById
{
    public class GetByIdOrderResponse
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public decimal TotalAmountInBaseCurrency { get; set; }
    }
}
