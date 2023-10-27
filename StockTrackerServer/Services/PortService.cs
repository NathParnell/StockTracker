using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services
{
    public class PortService : IPortService
    {
        //Define variables
        private Dictionary<string, int> _assignedRequestClientPorts = new Dictionary<string, int>();
        private Dictionary<string, int> _assignedMessagingClientPorts = new Dictionary<string, int>();

        public string GenerateClientRequestPort(string clientId)
        {
            //if the user already has a port assigned, return that port
            if (_assignedRequestClientPorts.ContainsKey(clientId) == true)
                return _assignedRequestClientPorts[clientId].ToString();

            int minPort = 49152;
            int maxPort = 57343;

            //lock to ensure thread safety
            lock (_assignedRequestClientPorts)
            {
                Random random = new Random();
                int port = random.Next(minPort, maxPort);

                //if the port is already assigned, generate a new port
                while (_assignedRequestClientPorts.ContainsValue(port))
                {
                    port = random.Next(minPort, maxPort);
                }

                _assignedRequestClientPorts.Add(clientId, port);
                return port.ToString();
            }
        }

        public string GenerateClientMessagingPort(string clientId)
        {
            //if the user already has a port assigned, return that port
            if (_assignedMessagingClientPorts.ContainsKey(clientId) == true)
                return _assignedMessagingClientPorts[clientId].ToString();

            int minPort = 57344;
            int maxPort = 65535;

            //lock to ensure thread safety
            lock (_assignedMessagingClientPorts)
            {
                Random random = new Random();
                int port = random.Next(minPort, maxPort);

                //if the port is already assigned, generate a new port
                while (_assignedMessagingClientPorts.ContainsValue(port))
                {
                    port = random.Next(minPort, maxPort);
                }

                _assignedMessagingClientPorts.Add(clientId, port);
                return port.ToString();
            }
        }

        public string GetClientMessagingPort(string clientId)
        {
            //if the user doesnt already have a port assigned, return null
            if (_assignedMessagingClientPorts.ContainsKey(clientId) == false)
                return null;

            //lock to ensure thread safety
            lock (_assignedMessagingClientPorts)
            {
                return _assignedMessagingClientPorts[clientId].ToString();
            }
        } 

        public bool UnassignClientsRequestPort(string clientId)
        {
            //if the user doesnt already have a port assigned, return false
            if (_assignedRequestClientPorts.ContainsKey(clientId) == false)
                return true;

            //lock to ensure thread safety
            lock (_assignedRequestClientPorts)
            {
                _assignedRequestClientPorts.Remove(clientId);
                return true;
            }
        }

        public bool UnassignClientsMessagingPort(string clientId)
        {
            //if the user doesnt already have a port assigned, return false
            if (_assignedMessagingClientPorts.ContainsKey(clientId) == false)
                return true;

            //lock to ensure thread safety
            lock (_assignedMessagingClientPorts)
            {
                _assignedMessagingClientPorts.Remove(clientId);
                return true;
            }
        }
    }
}
