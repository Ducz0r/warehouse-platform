using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data.Domain;

namespace Warehouse.Server.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
