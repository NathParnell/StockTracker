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

        public List<ProductCategory> GetProductCategoriesByProductCategoryIds(List<string> productCategoryIds)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetProductCategoriesByProductCategoryIDsRequest(productCategoryIds));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            List<ProductCategory> productCategories = ResponseDeserializingHelper.DeserializeResponse<List<ProductCategory>>(jsonResponse).First().ToList();
            return productCategories;
        }

    }
}
