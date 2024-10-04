using MediatR;

namespace OrderManagement.Application.Requests.Orders.Read.GetAll
{
    public class GetAllOrderRequest : IRequest<List<GetAllOrderResponse>>
    {
    }
}
