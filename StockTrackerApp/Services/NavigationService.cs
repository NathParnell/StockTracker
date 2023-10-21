using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public class NavigationService : NavigationManager
    {
        public static List<string> PreviousWebpageUris = new List<string>();

        /// <summary>
        /// Get the second newsest value from the previous uri list which is the redirectUri.
        /// We then remove the newest value from the list
        /// We then redirect to the redirectUri
        /// </summary>
        public void NavigateToPreviousWebpage(string parameter = "")
        {
            if (PreviousWebpageUris == null && PreviousWebpageUris.Count > 0)
            {
                string redirectUri = PreviousWebpageUris[PreviousWebpageUris.Count - 2];
                PreviousWebpageUris.RemoveAt(PreviousWebpageUris.Count - 1);
                base.NavigateTo($"redirectUri{parameter}");
            }
        }
    }
}
