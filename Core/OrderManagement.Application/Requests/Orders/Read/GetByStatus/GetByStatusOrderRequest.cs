using MediatR;

namespace OrderManagement.Application.Requests.Orders.Read.GetByStatus
{
    public class GetByStatusOrderRequest : IRequest<List<GetByStatusOrderResponse>>
    {
        public int Status { get; set; }
    }
}
