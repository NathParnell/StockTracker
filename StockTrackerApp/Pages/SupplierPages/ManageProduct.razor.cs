using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StockTrackerApp.Services;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages.SupplierPages
{
    public partial class ManageProduct
    {
        //Inject Services
        [Inject] private IJSRuntime _jSRuntime { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IProductCategoryService _productCategoryService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }


        //Define Parameters
        [Parameter]
        public string ProductId { get; set; }

        //Define variables
        private List<Product> _existingProducts = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();
        private Product _product = new Product();
        private string _measurementUnit;

        //define state variables
        private ManageProductPageState _pageState;
        private bool _isStateReadOnly = false;

        //define html string variables
        private string _productActionButtonText = "";
        private string _manageProductTitleText = "";

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("ManageProduct");
            SetPageState();
            GetExistingProductsInformation();
        }

        private async Task SetPageState(ManageProductPageState state = ManageProductPageState.Undefined)
        {
            //if we have passed through a page state, then we set the page state to this
            if (state != ManageProductPageState.Undefined)
            {
                _pageState = state;
            }
            else
            {
                //if there is no product id, then we assume it is a new product
                if (String.IsNullOrEmpty(ProductId))
                    _pageState = ManageProductPageState.NewProductMode;
                //if there is a product id then we assume the user is viewing a product
                else
                    _pageState = ManageProductPageState.ViewProductMode;
            }

            if (_pageState == ManageProductPageState.ViewProductMode)
                _isStateReadOnly = true;
            else
                _isStateReadOnly = false;

            SetTitleText();
            SetProductActionButtonText();
        }

        private async Task GetExistingProductsInformation()
        {
            _productCategories = _productCategoryService.GetAllProductCategories();
            _existingProducts = _productService.GetAllProducts();

            if (_pageState == ManageProductPageState.ViewProductMode)
            {
                _product = _productService.GetProductByProductId(ProductId);
                _measurementUnit = _product.ProductMeasurementUnit.ToString();
            }

        }

        private async Task SetTitleText()
        {
            if (_pageState == ManageProductPageState.NewProductMode)
                _manageProductTitleText = "Add New Product";
            else if (_pageState == ManageProductPageState.ViewProductMode)
                _manageProductTitleText = "View Product";
            else
                _manageProductTitleText = "Update Product";
        }

        /// <summary>
        /// Set the text of the action button to match the page state
        /// </summary>
        /// <returns></returns>
        private async Task SetProductActionButtonText()
        {
            if (_pageState == ManageProductPageState.NewProductMode)
                _productActionButtonText = "Add Product";
            else if (_pageState == ManageProductPageState.ViewProductMode)
                _productActionButtonText = "Edit Product";
            else
                _productActionButtonText = "Update Product";
        }

        private async Task ProductAction()
        {
            bool actionSuccess = false;
            //if the user wished to enter the edit product mode
            if (_pageState == ManageProductPageState.ViewProductMode)
            {
                SetPageState(ManageProductPageState.EditProductMode);
                return;
            }

            //Add the product measurement unit to the product object
            if (Enum.TryParse(_measurementUnit, out ProductMeasurementUnit parsedMeasurementUnit))
                _product.ProductMeasurementUnit = parsedMeasurementUnit;
            else
                _product.ProductMeasurementUnit = null; // or any other default value

            string prompt = "";

            //if the user is entering a new product then we attempt to validate and add the new product
            if (_pageState == ManageProductPageState.NewProductMode)
            {
                //set the supplier id to be the id of the current user (who is a suppier in this case)
                _product.SupplierId = _supplierService.CurrentUser.SupplierId;

                prompt = _productService.ValidateAndAddProduct(_product, _existingProducts, _productCategories, ref actionSuccess);
            }
            //if the user is updating an existing product, then we attempt to validate and update the product
            else if (_pageState == ManageProductPageState.EditProductMode)
            {
                prompt = _productService.ValidateAndUpdateProduct(_product, _existingProducts, _productCategories, ref actionSuccess);
            }

            await _jSRuntime.InvokeAsync<object>("alert", prompt);
            if (actionSuccess)
            {
                _navManager.NavigateTo($"ManageProduct/{_product.ProductId}", true);
            }
        }

        private async Task ViewProduct()
        {
            _navManager.NavigateTo($"ManageProduct/{ProductId}", true);
        }

        private async Task AddProductCategory()
        {
            if (_pageState == ManageProductPageState.EditProductMode)
                _navManager.NavigateTo($"AddProductCategory/{_product.ProductId}", true);
            else
                _navManager.NavigateTo("AddProductCategory", true);
        }
    }


    /// <summary>
    /// Create Type for the state of this control
    /// </summary>
    public enum ManageProductPageState
    {
        NewProductMode = 0,
        ViewProductMode = 1,
        EditProductMode = 2,
        Undefined = 3
    }
}
