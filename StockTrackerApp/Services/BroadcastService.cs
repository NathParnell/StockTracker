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
    public class BroadcastService : IBroadcastService
    {
        //Define Services
        private readonly IClientTransportService _clientTransportService;
        private readonly ISupplierService _supplierService;
        private readonly IAuthorizationService _authorizationService;


        public BroadcastService(IClientTransportService clientTransportService, ISupplierService supplierService,
            IAuthorizationService authorizationService)
        {
            _clientTransportService = clientTransportService;
            _supplierService = supplierService;
            _authorizationService = authorizationService;
        }

        public bool BroadcastMessage(string topic, string broadcastMessageBody, string broadcastSubject)
        {
            // ensure that the user is a supplier
            if (_authorizationService.UserType != StockTrackerCommon.Models.UserType.Supplier)
            {
                return false;
            }

            string senderId = _supplierService.CurrentUser.SupplierId;

            Broadcast broadcast = new Broadcast()
            {
                Topic = topic,
                SenderId = senderId,
                MessageBody = broadcastMessageBody,
                Subject = broadcastSubject
            };

            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateBroadcastMessageRequest(broadcast, _clientTransportService.ConnectionPortNumber));


            //if the method we tried to call did not exist
            if (string.IsNullOrEmpty(jsonResponse))
                return false;

            //deserialize the json as a bool
            bool broadcastSent = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();

            return broadcastSent;
        }
    }
}
