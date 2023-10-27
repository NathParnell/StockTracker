using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using StockTrackerCommon.Models;

namespace StockTrackerCommon.Database
{

    //Entity Framework Db Context for Stock Tracker
    public class StockTrackerDbContext : DbContext
    {
        public StockTrackerDbContext(DbContextOptions<StockTrackerDbContext> options)
          : base(options)
        {
            this.EnsureSeedData();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        //these commands are need to run
        //Add-Migration InitialCreate
        //Update-Database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(e => e.OrderItemIds)
                .HasConversion(
                    v => string.Join(";", v), // Convert List<string> to a delimited string
                    v => v.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() // Convert string back to List<string>
                );
        }
    }
}
