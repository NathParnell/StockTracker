using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;

namespace StockTrackerServer.Services
{
    public class BroadcastingService : IBroadcastingService
    {
        public bool BroadcastMessage(Broadcast broadcastMessage)
        {
            using (var publisherSocket = new PublisherSocket())
            {
                //bind to our publishing socket
                publisherSocket.Bind("tcp://*:5557");

                //convert the broadcast message to json
                string unencryptedBroadcast= JsonSerializer.Serialize(broadcastMessage);

                //encrypt the broadcast message
                string encryptedBroadcast = EncryptionHelper.Encrypt(unencryptedBroadcast);

                //send the broadcast message
                publisherSocket.SendMoreFrame("Notification").SendFrame(encryptedBroadcast);

                //unbind from our publishing socket
                publisherSocket.Unbind("tcp://*:5557");

                return true;
            }
        }
    }
}
