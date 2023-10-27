using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public  class ProductService : IProductService
    {
        //setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ProductService));

        //define services
        private readonly IClientTransportService _clientTransportService;

        public ProductService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }


        #region "Get Methods"
        public List<Product> GetProductsBySupplierId(string supplierId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductsBySupplierRequest(supplierId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Product> product = ResponseDeserializingHelper.DeserializeResponse<List<Product>>(jsonResponse).First().ToList();
            return product;
        }

        public Product GetProductByProductId(string productId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductByProductIdRequest(productId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            Product product = ResponseDeserializingHelper.DeserializeResponse<Product>(jsonResponse).First();
            return product;
        }

        public List<Product> GetProductsByProductIds(List<string> productIds)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductsByProductIdsRequest(productIds, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Product> products = ResponseDeserializingHelper.DeserializeResponse<List<Product>>(jsonResponse).First().ToList();
            return products;
        }

        public List<Product> GetAllProducts()
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetAllProductsRequest(_clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Product> products = ResponseDeserializingHelper.DeserializeResponse<List<Product>>(jsonResponse).First().ToList();
            return products;
        }
        #endregion

        #region "Add Methods"
        public bool AddProduct(Product product)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateAddProductRequest(product, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool addConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return addConfirmation;
        }
        #endregion

        #region "Update Methods"
        public bool UpdateProduct(Product updatedProduct)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateUpdateProductRequest(updatedProduct, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool updateConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return updateConfirmation;
        }
        #endregion

        #region "Delete Methods"
        public bool DeleteProductByProductID(string productId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateDeleteProductByProductIdRequest(productId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool deleteConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return deleteConfirmation;
        }
        #endregion

        #region "Validation Methods"
        public string ValidateAndAddProduct(Product newProduct, List<Product> suppliersExistingProducts, List<ProductCategory> productCategories, ref bool success)
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
                {
                    success = true;
                    return "Product Added Successfully";
                }
                else
                {
                    success = false;
                    return "Error Adding Product";
                }

            }
            else
            {
                success = false;
                return prompt;
            }
        }

        public string ValidateAndUpdateProduct(Product updatedProduct, List<Product> suppliersExistingProducts, List<ProductCategory> productCategories, ref bool success)
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
                {
                    success = true;
                    return "Product Updated Successfully";
                }
                else
                {
                    success = false;
                    return "Error Adding Product";
                }
            }
            else
            {
                success = false;
                return prompt;
            }
        }
        #endregion  
    }
}
