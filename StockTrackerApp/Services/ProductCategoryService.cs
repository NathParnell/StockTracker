using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        //setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ProductService));

        //define services
        private readonly IClientTransportService _clientTransportService;

        public ProductCategoryService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }

        #region "Get Methods"
        public List<ProductCategory> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductCategoriesByProductCategoryIDsRequest(productCategoryIds));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<ProductCategory> productCategories = ResponseDeserializingHelper.DeserializeResponse<List<ProductCategory>>(jsonResponse).First().ToList();
            return productCategories;
        }

        public List<ProductCategory> GetAllProductCategories()
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetAllProductCategoriesRequest());

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<ProductCategory> productCategories = ResponseDeserializingHelper.DeserializeResponse<List<ProductCategory>>(jsonResponse).First().ToList();
            return productCategories;
        }
        #endregion

        #region "Add Methods"
        public bool AddProductCategory(ProductCategory productCategory)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateAddProductCategoryRequest(productCategory));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool addConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return addConfirmation;
        }
        #endregion

        #region "Validation Methods"
        public string ValidateAndAddProductCategory(ProductCategory productCategory, List<ProductCategory> existingProductCategories)
        {
            bool isProductCategoryValid = true;
            string prompt = "Product Category invalid, please enter:\n";

            if (String.IsNullOrEmpty(productCategory.ProductCategoryName) || existingProductCategories.Any(cat => cat.ProductCategoryName == productCategory.ProductCategoryName))
            {
                prompt += "A Unique Product Category Name";
                isProductCategoryValid = false;
            }

            if (isProductCategoryValid == true)
            {
                productCategory.ProductCategoryId = Taikandi.SequentialGuid.NewGuid().ToString();
                if (AddProductCategory(productCategory))
                    return "Product Category Added Successfully";
                else
                    return "Error Adding Product Category";
            }
            else
            {
                return prompt;
            }
        }
        #endregion
    }
}
