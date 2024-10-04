using MediatR;

namespace OrderManagement.Application.Requests.Orders.Read.GetById
{
    public class GetByIdOrderRequest : IRequest<GetByIdOrderResponse>
    {
        public Guid Id { get; set; }
    }
}
