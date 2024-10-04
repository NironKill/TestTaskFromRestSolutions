using FluentValidation;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Requests.Orders.Read.GetByStatus
{
    public class GetByStatusOrderValidator : AbstractValidator<GetByStatusOrderRequest>
    {
        private readonly IApplicationDbContext _context;

        public GetByStatusOrderValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(request => request.Status).NotEmpty().WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");

            RuleFor(request => request.Status).Must(StatusValidation)
                .WithMessage("Sorry, there is no such status.")
                .WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");
        }

        private bool StatusValidation(int status)
        {
            bool result = Enum.IsDefined(typeof(Status), status);
            return result;
        }
    }
}
