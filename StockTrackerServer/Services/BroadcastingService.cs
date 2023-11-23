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
        private bool _productBroadcasterThreadsRunning = false;

        public BroadcastingService(IDataService dataService)
        {
            _publisherSocket = new PublisherSocket();
            _publisherSocket.Bind("tcp://*:5557");
            _dataService = dataService;
        }

        public void StartProductBroadcasterThreads()
        {
            _productBroadcasterThreadsRunning = true;
            _logger.Info($"StartProductBroadcasterThread() - Product Broadcaster threads Started");
            //start the thread for hourly notifications - 45 seconds
            Thread hourlyProductBroadcasterThread = new Thread(() => ProductBroadcasterThread(45000, "Hourly"));
            hourlyProductBroadcasterThread.Start();

            //start the thread for daily notifications - 5 mins
            Thread dailyProductBroadcasterThread = new Thread(() => ProductBroadcasterThread(45000, "Daily"));
            dailyProductBroadcasterThread.Start(300000);
        }

        public void StopProductBroadcasterThreads()
        {
            _productBroadcasterThreadsRunning = false;
        }

        private void ProductBroadcasterThread(int broadcastInterval, string broadcastType)
        {
            try
            {
                //continuously loop through the products and send a broadcast message until we stop the thread
                while (_productBroadcasterThreadsRunning == true)
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
                            Subject = $"{broadcastType} Stock Update - " + product.ProductName,
                            MessageBody = $"In Stock: {product.ProductQuantity}, Price: £{product.Price}"
                        };

                        //run the broadcast message method
                        BroadcastMessage(broadcast);
                    }
                    // Block the current thread for 45 seconds
                    Thread.Sleep(broadcastInterval);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ProductBroadcasterThread() - Error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Method which sends an encrypted broadcast message to all subscribers
        /// </summary>
        /// <param name="broadcastMessage"></param>
        /// <returns></returns>
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
