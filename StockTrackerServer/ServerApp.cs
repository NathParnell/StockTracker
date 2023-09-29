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
        private readonly IServerTransportService _transportServiceServer;   
        
        public ServerApp(IServerTransportService transportServiceServer) 
        {
            _transportServiceServer = transportServiceServer;
        }

        /// <summary>
        /// Method called from Program.cs which actually starts the listener thread
        /// </summary>
        /// <param name="args"></param>
        public void Run(string[] args)
        {
            Console.WriteLine("SERVER--------");

            Thread listenerThread = new Thread(() => _transportServiceServer.ListenThread());
            listenerThread.Start();
        }
    }
}
