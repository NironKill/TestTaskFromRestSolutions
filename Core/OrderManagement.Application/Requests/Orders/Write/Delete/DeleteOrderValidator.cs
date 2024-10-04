using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Requests.Orders.Write.Delete
{
    public class DeleteOrderValidator : AbstractValidator<DeleteOrderRequest>
    {
        private readonly IApplicationDbContext _context;

        public DeleteOrderValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(request => request.Id).NotEmpty().WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");

            RuleFor(request => request.Id).MustAsync(OrderExists)
                .WithMessage("There is no such order")
                .WithErrorCode($"{StatusCode.NotFound.GetHashCode()}");
        }

        private async Task<bool> OrderExists(Guid id, CancellationToken cancellation) =>
            await _context.Order.AnyAsync(x => x.Id == id);
    }
}
