using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IMessageListenerService
    {
        string MessagePortNumber { get; set; }
        event EventHandler<Message> MessageReceived;

        void StartListener();
        void StopListener();
    }
}
