using StockTrackerCommon.Models;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace StockTrackerCommon.Helpers
{
    public static class RequestSerializingHelper
    {
        /// <summary>
        /// Accepts parameters such as a Method Name and the data which we want to pass to the server
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateRequest(string methodName, object[] data)
        {
            var request = new Request
            {
                RequestId = Taikandi.SequentialGuid.NewGuid().ToString(),
                ClientIp = GetPrivateIpAddress(),
                Method = methodName,
                Data = data
            };
            return JsonSerializer.Serialize(request);
        }

        #region "Create Requests"

        /// <summary>
        /// Creates a JSON string of a Validate Login Request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateLoginRequest(string username, string password)
        {
            string methodName = "ValidateLogin";
            object[] data = new object[] { username, password };
            return CreateRequest(methodName, data);
        }

        /// <summary>
        /// Creates a JSON string of a get stock by supplier request
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static string CreateGetProductsBySupplierRequest(string supplierId)
        {
            string methodName = "RetrieveProductsBySupplierId";
            object[] data = new object[] { supplierId };
            return CreateRequest(methodName, data);
        }

        /// <summary>
        /// Creates a JSON string of a get products by product ids request 
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static string CreateGetProductsByProductIdsRequest(List<string> productIds)
        {
            string methodName = "RetrieveProductsByProductIds";
            object[] data = new object[] { productIds };
            return CreateRequest(methodName, data);
        }

        /// <summary>
        /// Creates a JSON string of a get all products request 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public static string CreateGetAllProductsRequest()
        {
            string methodName = "RetrieveAllProducts";
            object[] data = new object[] { };
            return CreateRequest(methodName, data);
        }

        /// <summary>
        /// Creates a JSON string of a get product categories by product category ids request 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public static string CreateGetProductCategoriesByProductCategoryIDsRequest(List<string> categoryIds)
        {
            string methodName = "RetrieveProductCategoriesByProductCategoryIds";
            object[] data = new object[] { categoryIds };
            return CreateRequest(methodName, data);
        }

        /// <summary>
        /// Creates a JSON string of a get all product categories request 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public static string CreateGetAllProductCategoriesRequest()
        {
            string methodName = "RetrieveAllProductCategories";
            object[] data = new object[] { };
            return CreateRequest(methodName, data);
        }

        /// <summary>
        /// Creates a JSON string of a delete stock by stock id request 
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static string CreateDeleteProductByProductIdRequest(string productId)
        {
            string methodName = "DeleteProductByProductId";
            object[] data = new object[] { productId };
            return CreateRequest(methodName, data);
        }

        public static string CreateAddProduct(Product product)
        {
            string methodName = "AddProduct";
            object[] data = new object[] { product };
            return CreateRequest(methodName, data);
        }

        #endregion



        #region "Get IP Address"
        /// <summary>
        /// We need to get the client's PUBLIC ip address if we want the server and clients to be able 
        /// to communicate on different networks
        /// </summary>
        /// <returns></returns>
        private static string GetPublicIpAddress()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //use this third party service to get the client's public ip address
                string ipApiUrl = "https://api.ipify.org?format=text";
                HttpResponseMessage response = httpClient.GetAsync(ipApiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string ip = response.Content.ReadAsStringAsync().Result;
                    return ip.Trim();
                }
                else
                {
                    throw new Exception("Failed to retrieve public IP address.");
                }
            }
        }

        private static string GetPrivateIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        #endregion
    }
}
