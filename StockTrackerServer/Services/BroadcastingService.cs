using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
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
        //Inject services
        private readonly IDataService _dataService;

        private readonly PublisherSocket _publisherSocket;
        private List<Product> _products = new List<Product>();
        private bool _productBroadcasterThreadRunning = false;

        public BroadcastingService(IDataService dataService)
        {
            _publisherSocket = new PublisherSocket();
            _publisherSocket.Bind("tcp://*:5557");
            _dataService = dataService;
        }

        public void StartProductBroadcasterThread()
        {
            _products = _dataService.GetAllProducts().Result;
            _productBroadcasterThreadRunning = true;
            Thread productBroadcasterThread = new Thread(() => ProductBroadcasterThread());
            productBroadcasterThread.Start();
        }

        public void StopProductBroadcasterThread()
        {
            _productBroadcasterThreadRunning = false;
            _products = new List<Product>();
        }

        private void ProductBroadcasterThread()
        {
            while (_productBroadcasterThreadRunning == true)
            {
                foreach (Product product in _products)
                {
                    Broadcast broadcast = new Broadcast()
                    {
                        SenderId = product.SupplierId,
                        Subject = product.ProductName,
                        MessageBody = $"In Stock: {product.ProductQuantity}, Price: £{product.Price}"
                    };

                    //run the broadcast message method in a new thread
                    Thread broadcastMessageThread = new Thread(() => BroadcastMessage(broadcast, product.ProductId));
                    broadcastMessageThread.Start();
                }
                // Block the current thread for 45 seconds
                Thread.Sleep(45000);
            }
        }

        public bool BroadcastMessage(Broadcast broadcastMessage, string topic)
        {
            try
            {
                //convert the broadcast message to json
                string unencryptedBroadcast = JsonSerializer.Serialize(broadcastMessage);

                //encrypt the broadcast message
                string encryptedBroadcast = EncryptionHelper.Encrypt(unencryptedBroadcast);

                //send the broadcast message
                _publisherSocket.SendMoreFrame(topic).SendFrame(encryptedBroadcast);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
