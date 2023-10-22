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

namespace StockTrackerApp.Pages
{
    public partial class AddProductCategory
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService { get; set; }

        [Inject]
        private IJSRuntime _jSRuntime { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        //define variables
        private List<ProductCategory> _existingProductCategories = new List<ProductCategory>();

        private ProductCategory _newProductCategory = new ProductCategory();


        protected override async Task OnInitializedAsync()
        {
            GetProductsCategoryInformation();
        }

        private async Task GetProductsCategoryInformation()
        {
            _existingProductCategories = _supplierService.GetAllProductCategories();
        }

        private async Task AddCategory()
        {
            string prompt = _supplierService.ValidateAndAddProductCategory(_newProductCategory, _existingProductCategories);

            await _jSRuntime.InvokeAsync<object>("alert", prompt);

            _navManager.NavigateTo("AddProductCategory", true);
        }

    }
}
