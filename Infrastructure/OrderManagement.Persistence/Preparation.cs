using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Persistence
{
    public class Preparation
    {
        public static async Task Initialize(ApplicationDbContext context) => await context.Database.MigrateAsync();
    }
}
