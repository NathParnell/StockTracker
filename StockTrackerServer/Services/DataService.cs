using Microsoft.EntityFrameworkCore;
using StockTrackerCommon.Database;
using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;

namespace StockTrackerServer.Services
{
    public class DataService : IDataService
    {
        private readonly StockTrackerDbContext _context;
        public DataService()
        {
            var options = new DbContextOptionsBuilder<StockTrackerDbContext>()
                .UseInMemoryDatabase("StockTracker")
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            _context = new StockTrackerDbContext(options);
        }

        public async Task<IEnumerable<User>> GetAllUsers() => await _context.Users.ToListAsync();

        public async Task<User> GetUserByUsername(string username)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
