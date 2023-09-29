using StockTrackerCommon.Models;
using StockTrackerCommon.Services;
using StockTrackerCommon.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer
{
    public class ServerApp
    {
        private readonly IDataService _dataService;
        private readonly IServerTransportService _transportServiceServer;
        
        
        public ServerApp(IDataService dataService, IServerTransportService transportServiceServer) 
        {
            _dataService = dataService;
            _transportServiceServer = transportServiceServer;
        }

        public void Run(string[] args)
        {
            Console.WriteLine("SERVER--------");

            Thread listenerThread = new Thread(() => _transportServiceServer.ListenThread());
            listenerThread.Start();

            //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            //Console.WriteLine("Starting TCP Listener...");
            //TcpListener listener = new TcpListener(ipAddress, 5000);
            //listener.Start();

            //try
            //{
            //    while (true)
            //    {
            //        var tcpClient = listener.AcceptTcpClient();


            //        StreamReader inStream = new StreamReader(tcpClient.GetStream(), Encoding.ASCII);


            //        string receivedData = inStream.ReadLine();
            //        Console.WriteLine("Received: " + receivedData);
            //        //Socket clientSocket = listener.AcceptSocket();
            //        //Console.WriteLine("Connection accepted.");

            //        //var childSocketThread = new Thread(() =>
            //        //{
            //        //    byte[] data = new byte[100];
            //        //    int size = clientSocket.Receive(data);
            //        //    Console.WriteLine("Recieved data: ");
            //        //    string x = "";
            //        //    for (int i = 0; i < size; i++)
            //        //    {
            //        //        //x.Append(Convert.ToChar(data[i]));
            //        //        x = Encoding.ASCII.GetString(data);
            //        //    }

            //        //    Console.Write(x);

            //        //    Console.WriteLine();
            //        //    clientSocket.Send(Encoding.ASCII.GetBytes($"Message {x.ToString()} Recieved"));

            //        //    clientSocket.Close();
            //        //});

            //        //childSocketThread.Start();
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error: " + e.Message);
            //}
            //finally
            //{
            //    listener.Stop(); // Stop the listener when done
            //}

        }

    }
}


//code which prints out users
//User[] users = _dataService.GetAllUsers().Result.ToArray();
//foreach(User user in users)
//{
//    Console.WriteLine($"User : {user.UserName}");
//}