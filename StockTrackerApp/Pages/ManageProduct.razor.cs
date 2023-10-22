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

namespace StockTrackerApp.Pages
{
    public partial class ManageProduct
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService { get; set; }

        [Inject]
        private IJSRuntime _jSRuntime { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        [Inject]
        private IUserService _userService { get; set; }

        //Define Parameters
        [Parameter]
        public string ProductId { get; set; }

        //Define variables
        private List<Product> _existingProducts = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();
        private Product _product = new Product();
        private string _measurementUnit;
        private ManageProductPageState _pageState;
        private bool _isStateReadOnly = false;

        protected override async Task OnInitializedAsync()
        {
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
        }

        private async Task GetExistingProductsInformation()
        {
            _productCategories = _supplierService.GetAllProductCategories();
            _existingProducts = _supplierService.GetAllProducts();

            if (_pageState == ManageProductPageState.ViewProductMode)
                _product = _supplierService.GetProductByProductId(ProductId);
        }

        private async Task ProductAction()
        {
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
                _product.SupplierId = _userService.CurrentUser.UserId;

                prompt = _supplierService.ValidateAndAddProduct(_product, _existingProducts, _productCategories);
            }
            //if the user is updating an existing product, then we attempt to validate and update the product
            else if (_pageState == ManageProductPageState.EditProductMode)
            {
                prompt = _supplierService.ValidateAndUpdateProduct(_product, _existingProducts, _productCategories);
            }


            await _jSRuntime.InvokeAsync<object>("alert", prompt);
            _navManager.NavigateTo("ManageProduct", true);
        }

        private async Task AddProductCategory()
        {
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
