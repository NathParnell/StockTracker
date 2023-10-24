using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services.Infrastructure
{
    public interface IDataService
    {
        #region "Get Methods"
        #region "Get All Methods"
        Task<List<User>> GetAllUsers();
        Task<List<ProductCategory>> GetAllProductCategories();
        Task<List<Product>> GetAllProducts();
        #endregion
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByUserId(string userId);
        Task<List<User>> GetUsersByUserType(UserType userType);
        Task<Product> GetProductByProductId(string productId);
        Task<List<Product>> GetProductsBySupplierId(string supplierId);
        Task<List<Product>> GetProductsByProductIds(List<string> productIds);
        Task<List<ProductCategory>> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds);
        #endregion

        #region "Delete Methods"
        Task<bool> DeleteProductByProductId(string productId);
        #endregion

        #region "Add Methods"
        Task<bool> AddProduct(Product product);
        Task<bool> AddProductCategory(ProductCategory productCategory);
        #endregion

        #region "Update Methods"
        Task<bool> UpdateProduct(Product updatedProduct);
        #endregion
    }
}
