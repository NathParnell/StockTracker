using StockTrackerCommon.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Services
{
    public class ClientTransportService : IClientTransportService
    {

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


        public void WriteRequestToServer(NetworkStream nwStream, string jsonRequest)
        {
            byte[] encodedRequest = Encoding.Default.GetBytes(jsonRequest);  //conversion string => byte array
            nwStream.Write(encodedRequest, 0, encodedRequest.Length);
        }

        public string ReadResponseFromServer(ref TcpClient tcpClient, NetworkStream nwStream)
        {
            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, tcpClient.ReceiveBufferSize);

            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }


    }
}
