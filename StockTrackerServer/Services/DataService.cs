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

        public async Task<List<Product>> GetProductsBySupplierId(string supplierId)
        {
            try
            {
                return await _context.Products.Where(prod => prod.SupplierId == supplierId).ToListAsync();
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
        
        public async Task<bool> DeleteProductByProductId(string productId)
        {
            var itemToDelete = _context.Products.Find(productId);

            if (itemToDelete != null)
            {
                _context.Products.Remove(itemToDelete);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion

        #region "Add Methods"
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
