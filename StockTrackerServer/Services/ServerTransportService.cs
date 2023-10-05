using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StockTrackerServer.Services
{
    public class ServerTransportService : IServerTransportService
    {
        //Set up Logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ServerTransportService));

        //define services
        private readonly IRequestService _requestService;

        public ServerTransportService(IRequestService requestService) 
        {
            _requestService = requestService;
        }

        /// <summary>
        /// We constantly listen for clients and whenever a new client is created,
        /// We make a client handler thread which handles a request made by a client
        /// </summary>
        public void ListenThread()
        {
            var listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            // Continuously loop to allow for further clients
            while (true)
            {
                // Wait for an incoming TCP Connection
                Logger.Info("ListenThread(), Listening for TCP Clients...");
                var tcpClient = listener.AcceptTcpClient();

                // Ensure that we have a Connected TCP Client
                if (tcpClient != null)
                {
                    Logger.Info($"ListenThread(), Server connected to Client : {tcpClient.Client.RemoteEndPoint.ToString()}");
                    Thread tcpClientHandlerThread = new Thread(() => TcpClientHandlerThread(tcpClient));
                    tcpClientHandlerThread.Start();
                }
            }
        }

        /// <summary>
        /// Client Handler thread which handles any requests made by a client
        /// </summary>
        /// <param name="tcpClient"></param>
        public void TcpClientHandlerThread(TcpClient tcpClient)
        {
            using (NetworkStream nwStream = tcpClient.GetStream())
            {
                string clientIpAddress = tcpClient.Client.RemoteEndPoint.ToString();

                // Read Message from client
                string encryptedRequest = ReadClientRequest(ref tcpClient, nwStream);
                Logger.Info($"TcpClientHandlerThread(), Server had received encrypted request from client " +
                    $"{clientIpAddress}: {encryptedRequest} ");

                //Decrypt request from client
                string decryptedRequest = EncryptionHelper.Decrypt(encryptedRequest);
                Logger.Info($"TcpClientHandlerThread(), The request decrypted from client " +
                    $"{clientIpAddress}: {decryptedRequest}");

                //process Message and perform actions and get the prepared response
                string decryptedResponse = _requestService.ProcessRequest(decryptedRequest, ref clientIpAddress);
                Logger.Info($"TcpClientHandlerThread(), The unencrypted response we will send to the client " +
                    $"{clientIpAddress}: {decryptedRequest}");              

                //Encrypt response for client
                string encryptedResponse = EncryptionHelper.Encrypt(decryptedResponse);
                Logger.Info($"TcpClientHandlerThread(), the encrypted response we will send to the client " +
                    $"{clientIpAddress}: {encryptedResponse}");

                //Send encrypted response to client
                WriteClientResponse(nwStream, encryptedResponse);

                // Close the TCP connection
                tcpClient.Close();
            } 
        }

        #region "Read from and Write to the TCP Client methods"
        /// <summary>
        /// method which just reads a string from the TCPStream
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="nwStream"></param>
        /// <returns></returns>
        public string ReadClientRequest(ref TcpClient tcpClient, NetworkStream nwStream)
        {
            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, tcpClient.ReceiveBufferSize);

            //convert the data received from a byte array to a string
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return dataReceived;
        }

        /// <summary>
        /// method which just writes a string to the TCPStream
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="nwStream"></param>
        /// <param name="responseMessage"></param>
        public void WriteClientResponse(NetworkStream nwStream, string responseMessage)
        {
            byte[] encodedResponse = Encoding.Default.GetBytes(responseMessage);
            nwStream.Write(encodedResponse, 0, encodedResponse.Length);
        }

        #endregion

    }
}
