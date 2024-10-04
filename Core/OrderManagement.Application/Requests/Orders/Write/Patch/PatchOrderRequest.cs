using MediatR;

namespace OrderManagement.Application.Requests.Orders.Write.Patch
{
    public class PatchOrderRequest : IRequest<PatchOrderResponse>
    {
        public Guid Id { get; set; }
    }
}
