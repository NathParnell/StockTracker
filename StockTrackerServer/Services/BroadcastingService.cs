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
        //Set up Logger
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(BroadcastingService));

        //Inject services
        private readonly IDataService _dataService;

        private readonly PublisherSocket _publisherSocket;
        private bool _productBroadcasterThreadRunning = false;

        public BroadcastingService(IDataService dataService)
        {
            _publisherSocket = new PublisherSocket();
            _publisherSocket.Bind("tcp://*:5557");
            _dataService = dataService;
        }

        public void StartProductBroadcasterThread()
        {
            _productBroadcasterThreadRunning = true;
            _logger.Info($"StartProductBroadcasterThread() - Product Broadcaster thread Started");
            Thread productBroadcasterThread = new Thread(() => ProductBroadcasterThread());
            productBroadcasterThread.Start();
        }

        public void StopProductBroadcasterThread()
        {
            _productBroadcasterThreadRunning = false;
        }

        private void ProductBroadcasterThread()
        {
            while (_productBroadcasterThreadRunning == true)
            {
                //get all products
                List<Product> products = _dataService.GetAllProducts().Result;

                _logger.Info("ProductBroadcasterThread(), Sending Stock Update Broadcasts");

                //loop through each product and send a broadcast message
                foreach (Product product in products)
                {
                    Broadcast broadcast = new Broadcast()
                    {
                        Topic = product.ProductId,
                        SenderId = product.SupplierId,
                        Subject = "Stock Update - " + product.ProductName,
                        MessageBody = $"In Stock: {product.ProductQuantity}, Price: £{product.Price}"
                    };

                    //run the broadcast message method
                    BroadcastMessage(broadcast);
                }
                // Block the current thread for 45 seconds
                Thread.Sleep(45000);
            }
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
                _publisherSocket.SendMoreFrame(broadcastMessage.Topic).SendFrame(encryptedBroadcast);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
