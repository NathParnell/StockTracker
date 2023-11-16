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
    public partial class MessagePopup
    {
        //Inject services
        [Inject] IAuthorizationService _authorizationService { get; set; }
        [Inject] ISupplierService _supplierService { get; set; }
        [Inject] ICustomerService _customerService { get; set; }
        [Inject] NavigationManager _navManager { get; set; }

        //declare parameters
        [Parameter] public Message Message { get; set; }

        //declare variables
        private string _senderName { get; set; }
        private bool _showPopup { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (Message == null)
            {
                Message = new Message();
            }

            if (_authorizationService.IsLoggedIn)
            {
                if (_authorizationService.UserType == UserType.Supplier)
                {
                    Customer customer = _customerService.GetCustomerByCustomerId(Message.SenderId);
                    _senderName = $"{customer.FirstNames} {customer.Surname}";
                }
                else if (_authorizationService.UserType == UserType.Customer)
                {
                    Supplier supplier = _supplierService.GetSupplierBySupplierId(Message.SenderId);
                    _senderName = supplier.CompanyName;
                }
            }
        }

        private async Task NavigateToMessageThread()
        {
            _navManager.NavigateTo($"/MessageThread/{Message.SenderId}");
            Message = new Message();
            _showPopup = false;
            StateHasChanged();
        }

        private void Close()
        {
            _showPopup = false;
            StateHasChanged();
        }

    }
}
