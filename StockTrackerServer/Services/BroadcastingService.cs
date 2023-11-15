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
        private readonly PublisherSocket _publisherSocket;

        public BroadcastingService()
        {
            _publisherSocket = new PublisherSocket();
            _publisherSocket.Bind("tcp://*:5557");
        }

        public bool BroadcastMessage(Broadcast broadcastMessage)
        {
            try
            {
                //convert the broadcast message to json
                string unencryptedBroadcast = JsonSerializer.Serialize(broadcastMessage);

                //encrypt the broadcast message
                string encryptedBroadcast = EncryptionHelper.Encrypt(unencryptedBroadcast);

                //send the broadcast message
                _publisherSocket.SendMoreFrame("Notification").SendFrame(encryptedBroadcast);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
