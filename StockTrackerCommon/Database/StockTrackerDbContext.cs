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
        public DbSet<Message> Messages { get; set; }


        /// <summary>
        /// This code is what allows us to store a list of strings in the Order table
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(e => e.OrderItemIds)
                .HasConversion(
                    v => string.Join(";", v), // Convert List<string> to a delimited string
                    v => v.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() // Convert string back to List<string>
                );

            modelBuilder.Entity<Customer>()
                .Property(e => e.ProductSubscriptions)
                .HasConversion(
                    v => string.Join(";", v), // Convert List<string> to a delimited string
                    v => v.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() // Convert string back to List<string>
                );

            modelBuilder.Entity<Customer>()
                .Property(e => e.SupplierSubscriptions)
                .HasConversion(
                    v => string.Join(";", v), // Convert List<string> to a delimited string
                    v => v.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList() // Convert string back to List<string>
                );
        }
    }
}
