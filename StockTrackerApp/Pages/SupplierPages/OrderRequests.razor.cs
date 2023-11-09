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
        [Inject] private IProductService _productService { get; set; }

        //Define Variables
        private List<Order> _orderRequests = new List<Order>();
        private List<OrderItem> _orderItemsInRequests = new List<OrderItem>();
        private List<Customer> _customers = new List<Customer>();
        private List<Product> _productsRequested = new List<Product>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("OrderRequests");
            Init();
        }

        public void Refresh()
        {
            Init();
            StateHasChanged();
        }

        private void Init()
        {
            //get the order requests for the supplier
            _orderRequests = _orderService.GetOrderRequestsBySupplierId(_supplierService.CurrentUser.SupplierId);

            //get the customers who have made an order request
            _customers = _customerService.GetCustomersByCustomerIds(_orderRequests.Select(order => order.CustomerId).ToList());
            
            //get the order items for all orders
            List<string> orderItemIds  = _orderRequests.SelectMany(order => order.OrderItemIds).ToList();
            _orderItemsInRequests = _orderService.GetOrderItemsByOrderItemIds(orderItemIds);

            //get the products for all order items
            _productsRequested = _productService.GetProductsByProductIds(_orderItemsInRequests.Select(orderItem => orderItem.ProductId).ToList());
        }

        private void AcceptOrderRequest(Order order)
        {
            //update the order request have a status of accepted
            order.OrderStatus = OrderStatus.Accepted;
            _orderService.UpdateOrder(order);

            //get the product ids of the order items which are in the order request
            List<string> productIds = _orderItemsInRequests.Where(oi => order.OrderItemIds.Contains(oi.OrderItemId))
                .Select(orderItem => orderItem.ProductId).ToList();

            //update the product stock levels which have just been accepted
            foreach (string productId in productIds)
            {
                Product product = _productsRequested.Where(p => p.ProductId == productId).FirstOrDefault();
                //update the product stock level
                product.ProductQuantity = product.ProductQuantity - _orderItemsInRequests.Where(oi => oi.ProductId == productId).Sum(oi => oi.Quantity);
                _productService.UpdateProduct(product);
            }

            //Refresh the page
            Refresh();
        }

        private void RejectOrderRequest(Order order)
        {
            //update the order request have a status of accepted
            order.OrderStatus = OrderStatus.Rejected;
            _orderService.UpdateOrder(order);

            //Refresh the page
            Refresh();
        }
    }
}
