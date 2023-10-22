using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public class SupplierService : ISupplierService
    {
        //setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(SupplierService));

        //define services
        private readonly IClientTransportService _clientTransportService;

        public SupplierService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }

        #region "Product Methods"
        public List<Product> GetProductBySupplierId(string supplierId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductsBySupplierRequest(supplierId));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Product> product = ResponseDeserializingHelper.DeserializeResponse<List<Product>>(jsonResponse).First().ToList();
            return product;
        }
        public bool DeleteProductByProductID(string productId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateDeleteProductByProductIdRequest(productId));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool deleteConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return deleteConfirmation;
        }
        
        public Product GetProductByProductId(string productId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductByProductIdRequest(productId));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            Product product = ResponseDeserializingHelper.DeserializeResponse<Product>(jsonResponse).First();
            return product;
        }

        public List<Product> GetProductsByProductIds(List<string> productIds)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductsByProductIdsRequest(productIds));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Product> products = ResponseDeserializingHelper.DeserializeResponse<List<Product>>(jsonResponse).First().ToList();
            return products;
        }

        public List<Product> GetAllProducts()
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetAllProductsRequest());

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Product> products = ResponseDeserializingHelper.DeserializeResponse<List<Product>>(jsonResponse).First().ToList();
            return products;
        }

        public bool AddProduct(Product product)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateAddProductRequest(product));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool addConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return addConfirmation;
        }

        public string ValidateAndAddProduct(Product newProduct, List<Product> suppliersExistingProducts, List<ProductCategory> productCategories)
        {
            bool isProductValid = true;
            string prompt = "Product invalid, please enter:\n";

            //check that category isnt null and that the category they are trying to add to exists
            if (String.IsNullOrEmpty(newProduct.ProductCategoryId) || productCategories.Any(cat => cat.ProductCategoryId == newProduct.ProductCategoryId) == false)
            {
                isProductValid = false;
                prompt += "A Valid Product Category,\n";
            }
            //check that product name isnt null and that a product doesnt already exist with the same name
            if (String.IsNullOrEmpty(newProduct.ProductName) || suppliersExistingProducts.Any(prod => prod.ProductName == newProduct.ProductName))
            {
                isProductValid = false;
                prompt += "A unique product name, \n";
            }
            //check that product name isnt null and that a product doesnt already exist with the same product code
            if (String.IsNullOrEmpty(newProduct.ProductCode) || suppliersExistingProducts.Any(prod => prod.ProductCode == newProduct.ProductCode))
            {
                isProductValid = false;
                prompt += "A unique product code \n";
            }
            //check that the product brand isnt null
            if (String.IsNullOrEmpty(newProduct.ProductBrand))
            {
                isProductValid = false;
                prompt += "A product brand name \n";
            }
            if (newProduct.ProductSize <= 0)
            {
                isProductValid = false;
                prompt += "A product size";
            }
            if (newProduct.ProductMeasurementUnit == null)
            {
                isProductValid = false;
                prompt += "A valid Product Measurement Unit";
            }
            if (newProduct.Price <= 0)
            {
                isProductValid = false;
                prompt += "A Product Price";
            }

            if (isProductValid == true)
            {
                newProduct.ProductId = Taikandi.SequentialGuid.NewGuid().ToString();
                if (AddProduct(newProduct))
                    return "Product Added Successfully";
                else
                    return "Error Adding Product";
            }
            else
            {
                return prompt;
            }
        }

        public bool UpdateProduct(Product updatedProduct)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateUpdateProductRequest(updatedProduct));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool addConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return addConfirmation;
        }

        public string ValidateAndUpdateProduct(Product updatedProduct, List<Product> suppliersExistingProducts, List<ProductCategory> productCategories)
        {
            bool isProductValid = true;
            string prompt = "Updated Product invalid, please enter:\n";

            //remove the product we have changed from the list we are validating the update products against
            suppliersExistingProducts.RemoveAll(prod => prod.ProductId == updatedProduct.ProductId);

            //check that category isnt null and that the category they are trying to add to exists
            if (String.IsNullOrEmpty(updatedProduct.ProductCategoryId) || productCategories.Any(cat => cat.ProductCategoryId == updatedProduct.ProductCategoryId) == false)
            {
                isProductValid = false;
                prompt += "A Valid Product Category,\n";
            }
            //check that product name isnt null and that a product doesnt already exist with the same name
            if (String.IsNullOrEmpty(updatedProduct.ProductName) || suppliersExistingProducts.Any(prod => prod.ProductName == updatedProduct.ProductName))
            {
                isProductValid = false;
                prompt += "A unique product name, \n";
            }
            //check that product name isnt null and that a product doesnt already exist with the same product code
            if (String.IsNullOrEmpty(updatedProduct.ProductCode) || suppliersExistingProducts.Any(prod => prod.ProductCode == updatedProduct.ProductCode))
            {
                isProductValid = false;
                prompt += "A unique product code \n";
            }
            //check that the product brand isnt null
            if (String.IsNullOrEmpty(updatedProduct.ProductBrand))
            {
                isProductValid = false;
                prompt += "A product brand name \n";
            }
            if (updatedProduct.ProductSize <= 0)
            {
                isProductValid = false;
                prompt += "A product size";
            }
            if (updatedProduct.ProductMeasurementUnit == null)
            {
                isProductValid = false;
                prompt += "A valid Product Measurement Unit";
            }
            if (updatedProduct.Price <= 0)
            {
                isProductValid = false;
                prompt += "A Product Price";
            }

            if (isProductValid == true)
            {
                if (UpdateProduct(updatedProduct))
                    return "Product Updated Successfully";
                else
                    return "Error Adding Product";
            }
            else
            {
                return prompt;
            }
        }

        #endregion

        #region "Product Category Methods"

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

        public bool AddProductCategory(ProductCategory productCategory)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateAddProductCategoryRequest(productCategory));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool addConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return addConfirmation;
        }

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
