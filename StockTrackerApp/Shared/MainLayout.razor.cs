using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Shared
{
    public partial class MainLayout
    {
        [Inject] private IAuthorizationService _authorizationService { get; set; }
        [Inject] private IMessageListenerService _messageListenerService { get; set; }
        [Inject] private IBroadcastListenerService _broadcastListenerService { get; set; }

        //declare variables
        private static Message _message { get; set; }
        private static Broadcast _broadcast { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (_message != null)
            {
                NewMessageReceived(this, _message);
            }
            _messageListenerService.MessageReceived += (sender, message) => InvokeAsync(() => NewMessageReceived(sender, message));
            _broadcastListenerService.BroadcastReceived += (sender, broadcast) => InvokeAsync(() => NewBroadcastReceived(sender, broadcast));

        }

        private async Task NewMessageReceived(object sender, Message message)
        {
            _message = message;
            StateHasChanged();
            await Task.Delay(8000);
            _message = null;
            StateHasChanged();
        }

        private async Task NewBroadcastReceived(object sender, Broadcast broadcast)
        {
            _broadcast = broadcast;
            StateHasChanged();
            await Task.Delay(8000);
            _broadcast = null;
            StateHasChanged();
        }


    }
}
