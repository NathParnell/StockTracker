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
        private List<string> VisitedWebpages = new List<string>();


        /// <summary>
        /// Get the second newest value from the previous uri list which is the redirectUri.
        /// We then remove the newest value from the list
        /// We then return the redirectUri
        /// </summary>
        public string GetPreviousWebpage()
        {
            if (VisitedWebpages != null && VisitedWebpages.Count > 0)
            {
                string redirectPath = VisitedWebpages[VisitedWebpages.Count - 2];
                VisitedWebpages.RemoveAt(VisitedWebpages.Count - 1);
                return redirectPath;
            }

            return null;
        }

        public string GetCurrentWebpage()
        {
            if (VisitedWebpages != null && VisitedWebpages.Count > 0)
            {
                return VisitedWebpages[VisitedWebpages.Count - 1];
            }
            return null;
        }

        /// <summary>
        /// Pass through a webpage name and add it to the list of previous webpages
        /// We only add the webpage to this list if there are no previous webpages or if the previous webpage doesn't equal the current webpage
        /// This is better for navigation purposes
        /// </summary>
        /// <param name="path"></param>
        public void AddWebpageToHistory(string path)
        {
            if (VisitedWebpages != null && VisitedWebpages.Count > 0)
            {
                if (VisitedWebpages[VisitedWebpages.Count - 1] != path)
                    VisitedWebpages.Add(path);
            }
            else
                VisitedWebpages.Add(path);
        }

    }
}
