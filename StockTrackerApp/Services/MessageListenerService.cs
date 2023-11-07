using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;

namespace StockTrackerApp.Services
{
    public class MessageListenerService : IMessageListenerService
    {
        //Define port number for listening for messages
        public string MessagePortNumber { get; set; } = String.Empty;

        //define events
        public event EventHandler<Message> MessageReceived;


        public void StartListener()
        {
            // Start a thread which will run in the background to listen for messages
            Thread messageListenerThread = new Thread(() => MessageListenerThread());
            messageListenerThread.Start();

            return;
        }

        public void StopListener()
        {
            MessagePortNumber = String.Empty;

            return;
        }

        private void MessageListenerThread()
        {
            try
            {
                //while we have a port number, listen for messages
                while (String.IsNullOrWhiteSpace(MessagePortNumber) == false)
                {
                    // Create a new socket
                    using (var socket = new ResponseSocket())
                    {
                        // Bind the socket to the port number
                        socket.Bind($"tcp://*:{MessagePortNumber}");

                        // Receive a message from the server
                        string message = socket.ReceiveFrameString();

                        // Send a response back to the server
                        socket.SendFrame("Message received by client");
                        
                        socket.Unbind($"tcp://*:{MessagePortNumber}");

                        //Ensure that a request was received
                        if (String.IsNullOrWhiteSpace(message) == false)
                        {
                            // Handle the request in a new thread
                            Thread messageHandlerThread = new Thread(() => MessageHandlerThread(message));
                            messageHandlerThread.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }

        private void MessageHandlerThread(string encryptedMessage)
        {
            try
            {
                // Process the message
                //decrypt message
                string decryptedMessage = EncryptionHelper.Decrypt(encryptedMessage);

                Message message = JsonSerializer.Deserialize<Message>(decryptedMessage);

                MessageReceived?.Invoke(this, message);

            }
            catch (Exception ex)
            {
                // Log the exception
            }
        }
    }
}
