using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IProductService
    {
        #region "Get Methods"
        List<Product> GetProductBySupplierId(string supplierId);
        Product GetProductByProductId(string productId);
        List<Product> GetProductsByProductIds(List<string> productIds);
        List<Product> GetAllProducts();
        #endregion

        #region "Add Methods"
        bool AddProduct(Product product);
        #endregion

        #region "Update Methods"
        bool UpdateProduct(Product updatedProduct);
        #endregion

        #region "Delete Methods"
        bool DeleteProductByProductID(string productId);
        #endregion

        #region "Validation Methods"
        string ValidateAndAddProduct(Product newProduct, List<Product> existingProducts, List<ProductCategory> productCategories, ref bool success);
        string ValidateAndUpdateProduct(Product updatedProduct, List<Product> suppliersExistingProducts, List<ProductCategory> productCategories, ref bool success);
        #endregion
    }
}
