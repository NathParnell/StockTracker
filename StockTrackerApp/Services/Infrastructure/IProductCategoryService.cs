using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IProductCategoryService
    {
        #region "Get Methods"
        List<ProductCategory> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds);
        List<ProductCategory> GetAllProductCategories();
        #endregion

        #region "Add Methods"
        bool AddProductCategory(ProductCategory productCategory);
        #endregion

        #region "Validation Methods"
        string ValidateAndAddProductCategory(ProductCategory productCategory, List<ProductCategory> existingProductCategories);
        #endregion
    }
}
