using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.CustomerComponents
{
    public partial class Messages
    {
        //Inject Services
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IMessageService _messageService { get; set; }

        //declare variables
        private List<string> _contactIds { get; set; }
        private List<Supplier> _suppliers { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("Messages");
            Init();
        }

        private async Task Init()
        {
            _contactIds = _messageService.GetContactIds(_customerService.CurrentUser.CustomerId);
            if (_contactIds != null)
            {
                _suppliers = _supplierService.GetSuppliersBySupplierIDs(_contactIds);
            }

        }
    }
}
