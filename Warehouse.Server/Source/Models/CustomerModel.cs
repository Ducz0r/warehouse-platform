using System;
using Warehouse.Server.Data.Domain;

namespace Warehouse.Server.Models
{
    public class CustomerModel : ICustomerModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public CustomerModel(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Quantity = customer.Quantity;
        }
    }
}
