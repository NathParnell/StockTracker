﻿using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface ISupplierService
    {
        #region "Get Methods"
        List<Stock> GetStockBySupplier(string supplierId);
        List<Product> GetProductsByProductIds(List<string> productIds);
        List<Product> GetAllProducts();
        List<ProductCategory> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds);
        List<ProductCategory> GetAllProductCategories();

        #endregion

        #region "Delete Methods"
        bool DeleteStockByStockID(string stockId);

        #endregion
    }
}
