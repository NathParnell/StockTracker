using log4net.Repository.Hierarchy;
using StockTracker.Services.Infrastructure;
using StockTrackerCommon.Helpers;
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
        //Setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ClientTransportService));

        /// <summary>
        /// Method which oversees the sending and receiving a message to and from the Server using System.Net.Sockets.
        /// Returns a Json String of the response
        /// </summary>
        /// <param name="jsonRequest"></param>
        /// <returns></returns>
        public string TcpHandler(string decryptedRequest)
        {
            string decryptedResponse = String.Empty;
            
            //Create a Tcp client and connect to the Remote endpoint of the server
            TcpClient tcpClient = new TcpClient();
            IPAddress serverIp = IPAddress.Parse("127.0.0.1");
            tcpClient.Connect(serverIp, 5000);

            using (NetworkStream nwStream = tcpClient.GetStream())
            {
                Logger.Info($"TcpHandler(), The unencrypted request we will send to the client: " +
                        $"{decryptedRequest}");

                //Encrypt the request
                string encryptedRequest = EncryptionHelper.Encrypt(decryptedRequest);
                Logger.Info($"The encrypted request we will send to the client:" +
                        $"{encryptedRequest} ");

                //Send the request to the Server
                WriteRequestToServer(nwStream, encryptedRequest);

                //Receive the encrypted response from the server
                string encryptedResponse = ReadResponseFromServer(ref tcpClient, nwStream);
                Logger.Info($"TcpHandler(), Client has received encrypted response from server: " +
                        $"{encryptedResponse}");

                //Decrypt response from the server
                decryptedResponse = EncryptionHelper.Decrypt(encryptedResponse);
                Logger.Info($"TcpHandler(), The decrypted response from the server :" +
                        $"{decryptedResponse}");
            }

            return decryptedResponse;
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
