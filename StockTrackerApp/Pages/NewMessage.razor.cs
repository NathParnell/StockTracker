using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages
{
    public partial class NewMessage
    {
        //Inject Services
        [Inject] ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] NavigationManager _navManager { get; set; }

        //Declare parameters
        [Parameter]
        public string RecipientIds { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _sessionHistoryService.AddWebpageToHistory("NewMessage");

            if (String.IsNullOrWhiteSpace(RecipientIds))
            {
                RecipientIds = "";
            }
        }

        private void NavigatePreviousPage()
        {
            string previousWebpage = _sessionHistoryService.GetPreviousWebpage();
            if (previousWebpage != null)
            {
                _navManager.NavigateTo(previousWebpage, true);
            }
            else
            {
                _navManager.NavigateTo("Home", true);
            }
        }
    }
}
