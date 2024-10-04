namespace OrderManagement.Application.Requests.Orders.Write.Patch
{
    public class PatchOrderResponse
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
