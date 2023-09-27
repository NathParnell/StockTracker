using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockTrackerCommon.Models;

namespace StockTrackerCommon.Database
{
    public class StockTrackerDbContext : DbContext
    {
        public StockTrackerDbContext(DbContextOptions<StockTrackerDbContext> options)
          : base(options)
        {
            this.EnsureSeedData();
        }

        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "AuthorDb");
        }


        public DbSet<User> Users { get; set; }
    }
}
