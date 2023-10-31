using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services
{
    public class PortService : IPortService
    {
        //Define variables
        private Dictionary<string, string> _assignedRequestClientEndpoints = new Dictionary<string, string>();
        private Dictionary<string, string> _assignedMessagingClientEndpoints = new Dictionary<string, string>();

        public string GenerateClientRequestPort(string clientId, string ipAddress)
        {
            //if the user already has a port assigned, return that port
            if (_assignedRequestClientEndpoints.ContainsKey(clientId) == true)
                return _assignedRequestClientEndpoints[clientId].ToString();

            int minPort = 49152;
            int maxPort = 57343;

            //lock to ensure thread safety
            lock (_assignedRequestClientEndpoints)
            {
                Random random = new Random();
                int port = random.Next(minPort, maxPort);

                string endpoint = $"{ipAddress}:{port.ToString()}";

                //if the endpoint is already assigned to a client, generate a new port and endpoint
                while (_assignedRequestClientEndpoints.ContainsValue(endpoint))
                {
                    port = random.Next(minPort, maxPort);
                    endpoint = $"{ipAddress}:{port.ToString()}";
                }

                _assignedRequestClientEndpoints.Add(clientId, endpoint);
                return port.ToString();
            }
        }

        public string GenerateClientMessagingPort(string clientId, string ipAddress)
        {
            //if the user already has a port assigned, return that port
            if (_assignedMessagingClientEndpoints.ContainsKey(clientId) == true)
                return _assignedMessagingClientEndpoints[clientId].ToString();

            int minPort = 57344;
            int maxPort = 65535;

            //lock to ensure thread safety
            lock (_assignedMessagingClientEndpoints)
            {
                Random random = new Random();
                int port = random.Next(minPort, maxPort);

                string endpoint = $"{ipAddress}:{port.ToString()}";

                //if the port is already assigned, generate a new port
                while (_assignedMessagingClientEndpoints.ContainsValue(endpoint))
                {
                    port = random.Next(minPort, maxPort);
                    endpoint = $"{ipAddress}:{port.ToString()}";
                }

                _assignedMessagingClientEndpoints.Add(clientId, endpoint);
                return port.ToString();
            }
        }

        public string GetClientMessagingPort(string clientId)
        {
            //if the user doesnt already have a port assigned, return null
            if (_assignedMessagingClientEndpoints.ContainsKey(clientId) == false)
                return null;

            //lock to ensure thread safety
            lock (_assignedMessagingClientEndpoints)
            {
                return _assignedMessagingClientEndpoints[clientId].ToString();
            }
        } 

        public bool UnassignClientsRequestPort(string clientId)
        {
            //if the user doesnt already have a port assigned, return false
            if (_assignedRequestClientEndpoints.ContainsKey(clientId) == false)
                return true;

            //lock to ensure thread safety
            lock (_assignedRequestClientEndpoints)
            {
                _assignedRequestClientEndpoints.Remove(clientId);
                return true;
            }
        }

        public bool UnassignClientsMessagingPort(string clientId)
        {
            //if the user doesnt already have a port assigned, return false
            if (_assignedMessagingClientEndpoints.ContainsKey(clientId) == false)
                return true;

            //lock to ensure thread safety
            lock (_assignedMessagingClientEndpoints)
            {
                _assignedMessagingClientEndpoints.Remove(clientId);
                return true;
            }
        }
    }
}
