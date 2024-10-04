using MediatR;

namespace OrderManagement.Application.Requests.Orders.Write.Put
{
    public class PutOrderRequest : IRequest<PutOrderResponse>
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
    }
}
