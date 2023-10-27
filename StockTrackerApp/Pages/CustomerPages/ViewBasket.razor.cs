using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages.CustomerPages
{
    public partial class ViewBasket
    {
        //Inject Services
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private IProductCategoryService _productCategoryService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IOrderService _orderService { get; set; }
        [Inject] private NavigationManager _navmanager { get; set; }

        //Define Variables
        List<Supplier> _suppliers = new List<Supplier>();
        List<Product> _basketProducts = new List<Product>();
        private Dictionary<string, int>? _productBasketQuantities = new Dictionary<string, int>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("ViewBasket");
            Init();
        }

        private async Task Init()
        {
            //make all this better as it has lots of db calls which are unnecessary
            //Get The Suppliers who the users have basket items of
            foreach (OrderItem basketItem in _orderService.BasketItems)
            {
                Supplier supplier = _supplierService.GetSupplierBySupplierId(_productService.GetProductByProductId(basketItem.ProductId).SupplierId);
                if (_suppliers.Any(supp => supp.SupplierId == supplier.SupplierId) == false)
                    _suppliers.Add(supplier);
            }

            //Get the products which are in the users basket
            foreach (OrderItem basketItem in _orderService.BasketItems)
            {
                _basketProducts.Add(_productService.GetProductByProductId(basketItem.ProductId));
            }

            GetBasketItems();
        }

        private void GetBasketItems()
        {
            foreach (OrderItem basketItem in _orderService.BasketItems)
            {
                UpdateProductQuantity(basketItem.ProductId, basketItem.Quantity);
            }

            foreach (Product product in _basketProducts)
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

            //we reload the page to update the basket
            _navmanager.NavigateTo("ViewBasket", true);
        }

        private void CreateOrderFromSupplier(Supplier supplier)
        {

        }
    }
}
