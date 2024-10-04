using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain;

namespace OrderManagement.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Order { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
