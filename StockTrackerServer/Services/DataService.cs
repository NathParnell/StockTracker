﻿using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Supplier>> GetAllSuppliers() => await _context.Suppliers.ToListAsync();
        public async Task<List<Customer>> GetAllCustomers() => await _context.Customers.ToListAsync();
        public async Task<List<ProductCategory>> GetAllProductCategories() => await _context.ProductCategories.ToListAsync();
        public async Task<List<Product>> GetAllProducts() => await _context.Products.ToListAsync();
        #endregion

        public async Task<Supplier> GetSupplierByEmail(string email)
        {
            try
            {
                return await _context.Suppliers.FirstOrDefaultAsync(supp => supp.Email == email);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Supplier> GetSupplierBySupplierId(string supplierId)
        {
            try
            {
                return await _context.Suppliers.FirstOrDefaultAsync(supp => supp.SupplierId == supplierId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Supplier>> GetSuppliersBySupplierIds(List<string> supplierIds)
        {
            try
            {
                return await _context.Suppliers.Where(supp => supplierIds.Contains(supp.SupplierId.ToString())).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(cust => cust.Email == email);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Customer> GetCustomerByCustomerId(string customerId)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(cust => cust.CustomerId == customerId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Customer>> GetCustomersByCustomerIds(List<string> customerIds)
        {
            try
            {
                return await _context.Customers.Where(cust => customerIds.Contains(cust.CustomerId.ToString())).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Product> GetProductByProductId(string productId)
        {
            try
            {
                return await _context.Products.FirstOrDefaultAsync(prod => prod.ProductId == productId);
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

        public async Task<List<string>> GetContactIds(string userId)
        {
            try
            {
                var userIDs = _context.Messages
                    .Where(message => message.ReceiverId == userId || message.SenderId == userId)
                    .Select(message => message.ReceiverId == userId ? message.SenderId : message.ReceiverId)
                    .Distinct()
                    .ToList();

                return userIDs;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Message>> GetMessageThreads(string userId, string contactId)
        {
            try
            {
                var messages = await _context.Messages
                    .Where(mess => (mess.SenderId == userId && mess.ReceiverId == contactId) || (mess.SenderId == contactId && mess.ReceiverId == userId))
                    .OrderBy(mess => mess.SentTime)
                    .ToListAsync();

                return messages;
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

        public async Task<bool> AddProductCategory(ProductCategory productCategory)
        {
            try
            {
                _context.ProductCategories.Add(productCategory);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddMessage(Message message)
        {
            try
            {
                _context.Messages.Add(message);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region "Update Methods"

        public async Task<bool> UpdateProduct(Product updatedProduct)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(prod => prod.ProductId == updatedProduct.ProductId);
                if (product != null)
                {
                    //update product values 
                    product.ProductName = updatedProduct.ProductName;
                    product.ProductBrand = updatedProduct.ProductBrand;
                    product.ProductQuantity = updatedProduct.ProductQuantity;
                    product.ProductMeasurementUnit = updatedProduct.ProductMeasurementUnit;
                    product.ProductCategoryId = updatedProduct.ProductCategoryId;
                    product.ProductSize = updatedProduct.ProductSize;
                    product.Price = updatedProduct.Price;
                    product.SupplierId = updatedProduct.SupplierId;
                    product.ProductCode = updatedProduct.ProductCode;

                    _context.Entry(product).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

    }
}
