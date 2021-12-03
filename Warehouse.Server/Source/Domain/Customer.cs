using System;

namespace Warehouse.Server.Domain
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public int Quantity { get; set; }
    }
}
