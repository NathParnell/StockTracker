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
        Task<List<Supplier>> GetSuppliersBySupplierIds(List<string> supplierIds);
        Task<Customer> GetCustomerByEmail(string email);
        Task<Customer> GetCustomerByCustomerId(string customerId);
        Task<List<Customer>> GetCustomersByCustomerIds(List<string> customerIds);
        Task<Product> GetProductByProductId(string productId);
        Task<List<Product>> GetProductsBySupplierId(string supplierId);
        Task<List<Product>> GetProductsByProductIds(List<string> productIds);
        Task<List<ProductCategory>> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds);
        Task<List<Order>> GetOrderRequestsBySupplierId(string supplierId);
        Task<List<OrderItem>> GetOrderItemsByOrderItemIds(List<string> orderItemIds);
        Task<List<string>> GetContactIds(string userId);
        Task<List<Message>> GetMessageThreads(string userId, string contactId);
        #endregion

        #region "Delete Methods"
        Task<bool> DeleteProductByProductId(string productId);
        #endregion

        #region "Add Methods"
        Task<bool> AddProduct(Product product);
        Task<bool> AddProductCategory(ProductCategory productCategory);
        Task<bool> AddOrder(Order order);
        Task<bool> AddOrderItems(List<OrderItem> orderItems);
        Task<bool> AddMessage(Message message);
        #endregion

        #region "Update Methods"
        Task<bool> UpdateProduct(Product updatedProduct);
        #endregion
    }
}
