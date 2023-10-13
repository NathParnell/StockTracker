using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Components.SupplierComponents
{
    public partial class Home
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService {  get; set; }
        [Inject]
        private IUserService _userService { get; set; }


        //Define Variables
        private List<Stock> _stock = new List<Stock>();
        private List<Product> _products = new List<Product>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        protected override async Task OnInitializedAsync()
        {
            Init();
        }

        private async Task Init()
        {
            //Get all of the stock which belongs to the current user (the supplier)
            _stock = _supplierService.GetStockBySupplier(_userService.CurrentUser.UserId);

            //Get a list of Product Ids
            List<string> productIds = _stock.Select(prod => prod.ProductId.ToString()).ToList();
            _products = _supplierService.GetProductsByProductIds(productIds);

            //Get a list of the product category ids
            List<string> productCategoryIds = _products.Select(cat => cat.ProductCategoryId.ToString()).Distinct().ToList();
            _productCategories = _supplierService.GetProductCategoriesByProductCategoryIds(productCategoryIds);
        }

    }
}
