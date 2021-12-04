using System;

namespace Warehouse.Server.Data.Domain
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Quantity { get; set; }

        public void IncreaseQuantity(int increase)
        {
            Quantity = Quantity + increase;
        }
    }
}
