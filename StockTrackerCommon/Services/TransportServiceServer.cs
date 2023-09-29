using NetMQ;
using NetMQ.Sockets;
using StockTrackerCommon.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace StockTrackerCommon.Services
{
    public class TransportServiceServer : IServerTransportService
    {

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
        /// Client Handler handles any requests made by a client
        /// </summary>
        /// <param name="tcpClient"></param>
        public void TcpClientHandlerThread(TcpClient tcpClient)
        {
            using (NetworkStream nwStream = tcpClient.GetStream())
            {
                // Read Message
                string dataReceived = ReadClientRequest(ref tcpClient, nwStream);

                //process Message and perform actions
                Thread.Sleep(1000);

                //Provide Response to the Client
                WriteClientResponse(nwStream, "Hello there client");

                // Close the TCP connection
                tcpClient.Close();
            } 
        }

        #region "Read to and from TCP Client methods"
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
            byte[]  helloResponse = Encoding.Default.GetBytes(responseMessage);  //conversion string => byte array
            nwStream.Write(helloResponse, 0, helloResponse.Length);
        }

        #endregion

    }
}
