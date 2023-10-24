using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.CustomerComponents
{
    /// <summary>
    /// Customer Homepage Component
    /// </summary>
    public partial class Home
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        [Inject]
        private ISessionHistoryService _sessionHistoryService { get; set; }

        [Inject]
        private IProductCategoryService _productCategoryService { get; set; }

        //declare variables
        private List<Supplier> _suppliers = new List<Supplier>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("Home");
            GetSuppliersInformation();
        }

        private async Task GetSuppliersInformation()
        {
            //Gets all of the suppliers
            _suppliers = _supplierService.GetAllSuppliers();
            _productCategories = _productCategoryService.GetAllProductCategories();
        }


        private void NavigateToViewSupplierPage(string supplierId)
        {
            _navManager.NavigateTo($"/ViewSupplier/{supplierId}");
        }

    }
}
