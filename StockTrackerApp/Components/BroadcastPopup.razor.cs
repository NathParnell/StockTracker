using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components
{
    public partial class BroadcastPopup
    {
        //Inject Services
        [Inject] IAuthorizationService _authorizationService { get; set; }
        [Inject] ISupplierService _supplierService { get; set; }
        [Inject] ICustomerService _customerService { get; set; }

        //Declare Parameters
        [Parameter] public Broadcast Broadcast { get; set; }

        //Declare Variables
        private string _senderName { get; set; }
        private bool _showPopup { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (Broadcast == null)
            {
                Broadcast = new Broadcast();
            }

            if (_authorizationService.IsLoggedIn)
            {
                if (_authorizationService.UserType == UserType.Supplier)
                {
                    Customer customer = _customerService.GetCustomerByCustomerId(Broadcast.SenderId);
                    _senderName = $"{customer.FirstNames} {customer.Surname}";
                }
                else if (_authorizationService.UserType == UserType.Customer)
                {
                    Supplier supplier = _supplierService.GetSupplierBySupplierId(Broadcast.SenderId);
                    _senderName = supplier.CompanyName;
                }
            }
        }

        private void Close()
        {
            _showPopup = false;
            StateHasChanged();
        }
    }
}
