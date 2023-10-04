using StockTrackerCommon.Models;
using StockTrackerServer.Services;
using StockTrackerServer.Services.Infrastructure;
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
        //Set up Logger
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ServerApp));

        //define services
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
            logger.Info("Run(), Server Started");

            Thread listenerThread = new Thread(() => _transportServiceServer.ListenThread());
            listenerThread.Start();
        }
    }
}
