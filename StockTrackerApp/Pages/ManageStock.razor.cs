using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages
{
    public partial class ManageStock
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        //define variables
        private List<ProductCategory> _productCategories = new List<ProductCategory>();
        private List<Product> _products = new List<Product>();

        private string _selectedProductCategoryId;



        protected override async Task OnInitializedAsync()
        {
            GetProductsInformation();
        }

        private async Task GetProductsInformation()
        {
            _productCategories = _supplierService.GetAllProductCategories();
            _products = _supplierService.GetAllProducts();
        }

        private async Task AddProduct()
        {
            _navManager.NavigateTo("AddProduct");
        }

    }
}
