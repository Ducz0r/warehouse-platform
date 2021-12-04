using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Data.Domain;

namespace Warehouse.Server.Data
{
    public interface IDataContext
    {
        DbSet<Customer> Customers { get; set; }

        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
