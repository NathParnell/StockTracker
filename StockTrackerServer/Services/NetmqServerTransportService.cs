using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using StockTrackerCommon.Helpers;
using System.Net.Sockets;

namespace StockTrackerServer.Services
{
    public class NetmqServerTransportService : IServerTransportService
    {
        //Set up Logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(NetmqServerTransportService));

        //define services
        private readonly IRequestService _requestService;

        //Define variables
        private string _serverPortNumber = "5556";

        public NetmqServerTransportService(IRequestService requestService)
        {
            _requestService = requestService;
        }


        /// <summary>
        /// Creates a Response socket and we constantly listen for clients
        /// Upon receiving a request from a client, we create a client handler which handles any request made by a client
        /// </summary>
        public void ListenThread()
        {
            var mqServer = new ResponseSocket();
            mqServer.Bind($"tcp://*:{_serverPortNumber}");

            while (true)
            {
                //Takes in the Response socket and waits for a request
                string requestMessage = ReadClientRequest(ref mqServer);

                //Ensure that a request was received
                if (String.IsNullOrWhiteSpace(requestMessage) == false)
                {
                    // Handle the request in a new thread
                    Thread tcpClientHandlerThread = new Thread(() => TcpClientHandlerThread(requestMessage));
                    tcpClientHandlerThread.Start();
                }
            }
        }

        /// <summary>
        /// Client Handler thread which handles any requests made by a client
        /// </summary>
        /// <param name="state"></param>
        public void TcpClientHandlerThread(string encryptedRequest)
        {
            try
            {
                string clientIpAddress = String.Empty;
                string clientPortNumber = String.Empty;

                //decrypt request from client
                string decryptedRequest = EncryptionHelper.Decrypt(encryptedRequest);
                Logger.Info("TcpClientHandlerThread(), Server has received encrypted request from unknown client: " +
                    $"{decryptedRequest}");

                //process Message and perform actions and get the prepared response
                string decryptedResponse = _requestService.ProcessRequest(decryptedRequest, ref clientIpAddress, ref clientPortNumber);
                Logger.Info($"TcpClientHandlerThread(), The unencrypted response we will send to the client " +
                    $"{clientIpAddress}: {clientPortNumber}: {decryptedRequest}");

                //Encrypt response for client
                string encryptedResponse = EncryptionHelper.Encrypt(decryptedResponse);
                Logger.Info($"TcpClientHandlerThread(), the encrypted response we will send to the client " +
                    $"{clientIpAddress}: {clientPortNumber}: {decryptedRequest}");

                //Send encrypted response to client
                WriteClientResponse(encryptedResponse, clientIpAddress, clientPortNumber);
            }
            catch (Exception ex)
            {
                Logger.Warn($"TcpClientHandlerThread(), Error: {ex.Message}", ex);
            }
        }


        #region "Read and write to and from the client using NetMQ"
        /// <summary>
        /// Method which takes in a response socket and which listens for clients and reads a message from the client
        /// We then send a confirmation message to the client to declare we received the request
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private string ReadClientRequest(ref ResponseSocket socket)
        {
            try
            {
                //Wait for an incoming message
                Logger.Info("ReadClientRequest(), Listening for NetMQ Clients...");

                string dataReceived = socket.ReceiveFrameString();
                socket.SendFrame("Message received by server");

                return dataReceived;
            }
            catch (Exception ex)
            {
                Logger.Warn($"ReadClientRequest(), Error: {ex.Message}", ex);
            }

            return null;
        }

        /// <summary>
        /// Method which creates a request socket targeting the client and sends the response to the client.
        /// To target the client we use the ip address in which they sent in the request
        /// We also receive and log a confirmation string from the client
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="clientIp"></param>
        private void WriteClientResponse(string responseMessage, string clientIp, string clientPort)
        {
            try
            {
                using (var socket = new RequestSocket())
                {
                    socket.Connect($"tcp://{clientIp}:{clientPort}");
                    socket.SendFrame(responseMessage);
                    string responseConfirmation = socket.ReceiveFrameString();
                    Logger.Info($"WriteClientResponse(), {responseConfirmation}");
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"WriteClientResponse(), Error: {ex.Message}", ex);
            }
        }
        #endregion

    }
}
