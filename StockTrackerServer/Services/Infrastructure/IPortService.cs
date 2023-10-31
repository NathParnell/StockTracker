using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services.Infrastructure
{
    public interface IPortService
    {
        string GenerateClientRequestPort(string clientId, string ipAddress);
        string GenerateClientMessagingPort(string clientId, string ipAddress);
        string GetClientMessagingPort(string clientId);
        bool UnassignClientsRequestPort(string clientId);
        bool UnassignClientsMessagingPort(string clientId);
    }
}
