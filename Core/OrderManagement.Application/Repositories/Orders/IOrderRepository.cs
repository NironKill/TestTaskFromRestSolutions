namespace OrderManagement.Application.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task OrderHandling(string currencyRate, CancellationToken cancellationToken);
    }
}
