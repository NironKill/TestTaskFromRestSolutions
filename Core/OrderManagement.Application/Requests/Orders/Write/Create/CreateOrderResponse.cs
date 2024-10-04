namespace OrderManagement.Application.Requests.Orders.Write.Create
{
    public class CreateOrderResponse
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
    }
}
