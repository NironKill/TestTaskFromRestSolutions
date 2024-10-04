using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;

namespace OrderManagement.Application.Requests.Orders.Write.Patch
{
    public class PatchOrderHandler : IRequestHandler<PatchOrderRequest, PatchOrderResponse>
    {
        private readonly IApplicationDbContext _context;

        public PatchOrderHandler(IApplicationDbContext context) => _context = context;

        public async Task<PatchOrderResponse> Handle(PatchOrderRequest request, CancellationToken cancellationToken)
        {
            Order updateOrder = await _context.Order.FirstOrDefaultAsync(x => x.Id == request.Id);

            updateOrder.Status = (int)Status.Cancelled;

            _context.Order.Update(updateOrder);
            await _context.SaveChangesAsync(cancellationToken);

            PatchOrderResponse response = new PatchOrderResponse
            {
                Id = updateOrder.Id,
                Status = Status.Cancelled.ToString()
            };

            return response;
        }
    }
}
