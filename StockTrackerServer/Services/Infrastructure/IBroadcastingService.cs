using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services.Infrastructure
{
    public interface IBroadcastingService
    {
        void StartProductBroadcasterThread();
        void StopProductBroadcasterThread();
        bool BroadcastMessage(Broadcast broadcastMessage, string topic);
    }
}
