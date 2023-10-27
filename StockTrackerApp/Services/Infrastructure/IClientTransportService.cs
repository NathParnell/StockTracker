using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IClientTransportService
    {
        string ConnectionPortNumber { get; set; }
        string TcpHandler(string jsonRequest);
    }
}
