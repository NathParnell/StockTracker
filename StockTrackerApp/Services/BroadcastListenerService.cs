using NetMQ.Sockets;
using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using System.Text.Json;
using StockTrackerApp.Services.Infrastructure;

namespace StockTrackerApp.Services
{
    public class BroadcastListenerService : IBroadcastListenerService
    {
        //Define events
        public event EventHandler<Broadcast> BroadcastReceived;

        //Define broadcastListenerThread
        private Thread _broadcastListenerThread;

        //Define variables
        private const string PUBLISHER_ENDPOINT = "192.168.5.122:5557";

        public void StartListener()
        {
            // Start a thread which will run in the background to listen for broadcasts
            _broadcastListenerThread = new Thread(() => BroadcastListenerThread());
            _broadcastListenerThread.Start();

            return;
        }

        public void StopListener()
        {
            //if the thread is running, end it by adding a thread timeout of 10ms
            if (_broadcastListenerThread != null && _broadcastListenerThread.IsAlive)
            {
                _broadcastListenerThread.Join(10);
            }
            return;
        }

        private void BroadcastListenerThread()
        {
            try
            {
                // Create a new socket
                using (var socket = new SubscriberSocket())
                {
                    // Connect the socket to the publisher
                    socket.Connect($"tcp://{PUBLISHER_ENDPOINT}");

                    // Subscribe to the topic
                    socket.Subscribe("Notification");

                    while (true)
                    {
                        var topic = socket.ReceiveFrameString();
                        var message = socket.ReceiveFrameString();

                        //Ensure that a broadcast was received
                        if (String.IsNullOrWhiteSpace(message) == false)
                        {
                            // Handle the broadcast in a new thread
                            Thread broadcastHandlerThread = new Thread(() => BroadcastHandlerThread(message));
                            broadcastHandlerThread.Start();
                        }
                    }
                    //socket.Disconnect("tcp://192.168.0.86:5557");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void BroadcastHandlerThread(string encryptedBroadcast)
        {
            try
            {
                //decrypt the broadcast
                string decryptedBroadcast = EncryptionHelper.Decrypt(encryptedBroadcast);

                //deserialize the broadcast
                Broadcast broadcast = JsonSerializer.Deserialize<Broadcast>(decryptedBroadcast);

                //raise the broadcast received event
                BroadcastReceived?.Invoke(this, broadcast);
            }
            catch (Exception ex)
            {
                //Logging
            }
        }
    }
}
