using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Server.Auth;
using Warehouse.Server.Data;
using Warehouse.Server.Data.Domain;

namespace Warehouse.Server.Tests.Utils
{
    public static class IDataContextSeedingExtensions
    {
        public static async Task<Customer> SeedSingleCustomer(this IDataContext dataContext)
        {
            dataContext.Customers.RemoveRange(dataContext.Customers);
            dataContext.SaveChanges();

            Customer seededCustomer = new Customer() { Id = Guid.NewGuid(), Name = TestConstants.TEST_CUSTOMER_NAME, Quantity = 0 };
            seededCustomer.AssignPassword(TestConstants.TEST_CUSTOMER_PASSWORD);
            await dataContext.Customers.AddAsync(seededCustomer);
            await dataContext.SaveChangesAsync();

            return seededCustomer;
        }

        public static async Task<IList<Customer>> SeedMultipleCustomers(this IDataContext dataContext)
        {
            dataContext.Customers.RemoveRange(dataContext.Customers);
            dataContext.SaveChanges();

            List<Customer> seededCustomers = new List<Customer>()
                {
                    new Customer() {Id = Guid.NewGuid(), Name = TestConstants.TEST_CUSTOMER_NAME, Quantity = 0},
                    new Customer() {Id = Guid.NewGuid(), Name = "Evergreen", Quantity = 0},
                    new Customer() {Id = Guid.NewGuid(), Name = "Luka Koper", Quantity = 0}
                };
            seededCustomers[0].AssignPassword(TestConstants.TEST_CUSTOMER_PASSWORD);
            seededCustomers[1].AssignPassword("evergreen-pass");
            seededCustomers[2].AssignPassword("koper-pass");
            await dataContext.Customers.AddRangeAsync(seededCustomers, CancellationToken.None);
            await dataContext.SaveChangesAsync();

            return seededCustomers;
        }
    }
}
