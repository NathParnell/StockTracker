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
        #region "Stock Methods"
        List<Stock> GetStockBySupplier(string supplierId);
        bool DeleteStockByStockID(string stockId);

        #endregion

        #region "Product Methods"
        List<Product> GetProductsByProductIds(List<string> productIds);
        List<Product> GetAllProducts();
        bool AddProduct(Product product);
        string ValidateAndAddProduct(Product newProduct, List<Product> existingProducts, List<ProductCategory> productCategories);

        #endregion

        #region "Product Category Methods"
        List<ProductCategory> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds);
        List<ProductCategory> GetAllProductCategories();

        #endregion

    }
}
