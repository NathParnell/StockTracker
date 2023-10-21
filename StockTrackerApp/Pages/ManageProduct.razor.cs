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
        private NavigationService _navService { get; set; }

        [Inject]
        private IUserService _userService { get; set; }

        //Define variables
        List<Product> _existingProducts = new List<Product>();
        List<ProductCategory> _productCategories = new List<ProductCategory>();

        private Product _newProduct = new Product();
        private string _size;
        private string _measurementUnit;

        protected override async Task OnInitializedAsync()
        {
            GetExistingProductsInformation();
        }

        private async Task GetExistingProductsInformation()
        {
            _productCategories = _supplierService.GetAllProductCategories();
            _existingProducts = _supplierService.GetAllProducts();
        }

        private async Task AddProduct()
        {
            //Add the product measurement unit to the product object
            if (Enum.TryParse(_measurementUnit, out ProductMeasurementUnit parsedMeasurementUnit))
                _newProduct.ProductMeasurementUnit = parsedMeasurementUnit;
            else
                _newProduct.ProductMeasurementUnit = null; // or any other default value

            //set the supplier id to be the id of the current user (who is a suppier in this case)
            _newProduct.SupplierId = _userService.CurrentUser.UserId;

            string prompt = _supplierService.ValidateAndAddProduct(_newProduct, _existingProducts, _productCategories);

            await _jSRuntime.InvokeAsync<object>("alert", prompt);
            _navService.NavigateTo("ManageProduct", true);
        }

        private async Task AddProductCategory()
        {
            _navService.NavigateTo("AddProductCategory", true);
        }

    }
}
