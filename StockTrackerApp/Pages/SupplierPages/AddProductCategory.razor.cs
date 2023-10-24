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

namespace StockTrackerApp.Pages.SupplierPages
{
    public partial class AddProductCategory
    {
        //Inject Services
        [Inject]
        private IProductCategoryService _productCategoryService { get; set; }

        [Inject]
        private IJSRuntime _jSRuntime { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        [Inject]
        private ISessionHistoryService _sessionHistoryService { get; set; }

        //define parameters
        [Parameter]
        public string CurrentProductId { get; set; }

        //define variables
        private List<ProductCategory> _existingProductCategories = new List<ProductCategory>();

        private ProductCategory _newProductCategory = new ProductCategory();


        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("AddProductCategory");
            GetProductsCategoryInformation();
        }

        private async Task GetProductsCategoryInformation()
        {
            _existingProductCategories = _productCategoryService.GetAllProductCategories();
        }

        private async Task AddCategory()
        {
            string prompt = _productCategoryService.ValidateAndAddProductCategory(_newProductCategory, _existingProductCategories);

            await _jSRuntime.InvokeAsync<object>("alert", prompt);

            string redirectUri = _sessionHistoryService.GetPreviousWebpage();
            _navManager.NavigateTo(redirectUri + $"/{CurrentProductId}", true);
        }

    }
}
