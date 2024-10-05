using MediatR;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services.Priorities;
using OrderManagement.Domain;

namespace OrderManagement.Application.Requests.Orders.Write.Create
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPrioritisationService _prioritisationService;

        public CreateOrderHandler(IApplicationDbContext context, IPrioritisationService prioritisationService)
        {
            _context = context;
            _prioritisationService = prioritisationService;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            int orderCreationTS = Convert.ToInt32(DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds);
            Currency currency = (Currency)Enum.Parse(typeof(Currency), request.Currency, true);

            int priority = default(int);
            int status = default(int);
            string strStatus = string.Empty;
            decimal totalAmountInBaseCurrency = decimal.Zero;
            decimal totalAmount = Math.Round(request.TotalAmount, 2, MidpointRounding.AwayFromZero);
            if (currency == Currency.USD)
            {
                totalAmountInBaseCurrency = totalAmount;
                priority = (int)totalAmountInBaseCurrency;
                status = (int)Status.Processing;
                strStatus = Status.Processing.ToString();
            }
            else
            {
                status = (int)Status.Pending;
                strStatus = Status.Pending.ToString();
                priority = _prioritisationService.PriorityCalculationForCreation(request.TotalAmount, currency);
            }

            Order newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = request.CustomerName,
                TotalAmount = totalAmount,
                Currency = (int)currency,
                OrderDate = orderCreationTS,
                Priority = priority,
                Status = status,
                TotalAmountInBaseCurrency = totalAmountInBaseCurrency
            };
            await _context.Order.AddAsync(newOrder, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            DateTime orderDate = DateTime.UnixEpoch.AddSeconds(orderCreationTS).ToLocalTime();

            CreateOrderResponse response = new CreateOrderResponse
            {
                Id = newOrder.Id,
                CustomerName = request.CustomerName,
                TotalAmount = totalAmount,
                Currency = currency.ToString(),
                OrderDate = orderDate,
                Priority = priority,
                Status = strStatus,
            };

            return response;
        }
    }
}
