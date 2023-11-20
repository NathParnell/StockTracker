using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IBroadcastListenerService
    {
        event EventHandler<Broadcast> BroadcastReceived;
        List<Broadcast> Broadcasts { get; set; }
        void StartListener(List<string> customerSubscriptions);
        void StopListener();
        void RestartListener(List<string> customerSubscriptions);
    }
}
