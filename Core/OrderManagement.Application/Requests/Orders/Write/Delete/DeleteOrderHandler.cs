using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain;

namespace OrderManagement.Application.Requests.Orders.Write.Delete
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderRequest, DeleteOrderResponse>
    {
        private readonly IApplicationDbContext _context;

        public DeleteOrderHandler(IApplicationDbContext context) => _context = context;

        public async Task<DeleteOrderResponse> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
        {
            Order order = await _context.Order.FirstOrDefaultAsync(x => x.Id == request.Id);
            Status status = (Status)order.Status;

            DeleteOrderResponse response = new DeleteOrderResponse
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                Status = status.ToString()
            };

            _context.Order.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
