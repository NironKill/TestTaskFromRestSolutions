using MediatR;

namespace OrderManagement.Application.Requests.Orders.Write.Create
{
    public class CreateOrderRequest : IRequest<CreateOrderResponse>
    {
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
    }
}
