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
        private readonly IMessageListenerService _messageListenerService;

        public SupplierService(IClientTransportService clientTransportService, IMessageListenerService messageListenerService)
        {
            _clientTransportService = clientTransportService;
            _messageListenerService = messageListenerService;
        }

        //Define public variables 
        public Supplier CurrentUser { get; private set; }
        public bool IsLoggedIn { get; private set; } = false;



        /// <summary>
        /// Method which sets the Current User variable to manage the user on the client side
        /// Method also sets the IsLoggedIn variable, which assists in managing the state of the app
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(Supplier user = null)
        {
            CurrentUser = user;
            if (CurrentUser == null)
                IsLoggedIn = false;
            else
                IsLoggedIn = true;
        }


        /// <summary>
        /// This method takes in an email and password, and it sends a request to the server to attempt
        /// a login. We are then provided with a response which should be an object of the supplier who has logged in.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Supplier RequestLogin(string email, string password)
        {
            //We create a JSON string of our Login Request and pass it to the TCP handler which handles our request
            //We are then returned a JSON string of our response from the server
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateSupplierLoginRequest(email, password, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            //deserialize the JSON as a list of suppliers and get the first (and only) Supplier
            Supplier supplier = ResponseDeserializingHelper.DeserializeResponse<Supplier>(jsonResponse).First();

            //set the current user to be the supplier who has just logged into the system
            SetCurrentUser(supplier);

            if (this.CurrentUser != null)
            {
                Logger.Info($"RequestLogin(), Supplier: {this.CurrentUser.Email} has logged into the system");
                //Now we are logged in we can Request Communication Ports from the server
                RequestCommunicationPorts(supplier.SupplierId);
            }
            else
                Logger.Info($"RequestLogin(), Supplier login attempt failed");

            return supplier;
        }

        private void RequestCommunicationPorts(string supplierId)
        {
            //We create a JSON string of our Request Communication Ports Request and pass it to the TCP handler which handles our request
            //We are then returned a JSON string of our response from the server
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateRequestCommunicationPortsRequest(supplierId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (string.IsNullOrEmpty(jsonResponse))
                return;

            //deserialize the JSON as a list of ports
            List<string> ports = ResponseDeserializingHelper.DeserializeResponse<List<string>>(jsonResponse).First().ToList();

            //set the ports on the client transport service and start listening for private messages
            _clientTransportService.ConnectionPortNumber = ports[0];
            _messageListenerService.MessagePortNumber = ports[1];
            _messageListenerService.StartListener();
        }

        public Supplier GetSupplierBySupplierId(string supplierId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetSupplierBySupplierIdRequest(supplierId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrWhiteSpace(jsonResponse))
                return null;

            Supplier supplier = ResponseDeserializingHelper.DeserializeResponse<Supplier>(jsonResponse).First();
            return supplier;
        }

        public List<Supplier> GetAllSuppliers()
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetAllSuppliersRequest(_clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (String.IsNullOrWhiteSpace(jsonResponse))
                return null;

            List<Supplier> suppliers = ResponseDeserializingHelper.DeserializeResponse<List<Supplier>>(jsonResponse).First().ToList();
            return suppliers;
        }
    }
}
