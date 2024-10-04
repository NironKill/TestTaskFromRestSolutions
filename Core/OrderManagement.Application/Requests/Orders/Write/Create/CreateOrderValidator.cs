using FluentValidation;
using OrderManagement.Application.Enums;

namespace OrderManagement.Application.Requests.Orders.Write.Create
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(request => request.CustomerName).NotEmpty().WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");
            RuleFor(request => request.CustomerName).Length(3, 30).WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");

            RuleFor(request => request.TotalAmount).NotEmpty().WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");
            RuleFor(request => request.TotalAmount).GreaterThan(decimal.Zero).WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");

            RuleFor(request => request.Currency).NotEmpty().WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");
            RuleFor(request => request.Currency).Must(CurrencyValidation)
                .WithMessage("I'm sorry, but this currency is not supported by our service.")
                .WithErrorCode($"{StatusCode.BadRequest.GetHashCode()}");
        }

        private bool CurrencyValidation(string currency)
        {
            Currency actualCurrency;
            bool result = Enum.TryParse(currency, true, out actualCurrency);

            return result;
        }
    }
}
