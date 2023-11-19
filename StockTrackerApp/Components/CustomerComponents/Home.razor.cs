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
        [Inject] private ISupplierService _supplierService { get; set; }
        [Inject] private NavigationManager _navManager { get; set; }
        [Inject] private ICustomerService _customerService { get; set; }
        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }
        [Inject] private IProductCategoryService _productCategoryService { get; set; }

        //declare variables
        private List<Supplier> _suppliers = new List<Supplier>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("Home");
            Init();
        }

        /// <summary>
        /// Method called on initialisation which gets all of the suppliers and product categories
        /// </summary>
        /// <returns></returns>
        private void Init()
        {
            //Gets all of the suppliers
            _suppliers = _supplierService.GetAllSuppliers();
            _productCategories = _productCategoryService.GetAllProductCategories();
        }

        private void NavigateToViewSupplierPage(string supplierId)
        {
            _navManager.NavigateTo($"/ViewSupplier/{supplierId}");
        }

        private void NavigateNewMessage(string customerId)
        {
            _navManager.NavigateTo($"NewMessage/{customerId}", true);
        }

        private void ManageSupplierSubscription(string supplierId)
        {
            Customer currentCustomer = _customerService.CurrentUser;
            if (currentCustomer.SupplierSubscriptions.Contains(supplierId))
            {
                currentCustomer.SupplierSubscriptions.Remove(supplierId);
            }
            else
            {
                currentCustomer.SupplierSubscriptions.Add(supplierId);
            }

            //if the customer gets updated then we need to refresh the page
            if (_customerService.UpdateCustomer(currentCustomer))
            {
                Init();
                StateHasChanged();
            }
        }

    }
}
