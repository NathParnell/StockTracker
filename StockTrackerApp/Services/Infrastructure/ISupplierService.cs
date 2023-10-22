using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface ISupplierService
    {
        #region "Product Methods"
        List<Product> GetProductBySupplierId(string supplierId);
        bool DeleteProductByProductID(string productId);
        Product GetProductByProductId(string productId);
        List<Product> GetProductsByProductIds(List<string> productIds);
        List<Product> GetAllProducts();
        bool AddProduct(Product product);
        string ValidateAndAddProduct(Product newProduct, List<Product> existingProducts, List<ProductCategory> productCategories);
        bool UpdateProduct(Product updatedProduct);
        string ValidateAndUpdateProduct(Product updatedProduct, List<Product> suppliersExistingProducts, List<ProductCategory> productCategories);

        #endregion

        #region "Product Category Methods"
        List<ProductCategory> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds);
        List<ProductCategory> GetAllProductCategories();
        bool AddProductCategory(ProductCategory productCategory);
        string ValidateAndAddProductCategory(ProductCategory productCategory, List<ProductCategory> existingProductCategories);

        #endregion

    }
}
