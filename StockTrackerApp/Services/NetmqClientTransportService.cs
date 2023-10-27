using NetMQ;
using NetMQ.Sockets;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public class NetmqClientTransportService : IClientTransportService
    {
        //Setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(NetmqClientTransportService));

        //define variables
        //create a port number variable and default it to 5555 - a new port number will be determined by the server
        public string ConnectionPortNumber { get; set; } = "5555";


        /// <summary>
        /// Method which oversees the sending and receiving a message to and from the Server using NetMQ
        /// Returns a Json String of the response
        /// </summary>
        /// <param name="decryptedRequest"></param>
        /// <returns></returns>
        public string TcpHandler(string decryptedRequest)
        {
            try
            {
                string decryptedResponse = String.Empty;

                using (var requestSocket = new RequestSocket())
                using (var responseSocket = new ResponseSocket())
                {
                    requestSocket.Connect("tcp://127.0.0.1:5556");

                    Logger.Info($"TcpHandler(), The unencrypted request we will send to the client: " +
                        $"{decryptedRequest}");

                    //encrypt the request
                    string encryptedRequest = EncryptionHelper.Encrypt(decryptedRequest);
                    Logger.Info($"The encrypted request we will send to the client:" +
                        $"{encryptedRequest} ");

                    //Send the request to the Server
                    requestSocket.SendFrame(encryptedRequest);
                    string requestConfirmation = requestSocket.ReceiveFrameString();
                    Logger.Info($"TcpHandler(), {requestConfirmation}");

                    responseSocket.Bind($"tcp://*:{ConnectionPortNumber}");

                    //Receive response from the server and provide server with confirmation receipt
                    string encryptedResponse = responseSocket.ReceiveFrameString();
                    responseSocket.SendFrame("Message received by client");
                    Logger.Info($"TcpHandler(), Client has received encrypted response from server: " +
                        $"{encryptedResponse}");

                    //Decrypt response from server
                    decryptedResponse = EncryptionHelper.Decrypt(encryptedResponse);
                    Logger.Info($"TcpHandler(), The decrypted response from the server :" +
                        $"{decryptedResponse}");
                }

                return decryptedResponse;
            }
            catch (Exception ex)
            {
                Logger.Warn($"TcpClientHandlerThread(), Error: {ex.Message}", ex);
                return null;
            }
        }

    }
}
