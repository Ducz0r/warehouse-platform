using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Data.Domain;

namespace Warehouse.Server.Data
{
    public interface IDataContext
    {
        DbSet<Customer> Customers { get; set; }
    }
}
