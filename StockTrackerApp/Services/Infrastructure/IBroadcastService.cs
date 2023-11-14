using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IBroadcastService
    {
        bool BroadcastMessage(string broadcastMessageBody, string broadcastSubject);
    }
}
