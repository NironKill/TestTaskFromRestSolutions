using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OrderManagement.Application.Enums;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Models;
using OrderManagement.Domain;

namespace OrderManagement.Application.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IApplicationDbContext _context;

        public OrderRepository(IApplicationDbContext context, ILogger<OrderRepository> logger) => _context = context;


        public async Task OrderHandling(string currencyRate, CancellationToken cancellationToken)
        {
            List<Order> listOrders = await _context.Order
                .Where(x => x.Status != (int)Status.Completed && x.Status != (int)Status.Cancelled)
                .OrderByDescending(x => x.Priority)
                .ToListAsync();

            JObject parse = JObject.Parse(currencyRate);
            JObject currencyData = (JObject)parse["data"];

            Dictionary<string, decimal> currencies = new Dictionary<string, decimal>();
            foreach (var currency in currencyData)
            {
                string currencyCode = currency.Key;  
                decimal currencyValue = (decimal)currency.Value["value"];  

                currencies[currencyCode] = currencyValue;
            }

            int currentTime = Convert.ToInt32(DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds);
            List<CompletedOrder> listCompletedOrders = new List<CompletedOrder>();
            foreach (Order order in listOrders)
            {
                Currency currencyOrder = (Currency)order.Currency;

                if (order.Status == (int)Status.Pending)
                {
                    decimal currentCurrencyRate = decimal.Zero;
                    foreach (var data in currencies)
                    {
                        if (data.Key == currencyOrder.ToString())
                        {
                            currentCurrencyRate = data.Value;
                            break;
                        }
                    }
                    int minutesElapsed = (currentTime - order.OrderDate) / 60;

                    int addingPriorityEveryMinute = default(int);
                    if (minutesElapsed > 10)
                        addingPriorityEveryMinute = minutesElapsed;

                    int hoursElapsed = default(int);
                    if (minutesElapsed > 60)
                        hoursElapsed = minutesElapsed / 60;

                    order.TotalAmountInBaseCurrency = (int)Math.Ceiling((double)order.TotalAmount / (double)currentCurrencyRate);

                    order.Priority = (int)Math.Ceiling((double)order.TotalAmountInBaseCurrency + (double)addingPriorityEveryMinute + (double)hoursElapsed);
                    order.Status = (int)Status.Processing;

                    _context.Order.Update(order);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                else if (order.Status == (int)Status.Processing)
                {
                    order.Status = (int)Status.Completed;

                    _context.Order.Update(order);
                    await _context.SaveChangesAsync(cancellationToken);

                    CompletedOrder completedOrder = new CompletedOrder
                    {
                        Id = order.Id,
                        CurrentTime = DateTime.UnixEpoch.AddSeconds(currentTime).ToLocalTime(),
                        CustomerName = order.CustomerName,
                        TotalAmount = order.TotalAmount,
                        TotalAmountInBaseCurrency = order.TotalAmountInBaseCurrency,
                        Currency = currencyOrder.ToString(),
                        BaseCurrency = Currency.USD.ToString()
                    };
                    listCompletedOrders.Add(completedOrder);
                }
            }
            using (StreamWriter writer = new StreamWriter("completed_orders.txt", true))
            {
                foreach (CompletedOrder completedOrder in listCompletedOrders)
                {
                    string logEntry = $"Time: {completedOrder.CurrentTime}, Id: {completedOrder.Id}, CustomerName: {completedOrder.CustomerName}, " +
                                      $"TotalAmount: {completedOrder.TotalAmount} {completedOrder.Currency}, " +
                                      $"TotalAmountInBaseCurrency: {completedOrder.TotalAmountInBaseCurrency} {completedOrder.BaseCurrency}"; 

                    await writer.WriteLineAsync(logEntry);
                }
            }
        }
    }
}
