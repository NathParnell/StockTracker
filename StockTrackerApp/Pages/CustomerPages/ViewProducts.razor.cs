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
    public partial class ViewProducts
    {
        //Inject Services
        [Inject] private IProductCategoryService _productCategoryService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IBroadcastListenerService _broadcastListenerService { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private IOrderService _orderService { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }

        // Define Variables
        private List<Product> _products = new List<Product>();
        private List<Supplier> _suppliers = new List<Supplier>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        private Dictionary<string, int>? _productBasketQuantities = new Dictionary<string, int>();
        private readonly List<String> _sortByOptions = new List<String>() { "Product Name (A-Z)", "Product Name (Z-A)", "Product Brand (A-Z)", "Product Brand (Z-A)", "Price (Low to High)", "Price (High to Low)" };

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("ViewProducts");
            Init();
        }

        private void Init()
        {
            //get all of the products with stock that can be sold on stock tracker
            _products = _productService.GetAllProductsWithStock();
            _suppliers = _supplierService.GetAllSuppliers();

            List<string> productCategoryIds = _products.Select(cat => cat.ProductCategoryId.ToString()).Distinct().ToList();
            _productCategories = _productCategoryService.GetProductCategoriesByProductCategoryIds(productCategoryIds);

            //we need to set the basket quantities for each product
            GetBasketItems();

            //we need to sort the products by name
            SortProducts(_sortByOptions[0]);
        }

        /// <summary>
        /// Sorts the products based on the selected option and re-renders the page
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task SortProducts(ChangeEventArgs e)
        {
            SortProducts(e.Value.ToString());
            StateHasChanged();
        }

        /// <summary>
        /// Sorts the products based on the selected option
        /// </summary>
        /// <param name="sortByOption"></param>
        /// <returns></returns>
        private void SortProducts(string sortByOption)
        {
            //Sort the products based on the selected option
            switch (sortByOption)
            {
                case "Product Name (A-Z)":
                    _products = _products.OrderBy(prod => prod.ProductName).ToList();
                    break;
                case "Product Name (Z-A)":
                    _products = _products.OrderByDescending(prod => prod.ProductName).ToList();
                    break;
                case "Product Brand (A-Z)":
                    _products = _products.OrderBy(prod => prod.ProductBrand).ToList();
                    break;
                case "Product Brand (Z-A)":
                    _products = _products.OrderByDescending(prod => prod.ProductBrand).ToList();
                    break;
                case "Price (Low to High)":
                    _products = _products.OrderBy(prod => prod.Price).ToList();
                    break;
                case "Price (High to Low)":
                    _products = _products.OrderByDescending(prod => prod.Price).ToList();
                    break;
                default:
                    break;
            }
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

        private void NavigateNewMessage(string customerId)
        {
            _navManager.NavigateTo($"NewMessage/{customerId}", true);
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
