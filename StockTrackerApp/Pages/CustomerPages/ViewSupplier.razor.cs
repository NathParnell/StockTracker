using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages.CustomerPages
{
    public partial class ViewSupplier
    {
        //Inject Services
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private IProductCategoryService _productCategoryService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IBroadcastListenerService _broadcastListenerService { get; set; }
        [Inject] private IOrderService _orderService { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }

        //Define Parameters
        [Parameter] public string SupplierId { get; set; }

        //Define Variables
        private Supplier _supplier = new Supplier();
        private List<Product> _products = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();
        private Dictionary<string, int>? _productBasketQuantities = new Dictionary<string, int>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("ViewSupplier");
            Init();
        }

        private void Init()
        {
            _supplier = _supplierService.GetSupplierBySupplierId(SupplierId);
            _productCategories = _productCategoryService.GetAllProductCategories();
            _products = _productService.GetProductsBySupplierId(SupplierId);
            GetBasketItems();
        }

        private void GetBasketItems()
        {
            foreach (OrderItem basketItem in _orderService.BasketItems)
            {
                UpdateProductQuantity(basketItem.ProductId, basketItem.Quantity);
            }

            foreach (Product product in _products)
            {
                if (_productBasketQuantities.ContainsKey(product.ProductId) == false)
                {
                    _productBasketQuantities.Add(product.ProductId, 0);
                }
            }
        }

        private void UpdateProductQuantity(string productId, int quantity)
        {
            if (_productBasketQuantities.ContainsKey(productId))
            {
                _productBasketQuantities[productId] = quantity;
            }
            else
            {
                _productBasketQuantities.Add(productId, quantity);
            }
        }

        private void UpdateBasket(Product product)
        {
            OrderItem basketItem = new OrderItem()
            {
                OrderItemId = Taikandi.SequentialGuid.NewGuid().ToString(),
                ProductId = product.ProductId,
                Quantity = _productBasketQuantities[product.ProductId],
                OrderPrice = _productBasketQuantities[product.ProductId] * product.Price
            };

            _orderService.AddItemToBasket(basketItem, product.Price);
        }

        public void ViewBasket()
        {
            _navManager.NavigateTo("ViewBasket");
        }

        private void ManageProductSubscription(string productId)
        {
            Customer currentCustomer = _customerService.CurrentUser;
            if (currentCustomer.ProductSubscriptions.Contains(productId))
            {
                currentCustomer.ProductSubscriptions.Remove(productId);
            }
            else
            {
                currentCustomer.ProductSubscriptions.Add(productId);
            }

            //if the customer gets updated then we need to refresh the page
            if (_customerService.UpdateCustomer(currentCustomer))
            {
                // Create one list with all of the customers subscriptions
                List<string> customerSubscriptions = new List<string>();
                customerSubscriptions.AddRange(_customerService.CurrentUser.ProductSubscriptions);
                customerSubscriptions.AddRange(_customerService.CurrentUser.SupplierSubscriptions);

                // Restart the Broadcast Listener
                _broadcastListenerService.RestartListener(customerSubscriptions);

                Init();
                StateHasChanged();
            }
        }

    }
}
