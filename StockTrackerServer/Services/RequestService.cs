using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerServer.Services
{
    public class RequestService : IRequestService
    {
        //define services
        private readonly IAuthenticationService _authenticationService;
        private readonly IDataService _dataService;

        public RequestService(IAuthenticationService authenticationService, IDataService dataService)
        {
            _authenticationService = authenticationService;
            _dataService = dataService;
        }

        /// <summary>
        /// Method takes in a JSON string and we deserialize it into our Response object. From this we can get the name of the Method
        /// in which we intend to call from the request, for example "ValidateLogin"
        /// We pass the methods an object array, and we interpret the data within the method
        /// From this method we get provided with another JSON string which we return
        /// </summary>
        /// <param name="jsonRequestString"></param>
        /// <returns></returns>
        public string ProcessRequest(string jsonRequestString, ref string clientIpAddress)
        {
            string response = String.Empty;

            Request request = JsonSerializer.Deserialize<Request>(jsonRequestString);

            if (request != null)
            {
                clientIpAddress = request.ClientIp;

                MethodInfo methodInfo = this.GetType().GetMethod(request.Method);
                if (methodInfo != null)
                {
                    response = methodInfo.Invoke(this, new object[] { new object[] { request } }).ToString();
                }
            }
            return response;
        }

        /// <summary>
        /// Method which takes an object array and turns it into a username and password
        /// With this information we call our AuthenticateUser method which returns the user who has just logged in (or null if invalid user creds)
        /// We then create and return a user authentication response 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ValidateLogin(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> parameters = JsonSerializer.Deserialize<List<string>>(request.Data.ToString());
            User user = _authenticationService.AuthenticateUser(parameters[0], parameters[1]);
            //need to make a response with this
            return ResponseSerializingHelper.CreateResponse(user);
        }

        #region "Get Methods"
        /// <summary>
        /// Method which takes in an object array and turns it into a supplier id
        /// With this information we make a call to the data service to retrieve the stock of a supplier with the aforementioned supplier id
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveStockBySupplierId(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string supplierId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            List<Stock> stock = _dataService.GetStockBySupplierId(supplierId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(stock);
        }

        /// <summary>
        /// Method which takes in an object array and converts it in to a list of strings which are product ids.
        /// With this information we make a call to the data service to retrieve the products with the product ids we pass through
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveProductsByProductIds(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> productIds = JsonSerializer.Deserialize<List<List<string>>>(request.Data.ToString()).First();
            List<Product> products = _dataService.GetProductsByProductIds(productIds).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(products);
        }

        /// <summary>
        /// Method which takes in an object array and converts it in to a list of strings which are product category ids.
        /// With this information we make a call to the data service to retrieve the product categories with the product category ids we pass through
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveProductCategoriesByProductCategoryIds(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> categoryIds = JsonSerializer.Deserialize<List<List<string>>>(request.Data.ToString()).First();
            List<ProductCategory> productCategories = _dataService.GetProductCategoriesByProductCategoryIds(categoryIds).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(productCategories);
        }
        #endregion

        #region "Delete Methods"
        public string DeleteStockItemByStockId(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string stockId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            bool deleteConfirmation = _dataService.DeleteStockByStockId(stockId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(deleteConfirmation);

        }

        #endregion

    }
}
