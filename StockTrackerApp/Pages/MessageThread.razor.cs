using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages
{
    public partial class MessageThread
    {
        //inject services
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IAuthorizationService _authorizationService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IMessageService _messageService { get; set; }
        [Inject] private IMessageListenerService _messageListenerService { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }

        //declare variables
        private List<Message> _messages { get; set; }
        private Supplier _supplier { get; set; }
        private Customer _customer { get; set; }
        private string _currentUserId;
        private string _newMessage { get; set; }

        //declare parameters
        [Parameter] public string ContactId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("MessageThread");
            _messageListenerService.MessageReceived += (sender, message) => InvokeAsync(() => NewMessageReceived(sender, message));
            //NavigationManager.LocationChanged += OnLeave;
            Init();
        }

        //protected override void OnAfterRender(bool firstRender)
        //{
        //    base.OnAfterRender(firstRender);
        //    if (firstRender)
        //    {
        //        _navManager.RegisterLocationChangingHandler(async (location) => await OnLocationChanging(location));
        //    }
        //}

        private void Init()
        {
            if (ContactId != null)
            {
                //Get our objects based on usertype
                if (_authorizationService.UserType == UserType.Supplier)
                {
                    _currentUserId = _supplierService.CurrentUser.SupplierId;
                    _messages = _messageService.GetMessageThreads(_supplierService.CurrentUser.SupplierId , ContactId);
                    _customer = _customerService.GetCustomerByCustomerId(ContactId);
                }
                else if (_authorizationService.UserType == UserType.Customer)
                {
                    _currentUserId = _customerService.CurrentUser.CustomerId;
                    _messages = _messageService.GetMessageThreads(_customerService.CurrentUser.CustomerId, ContactId);
                    _supplier = _supplierService.GetSupplierBySupplierId(ContactId);
                }
            }
        }

        private void SendMessage()
        {
            //return if there is no message
            if (String.IsNullOrWhiteSpace(_newMessage))
            {
                return;
            }
            
            bool messageSent = _messageService.SendMessage(ContactId, _newMessage);

            if (messageSent)
            {
               _navManager.NavigateTo($"/MessageThread/{ContactId}", true);
            }
        }

        private void NewMessageReceived(object sender, Message newMessage)
        {
            if (_sessionHistoryService.GetCurrentWebpage() == "MessageThread")
            {
                if (newMessage.SenderId == ContactId)
                {
                    _navManager.NavigateTo($"/MessageThread/{ContactId}", true);
                }
            }      
        }

        private void OnLeave(object sender, LocationChangedEventArgs args)
        {
            _messageListenerService.MessageReceived -= (sender, message) => InvokeAsync(() => NewMessageReceived(sender, message));
        }

        //private async ValueTask OnLocationChanging(object args)
        //{
        //    _messageListenerService.MessageReceived -= (sender, message) => InvokeAsync(() => NewMessageReceived(sender, message));
        //    await Task.CompletedTask;
        //}
    }
}
