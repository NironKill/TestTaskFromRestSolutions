using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;

namespace OrderManagement.Application.Requests.Orders.Write.Put
{
    public class PutOrderHandler : IRequestHandler<PutOrderRequest, PutOrderResponse>
    {
        private readonly IApplicationDbContext _context;

        public PutOrderHandler(IApplicationDbContext context) => _context = context;

        public async Task<PutOrderResponse> Handle(PutOrderRequest request, CancellationToken cancellationToken)
        {
            Order updateOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == request.Id);

            updateOrder.CustomerName = request.CustomerName;

            _context.Order.Update(updateOrder);
            await _context.SaveChangesAsync(cancellationToken);

            PutOrderResponse response = new PutOrderResponse
            {
                Id = updateOrder.Id,
                CustomerName = updateOrder.CustomerName
            };

            return response;
        }
    }
}
