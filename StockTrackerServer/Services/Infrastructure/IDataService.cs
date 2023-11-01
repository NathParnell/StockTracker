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
        Task<List<Supplier>> GetAllSuppliers();
        Task<List<Customer>> GetAllCustomers();
        Task<List<ProductCategory>> GetAllProductCategories();
        Task<List<Product>> GetAllProducts();
        #endregion
        Task<Supplier> GetSupplierByEmail(string email);
        Task<Supplier> GetSupplierBySupplierId(string supplierID);
        Task<Customer> GetCustomerByEmail(string email);
        Task<Customer> GetCustomerByCustomerId(string customerId);
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
        Task<bool> AddOrder(Order order);
        Task<bool> AddMessage(Message message);
        #endregion

        #region "Update Methods"
        Task<bool> UpdateProduct(Product updatedProduct);
        #endregion
    }
}
