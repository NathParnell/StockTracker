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
    public class CustomerService : ICustomerService
    {
        //setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(CustomerService));

        //define services
        private readonly IClientTransportService _clientTransportService;

        public CustomerService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }

        //Define public variables 
        public Customer CurrentUser { get; private set; }
        public bool IsLoggedIn { get; private set; } = false;


        /// <summary>
        /// Method which sets the Current User variable to manage the user on the client side
        /// Method also sets the IsLoggedIn variable, which assists in managing the state of the app
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(Customer user = null)
        {
            CurrentUser = user;
            if (CurrentUser == null)
                IsLoggedIn = false;
            else
                IsLoggedIn = true;
        }


        /// <summary>
        /// This method takes in an email and password, and it sends a request to the server to attempt
        /// a login. We are then provided with a response which should be an object of the customer who has logged in.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Customer RequestLogin(string email, string password)
        {
            //We create a JSON string of our Login Request and pass it to the TCP handler which handles our request
            //We are then returned a JSON string of our response from the server
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateCustomerLoginRequest(email, password, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (string.IsNullOrEmpty(jsonResponse))
                return null;

            //deserialize the JSON as a list of suppliers and get the first (and only) Supplier
            Customer customer = ResponseDeserializingHelper.DeserializeResponse<Customer>(jsonResponse).First();

            //set the current user to be the supplier who has just logged into the system
            SetCurrentUser(customer);

            if (CurrentUser != null)
            {
                Logger.Info($"RequestLogin(), Customer: {CurrentUser.Email} has logged into the system");
                //Now we are logged in we can Request Communication Ports from the server
                RequestCommunicationPorts(customer.CustomerId);
            }      
            else
                Logger.Info($"RequestLogin(), Customer login attempt failed");   

            return customer;
        }

        private void RequestCommunicationPorts(string customerId)
        {
            //We create a JSON string of our Request Communication Ports Request and pass it to the TCP handler which handles our request
            //We are then returned a JSON string of our response from the server
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateRequestCommunicationPortsRequest(customerId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (string.IsNullOrEmpty(jsonResponse))
                return;

            //deserialize the JSON as a list of suppliers and get the first (and only) Supplier
            List<string> ports = ResponseDeserializingHelper.DeserializeResponse<List<string>>(jsonResponse).First().ToList();

            //set the ports on the client transport service
            _clientTransportService.ConnectionPortNumber = ports[0];
        }

        public Customer GetCustomerByCustomerId(string customerId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetCustomerByCustomerIdRequest(customerId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (string.IsNullOrWhiteSpace(jsonResponse))
                return null;

            Customer customer = ResponseDeserializingHelper.DeserializeResponse<Customer>(jsonResponse).First();
            return customer;
        }

        public List<Customer> GetAllCustomers()
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetAllCustomersRequest(_clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (string.IsNullOrWhiteSpace(jsonResponse))
                return null;

            List<Customer> customers = ResponseDeserializingHelper.DeserializeResponse<List<Customer>>(jsonResponse).First().ToList();
            return customers;
        }

    }
}
