using log4net.Repository.Hierarchy;
using NetMQ;
using NetMQ.Sockets;
using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerServer.Services
{
    public class MessagingService : IMessagingService
    {
        //Set up Logger
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(MessagingService));

        //Define Services
        private IPortService _portService { get; set; }
        private IDataService _dataService { get; set; }

        public MessagingService(IPortService portService, IDataService dataService) 
        {
            _portService = portService;
            _dataService = dataService;
        }

        public bool SendMessage(Message message)
        {
            try
            {
                _logger.Info($"SendMessage(), Sending Message to user:{message.ReceiverId}");
                //save the message to the database
                bool messagedSaved = _dataService.AddMessage(message).Result;

                //check to make sure we can send this to our receiver as they may not even be able to receive it
                string targetEndpoint = _portService.GetClientMessagingEndpoint(message.ReceiverId);
                if (String.IsNullOrWhiteSpace(targetEndpoint) == false)
                {
                    using (var messageSocket = new RequestSocket())
                    {
                        messageSocket.Connect($"tcp://{targetEndpoint}");

                        //convert message to json
                        string unencryptedMessage = JsonSerializer.Serialize(message);

                        //encrypt message
                        string encryptedMessage = EncryptionHelper.Encrypt(unencryptedMessage);

                        //send message
                        messageSocket.SendFrame(encryptedMessage);

                        //receive confirmation
                        string confirmation = messageSocket.ReceiveFrameString();
                        _logger.Info($"SendMessage(), Send Message Confirmation received: {confirmation}");
                    }
                }
                //Always return true here as the message will have been saved to the database even if the message didnt send to the receiver
                return true;   
            }
            catch (Exception ex)
            {
                _logger.Warn($"SendMessage(), Error: {ex.Message}", ex);
                return false;
            }   
        }
    }
}
