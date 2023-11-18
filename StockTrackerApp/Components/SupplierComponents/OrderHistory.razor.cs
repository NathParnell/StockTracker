using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.SupplierComponents
{
    /// <summary>
    /// Supplier implementation of OrderHistory page which shows suppliers their accepted orders and rejected orders
    /// </summary>
    public partial class OrderHistory
    {
        //Inject Services
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private IOrderService _orderService { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }


        //Define Variables
        private List<Order> _orders { get; set; }
        private List<OrderItem> _orderItems { get; set; }
        private List<Customer> _customers { get; set; }
        private List<Product> _orderedProducts { get; set; }


        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("OrderHistory");
            Init();
        }

        private void Init()
        {
            _orders = _orderService.GetOrdersByUserId(_supplierService.CurrentUser.SupplierId);
            if (_orders != null)
            {
                _orderItems = _orderService.GetOrderItemsByOrderItemIds(_orders.SelectMany(x => x.OrderItemIds).ToList());
                _customers = _customerService.GetCustomersByCustomerIds(_orders.Select(x => x.CustomerId).Distinct().ToList());
                _orderedProducts = _productService.GetProductsByProductIds(_orderItems.Select(x => x.ProductId).Distinct().ToList());
            }
        }

        private void NavigateNewMessage(string customerId)
        {
            _navManager.NavigateTo($"NewMessage/{customerId}", true);
        }
    }
}
