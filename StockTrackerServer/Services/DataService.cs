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

        #region "Get Methods"
        #region "Get All Methods"
        public async Task<List<User>> GetAllUsers() => await _context.Users.ToListAsync();
        public async Task<List<ProductCategory>> GetAllProductCategories() => await _context.ProductCategories.ToListAsync();
        public async Task<List<Product>> GetAllProducts() => await _context.Products.ToListAsync();
        #endregion

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

        public async Task<List<Stock>> GetStockBySupplierId(string supplierId)
        {
            try
            {
                return await _context.Stock.Where(stock => stock.SupplierId == supplierId).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Product>> GetProductsByProductIds(List<string> productIds)
        {
            try
            {
                return await _context.Products.Where(prod => productIds.Contains(prod.ProductId.ToString())).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ProductCategory>> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds)
        {
            try
            {
                return await _context.ProductCategories.Where(cat => productCategoryIds.Contains(cat.ProductCategoryId.ToString())).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region "Delete Methods"
        
        public async Task<bool> DeleteStockByStockId(string stockId)
        {
            var itemToDelete = _context.Stock.Find(stockId); // Replace YourEntities with your actual DbSet name

            if (itemToDelete != null)
            {
                _context.Stock.Remove(itemToDelete);
                _context.SaveChanges(); // Save the changes to delete the entity
                return true;
            }
            return false;
        }
        
        #endregion
    }
}
