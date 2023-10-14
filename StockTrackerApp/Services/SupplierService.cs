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

        #region "Get Methods"
        public List<Stock> GetStockBySupplier(string supplierId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetStockBySupplierRequest(supplierId));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<Stock> stock = ResponseDeserializingHelper.DeserializeResponse<List<Stock>>(jsonResponse).First().ToList();
            return stock;
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

        #region "Delete Methods"
        public bool DeleteStockByStockID(string stockId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateDeleteStockByStockIdRequest(stockId));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return false;

            bool deleteConfirmation = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();
            return deleteConfirmation;
        }
        #endregion

    }
}
