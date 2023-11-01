using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Shared
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
        private async Task NavigateBasket()
        {
            _navManager.NavigateTo("ViewBasket", true);
        }
        private async Task NavigateCounter()
        {
            _navManager.NavigateTo("Counter", true);
        }
        private async Task Logout()
        {
            _supplierService.Logout();
            _customerService.Logout();
            _navManager.NavigateTo("", true);
        }
    }
}
