using MediatR;

namespace OrderManagement.Application.Requests.Orders.Write.Delete
{
    public class DeleteOrderRequest : IRequest<DeleteOrderResponse>
    {
        public Guid Id { get; set; }
    }
}
