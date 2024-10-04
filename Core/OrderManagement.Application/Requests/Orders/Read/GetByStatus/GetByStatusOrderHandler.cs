using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;

namespace OrderManagement.Application.Requests.Orders.Read.GetByStatus
{
    public class GetByStatusOrderHandler : IRequestHandler<GetByStatusOrderRequest, List<GetByStatusOrderResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetByStatusOrderHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<GetByStatusOrderResponse>> Handle(GetByStatusOrderRequest request, CancellationToken cancellationToken)
        {
            List<Order> listOrder = await _context.Order.Where(x => x.Status == request.Status).ToListAsync(cancellationToken);

            List<GetByStatusOrderResponse> responses = new List<GetByStatusOrderResponse>();
            foreach (Order order in listOrder)
            {
                Currency currency = (Currency)order.Currency;
                Status status = (Status)order.Status;
                DateTime orderDate = DateTime.UnixEpoch.AddSeconds(order.OrderDate).ToLocalTime();

                GetByStatusOrderResponse response = new GetByStatusOrderResponse
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
