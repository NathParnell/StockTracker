using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface ISessionHistoryService
    {
        string GetPreviousWebpage();
        string GetCurrentWebpage();
        void AddWebpageToHistory(string path);
    }
}
