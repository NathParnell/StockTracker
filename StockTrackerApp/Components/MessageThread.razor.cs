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
    public partial class MessageThread
    {
        //Inject Services
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IAuthorizationService _authorizationService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IMessageService _messageService { get; set; }
        [Inject] private IMessageListenerService _messageListenerService { get; set; }


        //Declare Variables
        private List<Message> _messages { get; set; }
        private Supplier _supplier { get; set; }
        private Customer _customer { get; set; }
        private string _currentUserId;
        private string _newMessage { get; set; }

        //Declare Parameters
        [Parameter] public string ContactId { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _messageListenerService.MessageReceived += (sender, message) => InvokeAsync(() => NewMessageReceived(sender, message));
            Init();
        }

        private void Init()
        {
            if (ContactId != null)
            {
                //Get our objects based on usertype
                if (_authorizationService.UserType == UserType.Supplier)
                {
                    _currentUserId = _supplierService.CurrentUser.SupplierId;
                    _messages = _messageService.GetMessageThreads(_supplierService.CurrentUser.SupplierId, ContactId);
                    _customer = _customerService.GetCustomerByCustomerId(ContactId);
                }
                else if (_authorizationService.UserType == UserType.Customer)
                {
                    _currentUserId = _customerService.CurrentUser.CustomerId;
                    _messages = _messageService.GetMessageThreads(_customerService.CurrentUser.CustomerId, ContactId);
                    _supplier = _supplierService.GetSupplierBySupplierId(ContactId);
                }
            }
            _newMessage = String.Empty;
        }

        private void SendMessage()
        {
            //return if there is no message
            if (String.IsNullOrWhiteSpace(_newMessage))
            {
                return;
            }

            if (_messageService.SendMessage(ContactId, _newMessage))
            {
                Init();
                StateHasChanged();
            }
        }

        private void NewMessageReceived(object sender, Message newMessage)
        {
            //refresh the page if we are on the message thread page and the message is from the contact we are currently viewing
            if (_sessionHistoryService.GetCurrentWebpage() == "Messages" || _sessionHistoryService.GetCurrentWebpage() == "MessageThread")
            {
                if (newMessage.SenderId == ContactId)
                {
                    Init();
                    StateHasChanged();
                }
            }
        }
    }
}
