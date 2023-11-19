using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages.CustomerPages
{
    public partial class Notifications
    {
        //Inject Services
        [Inject] private IBroadcastListenerService _broadcastListenerService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }

        //Declare variables
        private List<Supplier> _suppliers = new List<Supplier>();


        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _sessionHistoryService.AddWebpageToHistory("Notifications");
            Init();
        }

        private void Init()
        {
            _broadcastListenerService.BroadcastReceived += BroadcastReceived;
            _suppliers = _supplierService.GetAllSuppliers();
        }

        /// <summary>
        /// Upon receiving a new broadcast, refresh the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BroadcastReceived(object sender, StockTrackerCommon.Models.Broadcast e)
        {
            if (_sessionHistoryService.GetCurrentWebpage() == "Notifications")
            {
                _navManager.NavigateTo("Notifications", true);
            }        
        }
    }
}
