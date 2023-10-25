using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages.CustomerPages
{
    public partial class ViewSupplier
    {
        //Inject Services
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private IProductService _productService { get; set; }
        [Inject] private IProductCategoryService _productCategoryService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }

        //Define Parameters
        [Parameter] public string SupplierId { get; set; }

        //Define Variables
        private Supplier _supplier = new Supplier();
        private List<Product> _products = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("ViewSupplier");
            Init();
        }

        private async Task Init()
        {
            _supplier = _supplierService.GetSupplierBySupplierId(SupplierId);
            _products = _productService.GetProductsBySupplierId(SupplierId);
            _productCategories = _productCategoryService.GetAllProductCategories();
        }


    }
}
