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
    public partial class OrderRequests
    {
        //Inject Services
        [Inject] private IOrderService _orderService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }

        //Define Variables
        private List<Order> _orderRequests = new List<Order>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("OrderRequests");
            Init();
        }

        private void Init()
        {
            _orderRequests = _orderService.GetSuppliersOrderRequests(_supplierService.CurrentUser.SupplierId);
        }

    }
}
