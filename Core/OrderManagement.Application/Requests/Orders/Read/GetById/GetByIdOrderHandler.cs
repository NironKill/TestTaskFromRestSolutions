using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;

namespace OrderManagement.Application.Requests.Orders.Read.GetById
{
    public class GetByIdOrderHandler : IRequestHandler<GetByIdOrderRequest, GetByIdOrderResponse>
    {
        private readonly IApplicationDbContext _context;

        public GetByIdOrderHandler(IApplicationDbContext context) => _context = context;

        public async Task<GetByIdOrderResponse> Handle(GetByIdOrderRequest request, CancellationToken cancellationToken)
        {
            Order order = await _context.Order.FirstOrDefaultAsync(x => x.Id == request.Id);

            Currency currency = (Currency)order.Currency;
            Status status = (Status)order.Status;
            DateTime orderDate = DateTime.UnixEpoch.AddSeconds(order.OrderDate).ToLocalTime();

            GetByIdOrderResponse response = new GetByIdOrderResponse
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

            return response;
        }
    }
}
