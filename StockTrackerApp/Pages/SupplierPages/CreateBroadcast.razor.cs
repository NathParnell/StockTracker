using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages.SupplierPages
{
    public partial class CreateBroadcast
    {
        //Inject Services
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IBroadcastService _broadcastService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }

        //Declare Variables
        private string _broadcastSubject { get; set; }
        private string _broadcastMessageBody { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _sessionHistoryService.AddWebpageToHistory("CreateBroadcast");
        }

        private void SendBroadcast()
        {
            //Ensure that user has entered a subject and message body
            if (String.IsNullOrWhiteSpace(_broadcastSubject) || String.IsNullOrWhiteSpace(_broadcastMessageBody))
            {
                return;
            }

            Broadcast broadcast = new Broadcast()
            {
                Topic = _supplierService.CurrentUser.SupplierId,
                SenderId = _supplierService.CurrentUser.SupplierId,
                Subject = _broadcastSubject,
                MessageBody = _broadcastMessageBody,
            };

            bool broadcastSent = _broadcastService.BroadcastMessage(broadcast.Topic, broadcast.MessageBody, broadcast.Subject);
            if (broadcastSent)
            {
                _broadcastSubject = String.Empty;
                _broadcastMessageBody = String.Empty;
            }

        }
    }
}
