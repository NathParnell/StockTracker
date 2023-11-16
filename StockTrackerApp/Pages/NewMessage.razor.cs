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

        //Declare parameters
        [Parameter]
        public string RecipientIds { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("NewMessage");

            if (String.IsNullOrWhiteSpace(RecipientIds))
            {
                RecipientIds = "";
            }
        }
    }
}
