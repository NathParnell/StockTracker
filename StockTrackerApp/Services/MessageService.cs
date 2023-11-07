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
    public class MessageService : IMessageService
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

        public List<Message> GetMessageThreads(string userId, string contactId)
        {
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateGetMessageThreadsRequest(userId, contactId, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (string.IsNullOrEmpty(jsonResponse))
                return null;

            //deserialize the JSON as a list of messages
            List<Message> messages = ResponseDeserializingHelper.DeserializeResponse<List<Message>>(jsonResponse).First().ToList();
            return messages;
        }

        public bool SendMessage(Message message)
        {
            string jsonResponse = _clientTransportService.TcpHandler(
                RequestSerializingHelper.CreateSendPrivateMessagesRequest(new List<Message>() { message }, _clientTransportService.ConnectionPortNumber));

            //if the method we tried to call did not exist
            if (string.IsNullOrEmpty(jsonResponse))
                return false;

            //deserialize the JSON as a bool
            bool messageSent = ResponseDeserializingHelper.DeserializeResponse<bool>(jsonResponse).First();

            return messageSent;
        }
    }
}
