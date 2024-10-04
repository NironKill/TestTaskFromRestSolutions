namespace OrderManagement.Application.Requests.Orders.Write.Delete
{
    public class DeleteOrderResponse
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
    }
}
