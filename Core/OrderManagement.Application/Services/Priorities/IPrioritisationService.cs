using OrderManagement.Application.Enums;

namespace OrderManagement.Application.Services.Priorities
{
    public interface IPrioritisationService
    {
        int PriorityCalculationForCreation(decimal totalAmount, Currency currency);
    }
}
