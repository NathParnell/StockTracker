using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public class SessionHistoryService : ISessionHistoryService
    {
        private List<string> PreviousWebpages = new List<string>();


        /// <summary>
        /// Get the second newest value from the previous uri list which is the redirectUri.
        /// We then remove the newest value from the list
        /// We then return the redirectUri
        /// </summary>
        public string GetPreviousWebpage()
        {
            if (PreviousWebpages != null && PreviousWebpages.Count > 0)
            {
                string redirectPath = PreviousWebpages[PreviousWebpages.Count - 2];
                PreviousWebpages.RemoveAt(PreviousWebpages.Count - 1);
                return redirectPath;
            }

            return null;
        }

        /// <summary>
        /// Pass through a webpage name and add it to the list of previous webpages
        /// </summary>
        /// <param name="path"></param>
        public void AddWebpageToHistory(string path)
        {
            PreviousWebpages.Add(path);
        }

    }
}
