using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
    public partial class AddProduct
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService { get; set; }

        [Inject]
        private IJSRuntime _jSRuntime { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

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

        private async Task Add()
        {
            //Add the product size to the new product object, if string cannot be parsed into a double then value is -1 which will be rejected 
            if (double.TryParse(_size, out double parsedSize))
                _newProduct.ProductSize = parsedSize;
            else
                _newProduct.ProductSize = -1; // or any other default value

            //Add the product measurement unit to the product objet
            if (Enum.TryParse(_measurementUnit, out ProductMeasurementUnit parsedMeasurementUnit))
                _newProduct.ProductMeasurementUnit = parsedMeasurementUnit;
            else
                _newProduct.ProductMeasurementUnit = null; // or any other default value

            string prompt = _supplierService.ValidateAndAddProduct(_newProduct, _existingProducts, _productCategories);

            await _jSRuntime.InvokeAsync<object>("alert", prompt);
        }

    }
}
