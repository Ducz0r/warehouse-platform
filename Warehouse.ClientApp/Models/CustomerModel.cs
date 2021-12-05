using System;

namespace Warehouse.ClientApp.Models
{
    public class CustomerModel : ICustomerModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
