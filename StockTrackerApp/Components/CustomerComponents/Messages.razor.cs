using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.CustomerComponents
{
    public partial class Messages
    {
        //Inject Services
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }

        //declare variables
        private List<string> ContactIds { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("Messages");
            Init();
        }

        private async Task Init()
        {
            
        }
    }
}
