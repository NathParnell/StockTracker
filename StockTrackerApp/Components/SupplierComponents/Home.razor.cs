using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockTrackerApp.Services;
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
        private IUserService _userService { get; set; }

        [Inject]
        private IProductCategoryService _productCategoryService { get; set; }

        [Inject]
        private IProductService _productService { get; set; }

        [Inject]
        private IJSRuntime _jSRuntime { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        [Inject]
        private ISessionHistoryService _sessionHistoryService { get; set; }


        //Define Variables
        private List<Product> _products = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        private Product _selectedProduct = new Product();
        private bool _isProductSelected = false;

        //private List<string> _selectedCategories = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("Home");
            GetProductsInformation();
        }

        private async Task GetProductsInformation()
        {
            //Get all of the products which belong to the current user (the supplier)
            _products = _productService.GetProductBySupplierId(_userService.CurrentUser.UserId);

            //Get a list of the product category ids
            List<string> productCategoryIds = _products.Select(cat => cat.ProductCategoryId.ToString()).Distinct().ToList();
            _productCategories = _productCategoryService.GetProductCategoriesByProductCategoryIds(productCategoryIds);
        }

        private void RowSelected(Product product)
        {
            _selectedProduct = product;
            _isProductSelected = true;
        }

        /// <summary>
        /// Creates a popup asking if user would like to delete the selected product
        /// if yes, the product is deleted and we rerender the page
        /// </summary>
        private async void DeleteProduct()
        {
            bool confirmed = await _jSRuntime.InvokeAsync<bool>("confirm", "Are you sure you would like to delete this Product?");
            if (confirmed)
            {
                if (_productService.DeleteProductByProductID(_selectedProduct.ProductId) == true)
                {
                    await _jSRuntime.InvokeAsync<object>("alert", "Product successfully deleted");
                    _navManager.NavigateTo("Home", true);
                }
            }
        }

        private async void AddProduct()
        {
            _navManager.NavigateTo("ManageProduct", true);
        }

        private async void EditProduct(Product product)
        {
            _navManager.NavigateTo($"ManageProduct/{product.ProductId}", true);
        }

    }
}
