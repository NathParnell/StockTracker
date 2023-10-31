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
        //Define Services
        private IPortService _portService { get; set; }

        public MessagingService(IPortService portService) 
        {
            _portService = portService;
        }

        public bool SendMessage(Message message)
        {
            try
            {
                //save the message to the database
                //TODO: save the message to the database

                //check to make sure we can send this to our receiver as they may not even be able to receive it
                string targetEndpoint = _portService.GetClientMessagingPort(message.ReceiverId);
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

                    }
                }
                //Always return true here as the message will have been saved to the database even if the message didnt send to the receiver
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }



    }
}
