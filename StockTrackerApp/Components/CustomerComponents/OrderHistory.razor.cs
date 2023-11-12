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
    /// <summary>
    /// Customer implementation of OrderHistory page which shows customers their accepted orders, pending orders and rejected orders
    /// </summary>
    public partial class OrderHistory
    {
        //Inject Services
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private IOrderService _orderService { get; set; }


        //Define Variables
        private List<Order> _orders { get; set; }
        private List<OrderItem> _orderItems { get; set; }
        private List<Supplier> _suppliers { get; set; }
        private List<Product> _orderedProducts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("OrderHistory");
            Init();
        }

        private void Init()
        {
            _orders = _orderService.GetOrdersByUserId(_customerService.CurrentUser.CustomerId);
            if (_orders != null)
            {
                _orderItems = _orderService.GetOrderItemsByOrderItemIds(_orders.SelectMany(x => x.OrderItemIds).ToList());
                _suppliers = _supplierService.GetSuppliersBySupplierIDs(_orders.Select(x => x.SupplierId).Distinct().ToList());
                _orderedProducts = _productService.GetProductsByProductIds(_orderItems.Select(x => x.ProductId).Distinct().ToList());
            }
        }

    }
}
