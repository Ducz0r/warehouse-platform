using System;

namespace Warehouse.ClientApp.Models
{
    public interface ICustomerModel
    {
        Guid Id { get; set; }
        string Name { get; set; }
        int Quantity { get; set; }
    }
}
