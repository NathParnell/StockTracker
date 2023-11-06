using Microsoft.Maui.ApplicationModel.Communication;
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
    public class MessageService
    {
        //Define Services
        private readonly IClientTransportService _clientTransportService;

        public MessageService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }

        public List<string> GetContactIds(string userId)
        {
            // We create a JSON string of our Login Request and pass it to the TCP handler which handles our request
            // We are then returned a JSON string of our response from the server
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetContactIdsRequest(userId, _clientTransportService.ConnectionPortNumber));
            
            //if the method we tried to call did not exist
            if (string.IsNullOrEmpty(jsonResponse))
                return null;

            //deserialize the JSON as a list of contact ids
            List<string> contactIds = ResponseDeserializingHelper.DeserializeResponse<List<string>>(jsonResponse).First().ToList();

            return contactIds;
        }
    }
}
