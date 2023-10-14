using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.SupplierComponents
{
    public partial class Home
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService {  get; set; }

        [Inject]
        private IUserService _userService { get; set; }

        [Inject]
        private IJSRuntime _jSRuntime { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }


        private Popup popupRef = new();

        //Define Variables
        private List<Stock> _stock = new List<Stock>();
        private List<Product> _products = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        private Stock _selectedStock = new Stock();
        private bool _isStockSelected = false;

        //private List<string> _selectedCategories = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            Init();
        }

        private async Task Init()
        {
            //Get all of the stock which belongs to the current user (the supplier)
            _stock = _supplierService.GetStockBySupplier(_userService.CurrentUser.UserId);

            //Get a list of Product Ids
            List<string> productIds = _stock.Select(prod => prod.ProductId.ToString()).ToList();
            _products = _supplierService.GetProductsByProductIds(productIds);

            //Get a list of the product category ids
            List<string> productCategoryIds = _products.Select(cat => cat.ProductCategoryId.ToString()).Distinct().ToList();
            _productCategories = _supplierService.GetProductCategoriesByProductCategoryIds(productCategoryIds);
        }

        private void HandleRowClick(Stock stock)
        {
            Console.WriteLine("hello there");
        }

        private void RowSelected(Stock stock)
        {
            _selectedStock = stock;
            _isStockSelected = true;
        }

        /// <summary>
        /// Creates a popup asking if user would like to delete the selected stock item
        /// if yes, the stock item is deleted and we rerender the page
        /// </summary>
        private async void DeleteClicked()
        {
            bool confirmed = await _jSRuntime.InvokeAsync<bool>("confirm", "Are you sure you would like to delete this Stock Item?");
            if (confirmed)
            {
                if (_supplierService.DeleteStockByStockID(_selectedStock.StockId) == true)
                {
                    await _jSRuntime.InvokeAsync<object>("alert", "Stock item successfully deleted");
                    _navManager.NavigateTo("Home", true);
                }
            }

        }

    }
}
