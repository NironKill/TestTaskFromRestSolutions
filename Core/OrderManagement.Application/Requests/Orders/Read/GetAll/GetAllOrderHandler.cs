using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;

namespace OrderManagement.Application.Requests.Orders.Read.GetAll
{
    public class GetAllOrderHandler : IRequestHandler<GetAllOrderRequest, List<GetAllOrderResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllOrderHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<GetAllOrderResponse>> Handle(GetAllOrderRequest request, CancellationToken cancellationToken)
        {
            List<Order> listOrder = await _context.Order.ToListAsync(cancellationToken);

            List<GetAllOrderResponse> responses = new List<GetAllOrderResponse>();
            foreach (Order order in listOrder)
            {
                Currency currency = (Currency)order.Currency;
                Status status = (Status)order.Status;
                DateTime orderDate = DateTime.UnixEpoch.AddSeconds(order.OrderDate).ToLocalTime();

                GetAllOrderResponse response = new GetAllOrderResponse
                {
                    Id = order.Id,
                    CustomerName = order.CustomerName,
                    OrderDate = orderDate,
                    TotalAmount = order.TotalAmount,
                    Currency = currency.ToString(),
                    Priority = order.Priority,
                    Status = status.ToString(),
                    TotalAmountInBaseCurrency = order.TotalAmountInBaseCurrency
                };
                responses.Add(response);
            }
            return responses;
        }
    }
}
