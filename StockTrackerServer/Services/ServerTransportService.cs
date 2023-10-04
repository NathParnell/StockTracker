using StockTrackerCommon.Helpers;
using StockTrackerServer.Services.Infrastructure;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StockTrackerServer.Services
{
    public class ServerTransportService : IServerTransportService
    {
        IRequestService _requestService;

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
            Console.WriteLine("Listening For Client...");

            // Continuously loop to allow for further clients
            while (true)
            {
                // Wait for an incoming TCP Connection
                var tcpClient = listener.AcceptTcpClient();

                // Ensure that we have a Connected TCP Client
                if (tcpClient != null)
                {
                    Console.WriteLine($"Server connected to Client : {tcpClient.Client.RemoteEndPoint.ToString()}");
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
                // Read Message from client and Decrypt the message
                string dataReceived = EncryptionHelper.Decrypt(ReadClientRequest(ref tcpClient, nwStream));

                //process Message and perform actions and get the prepared response
                string response = _requestService.ProcessRequest(dataReceived);

                //Encrypt response and send Response to the Client
                WriteClientResponse(nwStream, EncryptionHelper.Encrypt(response));

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

            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received : " + dataReceived);
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
