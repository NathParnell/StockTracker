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
        List<Stock> GetStockBySupplier(string supplierId);
        List<Product> GetProductsByProductIds(List<string> productIds);
        List<ProductCategory> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds);
    }
}
