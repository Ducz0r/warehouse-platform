using Microsoft.EntityFrameworkCore;
using System;
using Warehouse.Server.Domain;

namespace Warehouse.Server
{
    public class DataContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public string DbPath { get; private set; }

        public DataContext()
        {
            var folder = Environment.SpecialFolder.CommonApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}warehouse.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
