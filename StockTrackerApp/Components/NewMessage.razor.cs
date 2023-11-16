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
    public partial class NewMessage
    {
        //Inject Services
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IAuthorizationService _authorizationService { get; set; }
        [Inject] private IMessageService _messageService { get; set; }

        //Declare Variables
        private List<Customer> _customers { get; set; }
        private List<Supplier> _suppliers { get; set; }
        private string _newMessage { get; set; }
        private List<string> _recipientIds { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Init();
        }

        private void Init()
        {
            _newMessage = String.Empty;
            _recipientIds = new List<string>();

            if (_authorizationService.UserType == UserType.Supplier)
            {
                _customers = _customerService.GetAllCustomers();
            }
            else if (_authorizationService.UserType == UserType.Customer)
            {
                _suppliers = _supplierService.GetAllSuppliers();
            }
        }

        private void AddRecipient(ChangeEventArgs e)
        {
            string recipientId = e.Value.ToString();
            if (_recipientIds.Contains(recipientId) == false && String.IsNullOrWhiteSpace(recipientId) == false)
            {
                _recipientIds.Add(recipientId);
                StateHasChanged();
            }
        }

        private void RemoveRecipient(string recipientId)
        {
            if (_recipientIds.Contains(recipientId))
            {
                _recipientIds.Remove(recipientId);
                StateHasChanged();
            }
        }

        private void SendMessage()
        {
            //return if there is no message
            if (String.IsNullOrWhiteSpace(_newMessage))
            {
                return;
            }

            //ensure that we have a recipient and a message
            if (_recipientIds != null && _recipientIds.Count > 0 && String.IsNullOrWhiteSpace(_newMessage) == false)
            {
                //if the message is sent successfully, then we reset the page
                if (_messageService.SendMessage(_recipientIds, _newMessage))
                {
                    Init();
                    StateHasChanged();
                }
            }
            else
            {
                 //need to display to user
            }
        }

    }
}
