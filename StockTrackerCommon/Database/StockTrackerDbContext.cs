using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

    }
}
