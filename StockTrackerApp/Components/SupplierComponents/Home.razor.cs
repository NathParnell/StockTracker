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
    /// <summary>
    /// Supplier Homepage Component
    /// </summary>
    public partial class Home
    {
        //Inject Services
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IProductCategoryService _productCategoryService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private IJSRuntime _jSRuntime { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }

        //Define Variables
        private List<Product> _products = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        private Product _selectedProduct = new Product();
        private bool _isProductSelected = false;

        private List<String> _sortByOptions = new List<String>() { "Product Name (A-Z)", "Product Name (Z-A)", "Product Brand (A-Z)", "Product Brand (Z-A)", "Price (Low to High)", "Price (High to Low)",
            "Quantity (Low to High)", "Quantity (High to Low)"};

        //private List<string> _selectedCategories = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("Home");
            GetProductsInformation();
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
        private void SortProducts (string sortByOption)
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
                case "Quantity (Low to High)":
                    _products = _products.OrderBy(prod => prod.ProductQuantity).ToList();
                    break;
                case "Quantity (High to Low)":
                    _products = _products.OrderByDescending(prod => prod.ProductQuantity).ToList();
                    break;
                default:
                    break;
            }
        }

        private async Task GetProductsInformation()
        {
            //Get all of the products which belong to the current user (the supplier)
            _products = _productService.GetProductsBySupplierId(_supplierService.CurrentUser.SupplierId);

            //Get a list of the product category ids
            List<string> productCategoryIds = _products.Select(cat => cat.ProductCategoryId.ToString()).Distinct().ToList();
            _productCategories = _productCategoryService.GetProductCategoriesByProductCategoryIds(productCategoryIds);

            //we need to sort the products by name
            SortProducts(_sortByOptions[0]);
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
