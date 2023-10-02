using StockTracker.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StockTracker.Services
{
    public class ClientTransportService : IClientTransportService
    {

        /// <summary>
        /// Method which oversees the sending and receiving a message to and from the Server.
        /// Returns a Json String response
        /// </summary>
        /// <param name="jsonRequest"></param>
        /// <returns></returns>
        public string TcpHandler(string jsonRequest)
        {
            string jsonResponse = String.Empty;
            TcpClient tcpClient = new TcpClient();
            IPAddress serverIp = IPAddress.Parse("127.0.0.1");
            tcpClient.Connect(serverIp, 5000);

            using (NetworkStream nwStream = tcpClient.GetStream())
            {
                WriteRequestToServer(nwStream, jsonRequest);
                jsonResponse = ReadResponseFromServer(ref tcpClient, nwStream);
            }
            return jsonResponse;
        }


        #region "Writing to and Reading From the TCP Server methods"
        /// <summary>
        /// Method which writes string to a TCPStream
        /// </summary>
        /// <param name="nwStream"></param>
        /// <param name="jsonRequest"></param>
        public void WriteRequestToServer(NetworkStream nwStream, string jsonRequest)
        {
            byte[] encodedRequest = Encoding.Default.GetBytes(jsonRequest);  //conversion string => byte array
            nwStream.Write(encodedRequest, 0, encodedRequest.Length);
        }

        /// <summary>
        /// Method which reads a string from a TCPStream
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="nwStream"></param>
        /// <returns></returns>
        public string ReadResponseFromServer(ref TcpClient tcpClient, NetworkStream nwStream)
        {
            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, tcpClient.ReceiveBufferSize);

            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        #endregion

    }
}
