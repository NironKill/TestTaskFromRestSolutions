using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Requests.Orders.Write.Put
{
    public class PutOrderValidator : AbstractValidator<PutOrderRequest>
    {
        private readonly IApplicationDbContext _context;

        public PutOrderValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(request => request.CustomerName).NotEmpty().WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");
            RuleFor(request => request.CustomerName).Length(3, 30).WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");

            RuleFor(request => request.Id).NotEmpty().WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");

            RuleFor(request => request.Id).MustAsync(OrderExists)
                .WithMessage("There is no such order")
                .WithErrorCode($"{StatusCode.NotFound.GetHashCode()}");
        }

        private async Task<bool> OrderExists(Guid id, CancellationToken cancellation) =>
           await _context.Order.AnyAsync(x => x.Id == id);
    }
}
