using StockTrackerCommon.Models;
using System.Net;
using System.Net.NetworkInformation;
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
        public static string CreateRequest(string methodName, object[] data, string portNumber)
        {
            var request = new Request
            {
                RequestId = Taikandi.SequentialGuid.NewGuid().ToString(),
                ClientIp = GetPrivateIpAddress(),
                ClientPortNumber = portNumber,
                Method = methodName,
                Data = data
            };
            return JsonSerializer.Serialize(request);
        }

        #region "Generate Requests"

        #region "Port Requests"

        /// <summary>
        /// Creates a JSON string of a Get Communication Ports Request
        /// </summary>
        /// <param name="portNumber"></param>
        /// <returns></returns>
        public static string CreateRequestCommunicationPortsRequest(string clientId, string portNumber)
        {
            string methodName = "RetrieveCommunicationPorts";
            object[] data = new object[] { clientId };
            return CreateRequest(methodName, data, portNumber);
        }
        
        /// <summary>
        /// Creates a JSON string of a drop Communication Ports Request
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="portNumber"></param>
        /// <returns></returns>
        public static string CreateDropCommunicationPortsRequest(string clientId, string portNumber)
        {
            string methodName = "DropCommunicationPorts";
            object[] data = new object[] { clientId };
            return CreateRequest(methodName, data, portNumber);
        }

        #endregion

        #region "Message Requests"
        
        /// <summary>
        /// Creates a JSON string of a Send Message Request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="portNumber"></param>
        /// <returns></returns>
        public static string CreateSendPrivateMessagesRequest(List<Message> messages, string portNumber)
        {
            string methodName = "SendPrivateMessages";
            object[] data = new object[] { messages };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a Get Contact ids Request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="portNumber"></param>
        /// <returns></returns>
        public static string CreateGetContactIdsRequest(string userId, string portNumber)
        {
            string methodName = "RetrieveContactIds";
            object[] data = new object[] { userId };
            return CreateRequest(methodName, data, portNumber);
        }

        #endregion

        #region "User Requests"

        /// <summary>
        /// Creates a JSON string of a Validate Supplier Login Request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateSupplierLoginRequest(string email, string password, string portNumber)
        {
            string methodName = "ValidateSupplierLogin";
            object[] data = new object[] { email, password };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a Get Supplier By Supplier Id Request
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string CreateGetSupplierBySupplierIdRequest(string supplierId, string portNumber)
        {
            string methodName = "RetrieveSupplierBySupplierId";
            object[] data = new object[] { supplierId };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a Get All Suppliers Request
        /// </summary>
        /// <returns></returns>
        public static string CreateGetAllSuppliersRequest(string portNumber)
        {
            string methodName = "RetrieveAllSuppliers";
            object[] data = new object[] { };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a Validate Customer Login Request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateCustomerLoginRequest(string email, string password, string portNumber)
        {
            string methodName = "ValidateCustomerLogin";
            object[] data = new object[] { email, password };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a Get Customer By Customer Id Request
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public static string CreateGetCustomerByCustomerIdRequest(string customerId, string portNumber)
        {
            string methodName = "RetrieveCustomerByCustomerId";
            object[] data = new object[] { customerId };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a Get All Customers Request
        /// </summary>
        /// <returns></returns>
        public static string CreateGetAllCustomersRequest(string portNumber)
        {
            string methodName = "RetrieveAllCustomers";
            object[] data = new object[] { };
            return CreateRequest(methodName, data, portNumber);
        }

        #endregion

        #region "Product Requests"
        /// <summary>
        /// Creates a JSON string of a get stock by supplier request
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static string CreateGetProductsBySupplierRequest(string supplierId, string portNumber)
        {
            string methodName = "RetrieveProductsBySupplierId";
            object[] data = new object[] { supplierId };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a get products by product ids request 
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static string CreateGetProductsByProductIdsRequest(List<string> productIds, string portNumber)
        {
            string methodName = "RetrieveProductsByProductIds";
            object[] data = new object[] { productIds };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a get product by product id request 
        /// </summary>
        /// <param name="productIds"></param>
        /// <returns></returns>
        public static string CreateGetProductByProductIdRequest(string productId, string portNumber)
        {
            string methodName = "RetrieveProductByProductId";
            object[] data = new object[] { productId };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a get all products request 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public static string CreateGetAllProductsRequest(string portNumber)
        {
            string methodName = "RetrieveAllProducts";
            object[] data = new object[] { };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a delete stock by stock id request 
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static string CreateDeleteProductByProductIdRequest(string productId, string portNumber)
        {
            string methodName = "DeleteProductByProductId";
            object[] data = new object[] { productId };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of an Add Product request
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static string CreateAddProductRequest(Product product, string portNumber)
        {
            string methodName = "AddProduct";
            object[] data = new object[] { product };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of an Update Product request
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static string CreateUpdateProductRequest(Product product, string portNumber)
        {
            string methodName = "UpdateProduct";
            object[] data = new object[] { product };
            return CreateRequest(methodName, data, portNumber);
        }
        #endregion

        #region "Product Category Requests"
        /// <summary>
        /// Creates a JSON string of a get product categories by product category ids request 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public static string CreateGetProductCategoriesByProductCategoryIDsRequest(List<string> categoryIds, string portNumber)
        {
            string methodName = "RetrieveProductCategoriesByProductCategoryIds";
            object[] data = new object[] { categoryIds };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of a get all product categories request 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <returns></returns>
        public static string CreateGetAllProductCategoriesRequest(string portNumber)
        {
            string methodName = "RetrieveAllProductCategories";
            object[] data = new object[] { };
            return CreateRequest(methodName, data, portNumber);
        }

        /// <summary>
        /// Creates a JSON string of an Add Product Category request
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static string CreateAddProductCategoryRequest(ProductCategory productCategory, string portNumber)
        {
            string methodName = "AddProductCategory";
            object[] data = new object[] { productCategory };
            return CreateRequest(methodName, data, portNumber);
        }
        #endregion

        #region "Order Requests"
        public static string CreateAddOrderRequest(Order order, string portNumber)
        {
            string methodName = "AddOrder";
            object[] data = new object[] { order };
            return CreateRequest(methodName, data, portNumber);
        }

        #endregion

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
            // var host = Dns.GetHostEntry(Dns.GetHostName());
            // foreach (var ip in host.AddressList)
            // {
            //     if (ip.AddressFamily == AddressFamily.InterNetwork)
            //     {
            //         return ip.ToString();
            //     }
            // }
            // throw new Exception("No network adapters with an IPv4 address in the system!");
            
            // Get all network interfaces
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // Check if the network interface is up and it's not a loopback or virtual interface
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Ppp)
                {
                    // Get the IPv4 addresses associated with this interface
                    foreach (UnicastIPAddressInformation ipInfo in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ipInfo.Address.ToString();
                        }
                    }
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        
        #endregion
    }
}
