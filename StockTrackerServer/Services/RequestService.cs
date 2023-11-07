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
        private readonly IPortService _portService;
        private readonly IMessagingService _messagingService;

        public RequestService(IAuthenticationService authenticationService, IDataService dataService, IPortService portService, IMessagingService messagingService)
        {
            _authenticationService = authenticationService;
            _dataService = dataService;
            _portService = portService;
            _messagingService = messagingService;
        }

        /// <summary>
        /// Method takes in a JSON string and we deserialize it into our Response object. From this we can get the name of the Method
        /// in which we intend to call from the request, for example "ValidateLogin"
        /// We pass the methods an object array, and we interpret the data within the method
        /// From this method we get provided with another JSON string which we return
        /// </summary>
        /// <param name="jsonRequestString"></param>
        /// <returns></returns>
        public string ProcessRequest(string jsonRequestString, ref string clientIpAddress, ref string clientPortAddress)
        {
            string response = String.Empty;

            Request request = JsonSerializer.Deserialize<Request>(jsonRequestString);

            if (request != null)
            {
                clientIpAddress = request.ClientIp;
                clientPortAddress = request.ClientPortNumber;

                MethodInfo methodInfo = this.GetType().GetMethod(request.Method);
                if (methodInfo != null)
                {
                    response = methodInfo.Invoke(this, new object[] { new object[] { request } }).ToString();
                }
            }
            return response;
        }

        /// <summary>
        /// Method which takes in an object array and turns it into a list of messages
        /// We go through each of the messages and send the messages to the dedicated receivers
        /// We return a success boolean which notifies whether or not the messages were sent/saved successfully
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string SendPrivateMessages(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<Message> messages = JsonSerializer.Deserialize<List<List<Message>>>(request.Data.ToString()).First();
            bool success = true;
            foreach (Message message in messages)
            {
                //send the message for the message and if we are returned false, then we change the value of the success variable to false
                if (_messagingService.SendMessage(message) == false)
                    success = false;
            }
            //make response
            return ResponseSerializingHelper.CreateResponse(success);

        }

        #region "Ports Methods"

        /// <summary>
        /// Method which takes in an object array and turns it into a clientId
        /// With this information we call our GenerateClientRequestPort method and our GenerateClientMessagingPort and to generate a list of ports
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveCommunicationPorts(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string clientId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            List<string> ports = new List<string>
            {
                _portService.GenerateClientRequestPort(clientId, request.ClientIp),
                _portService.GenerateClientMessagingPort(clientId, request.ClientIp)
            };
            return ResponseSerializingHelper.CreateResponse(ports);
        }

        /// <summary>
        /// Method which takes in an object array and turns it into a clientId
        /// with this information we call our UnassignClientsRequestPort and UnassignClientsMessagingPort methods to unassign the ports, if this succeeded we return true
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string DropCommunicationPorts(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string clientId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            bool success = _portService.UnassignClientsRequestPort(clientId) && _portService.UnassignClientsMessagingPort(clientId);
            return ResponseSerializingHelper.CreateResponse(success);
        }

        #endregion

        #region "Validate Login Methods"
        /// <summary>
        /// Method which takes an object array and turns it into a email and password
        /// With this information we call our AuthenticateSupplier method which returns the supplier who has just logged in (or null if invalid user creds)
        /// We then create and return a supplier authentication response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ValidateSupplierLogin(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> parameters = JsonSerializer.Deserialize<List<string>>(request.Data.ToString());
            Supplier supplier = _authenticationService.AuthenticateSupplier(parameters[0], parameters[1]);
            //make a response with this
            return ResponseSerializingHelper.CreateResponse(supplier);
        }

        /// <summary>
        /// Method which takes an object array and turns it into a email and password
        /// With this information we call our AuthenticateCustomer method which returns the customer who has just logged in (or null if invalid user creds)
        /// We then create and return a customer authentication response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ValidateCustomerLogin(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> parameters = JsonSerializer.Deserialize<List<string>>(request.Data.ToString());
            Customer customer = _authenticationService.AuthenticateCustomer(parameters[0], parameters[1]);
            //make a response with this
            return ResponseSerializingHelper.CreateResponse(customer);
        }

        #endregion

        #region "Get Methods"

        /// <summary>
        /// Method which takes in an object array and turns it into a supplier id
        /// With this information we can make a call to the data service to retrieve a supplier by the aforementioned supplier id
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveSupplierBySupplierId(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string supplierId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            Supplier supplier = _dataService.GetSupplierBySupplierId(supplierId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(supplier);
        }

        /// <summary>
        /// Method which takes in an object array which is empty and not used (Conforms with the other methods)
        /// We make a call to the data service which retrieves all of the available Suppliers
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveAllSuppliers(object[] requestObject)
        {
            List<Supplier> suppliers = _dataService.GetAllSuppliers().Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(suppliers);
        }

        public string RetrieveSuppliersBySupplierIds(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> supplierIds = JsonSerializer.Deserialize<List<List<string>>>(request.Data.ToString()).First();
            List<Supplier> suppliers = _dataService.GetSuppliersBySupplierIds(supplierIds).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(suppliers);

        }

        /// <summary>
        /// Method which takes in an object array and turns it into a customer id
        /// With this information we can make a call to the data service to retrieve a customer by the aforementioned customer id
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveCustomerByCustomerId(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string customerId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            Customer customer = _dataService.GetCustomerByCustomerId(customerId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(customer);
        }

        /// <summary>
        /// Method which takes in an object array which is empty and not used (Conforms with the other methods)
        /// We make a call to the data service which retrieves all of the available Customers
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveAllCustomers(object[] requestObject)
        {
            List<Customer> customers = _dataService.GetAllCustomers().Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(customers);
        }

        public string RetrieveCustomersByCustomerIds(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> customerIds = JsonSerializer.Deserialize<List<List<string>>>(request.Data.ToString()).First();
            List<Customer> suppliers = _dataService.GetCustomersByCustomerIds(customerIds).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(suppliers);
        }

        /// <summary>
        /// Method which takes in an object array and turns it into a supplier id
        /// With this information we make a call to the data service to retrieve a suppliers products by the aforementioned supplier id
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveProductsBySupplierId(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string supplierId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            List<Product> products = _dataService.GetProductsBySupplierId(supplierId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(products);
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
        /// Method which takes in an object array which is empty and not used (Conforms with the other methods)
        /// We make a call to the data service which retrieves all of the available products
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveAllProducts(object[] requestObject)
        {
            List<Product> products = _dataService.GetAllProducts().Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(products);
        }

        /// <summary>
        /// Method which takes in an object array and converts it in to a string which the product id of the product we wish to retrieve
        /// We make a call to the data service to retrieve the product
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveProductByProductId(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string productId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            Product product = _dataService.GetProductByProductId(productId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(product);
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

        /// <summary>
        /// Method which takes in an object array which is empty and not used (Conforms with the other methods)
        /// We make a call to the data service which retrieves all of the available product categories
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveAllProductCategories(object[] requestObject)
        {
            List<ProductCategory> productCategories = _dataService.GetAllProductCategories().Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(productCategories);
        }

        /// <summary>
        /// Method which takes in an object array and converts it in to a string which is the user id of the user whos contacts we wish to retrieve
        /// We make a call to the data service to retrieve the contacts of the user with the user id we pass through
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveContactIds(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string userId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            List<string> contactIds = _dataService.GetContactIds(userId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(contactIds);
        }

        /// <summary>
        /// Method which takes in an object array and converts it in to a list of strings which are the user ids of the users whose message threads we wish to retrieve
        /// We make a call to the data service to retrieve the message threads of the users with the user ids we pass through
        /// We then create and return our response
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string RetrieveMessageThreads(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> parameters = JsonSerializer.Deserialize<List<string>>(request.Data.ToString());
            List<Message> messages = _dataService.GetMessageThreads(parameters[0], parameters[1]).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(messages);
        }

        #endregion

        #region "Delete Methods"
        public string DeleteProductByProductId(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            string productId = JsonSerializer.Deserialize<List<string>>(request.Data.ToString()).First();
            bool deleteConfirmation = _dataService.DeleteProductByProductId(productId).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(deleteConfirmation);

        }

        #endregion

        #region "Add Methods"
        
        public string AddProduct(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            Product product = JsonSerializer.Deserialize<List<Product>>(request.Data.ToString()).First();
            bool addConfirmation = _dataService.AddProduct(product).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(addConfirmation);
        }
        
        public string AddProductCategory(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            ProductCategory productCategory = JsonSerializer.Deserialize<List<ProductCategory>>(request.Data.ToString()).First();
            bool addConfirmation = _dataService.AddProductCategory(productCategory).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(addConfirmation);
        }

        public string AddOrder(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            Order order = JsonSerializer.Deserialize<List<Order>>(request.Data.ToString()).First();
            bool addConfirmation = _dataService.AddOrder(order).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(addConfirmation);
        }

        #endregion

        #region "Update Methods"
        public string UpdateProduct(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            Product product = JsonSerializer.Deserialize<List<Product>>(request.Data.ToString()).First();
            bool editConfirmation = _dataService.UpdateProduct(product).Result;
            //make response
            return ResponseSerializingHelper.CreateResponse(editConfirmation);
        }
        #endregion

    }
}
