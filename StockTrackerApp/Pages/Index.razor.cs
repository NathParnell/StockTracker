using Microsoft.AspNetCore.Components;
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
    public partial class Index
    {
        //Inject Services
        [Inject] private ISupplierService _supplierService { get; set; }

        [Inject] private ICustomerService _customerService { get; set; }

        [Inject] private IAuthorizationService _authorizationService { get; set; }

        [Inject] private NavigationManager _navManager { get; set; }

        [Inject] private ISessionHistoryService _sessionHistoryService { get; set; }


        //declare variables
        private string _email;
        private string _password;
        private string _userType;
        private string _loginPrompt = String.Empty;

        protected override async Task OnInitializedAsync()
        {
            _sessionHistoryService.AddWebpageToHistory("Index");
            NavigateHomeIfLoggedIn();
            Init();
        }

        /// <summary>
        /// Add method to reset username and password fields
        /// </summary>
        /// <returns></returns>
        private async Task Init()
        {
            _userType = String.Empty;
            _email = String.Empty;
            _password = String.Empty;
        }

        /// <summary>
        /// Method checks whether a user is currently logged in and if so, redirects them to the home page
        /// This is called upon loading the page and after logging in
        /// </summary>
        private void NavigateHomeIfLoggedIn()
        {
            if (_authorizationService.IsLoggedIn)
            {
                _navManager.NavigateTo("Home");
            }
        }

        /// <summary>
        /// Method which gets called upon user pressing the Login button
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            //ensure that values have been entered of for both fields
            if (String.IsNullOrWhiteSpace(_userType) || String.IsNullOrWhiteSpace(_email) || String.IsNullOrWhiteSpace(_password))
            {
                _loginPrompt = "Please select your user role and enter a email and password!";
                return;
            }

            //check if user is logging as a supplier or a customer
            Enum.TryParse(_userType, out UserType userType);
            if (userType == UserType.Supplier)
            {
                //attempt login
                Supplier user = _supplierService.RequestLogin(_email, _password);

                //check that the user logged in, if so redirect, if not then prompt the user to retry
                if (user == null)
                {
                    _loginPrompt = "The email or password is incorrect";
                    Init();
                }
                else
                    NavigateHomeIfLoggedIn();
            }
            else if (userType == UserType.Customer)
            {
                //attempt login
                Customer user = _customerService.RequestLogin(_email, _password);

                //check that the user logged in, if so redirect, if not then prompt the user to retry
                if (user == null)
                {
                    _loginPrompt = "The email or password is incorrect";
                    Init();
                }
                else
                    NavigateHomeIfLoggedIn();
            }
        }
    }
}
