using NetMQ;
using NetMQ.Sockets;
using StockTracker.Services.Infrastructure;
using StockTrackerCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StockTracker.Services
{
    public class NetmqClientTransportService : IClientTransportService
    {
        /// <summary>
        /// Method which oversees the sending and receiving a message to and from the Server using NetMQ
        /// Returns a Json String of the response
        /// </summary>
        /// <param name="decryptedRequest"></param>
        /// <returns></returns>
        public string TcpHandler(string decryptedRequest)
        {
            string decryptedResponse = String.Empty;

            using (var requestSocket = new RequestSocket())
            using (var responseSocket = new ResponseSocket())
            {
                requestSocket.Connect("tcp://127.0.0.1:5556");

                //encrypt and send the request to the server
                string encryptedRequest = EncryptionHelper.Encrypt(decryptedRequest);
                requestSocket.SendFrame(encryptedRequest);
                string requestConfirmation = requestSocket.ReceiveFrameString();
                //log confirmation...


                responseSocket.Bind("tcp://*:5555");

                //receive and decrypt response from the server
                string encryptedResponse = responseSocket.ReceiveFrameString();
                decryptedResponse = EncryptionHelper.Decrypt(encryptedResponse);
                responseSocket.SendFrame("Message received by client");
                //log response...?
            }

            return decryptedResponse;
        }
    }
}
