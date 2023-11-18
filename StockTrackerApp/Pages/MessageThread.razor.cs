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

        private void NavigatePreviousPage()
        {
            string previousWebpage = _sessionHistoryService.GetPreviousWebpage();
            if (previousWebpage != null)
            {
                _navManager.NavigateTo(previousWebpage, true);
            }
            else
            {
                _navManager.NavigateTo("Home", true);
            }
        }

    }
}
