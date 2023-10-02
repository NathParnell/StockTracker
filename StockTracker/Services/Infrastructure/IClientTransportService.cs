using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTracker.Services.Infrastructure
{
    public interface IClientTransportService
    {
        public string TcpHandler(string jsonRequest);
    }
}
