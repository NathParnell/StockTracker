﻿using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.CustomerComponents
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
        private async Task NavigateViewProducts()
        {
            _navManager.NavigateTo("ViewProducts", true);
        }
        private async Task NavigateBasket()
        {
            _navManager.NavigateTo("ViewBasket", true);
        }
        private async Task NavigateOrderHistory()
        {
            _navManager.NavigateTo("OrderHistory", true);
        }
        private async Task NavigateMessages()
        {
            _navManager.NavigateTo("Messages", true);
        }

        private async Task NavigateNotifications()
        {
            _navManager.NavigateTo("Notifications", true);
        }
        private async Task Logout()
        {
            if (_customerService.Logout())
                _navManager.NavigateTo("", true);
        }
    }
}
