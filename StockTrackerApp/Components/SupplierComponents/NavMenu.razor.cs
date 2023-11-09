using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.SupplierComponents
{
    public partial class NavMenu
    {
        //Inject Services
        [Inject] private NavigationManager _navManager { get; set; }

        [Inject] private ISupplierService _supplierService { get; set; }

        [Inject] private ICustomerService _customerService { get; set; }

        [Inject] private IAuthorizationService _authorizationService { get; set; }


        private async Task NavigateHome()
        {
            _navManager.NavigateTo("Home", true);
        }
        private async Task NavigateMessages()
        {
            _navManager.NavigateTo("Messages", true);
        }
        private async Task NavigateOrderRequests()
        {
            _navManager.NavigateTo("OrderRequests", true);
        }
        private async Task Logout()
        {
            if (_supplierService.Logout())
                _navManager.NavigateTo("", true);
        }
    }
}
