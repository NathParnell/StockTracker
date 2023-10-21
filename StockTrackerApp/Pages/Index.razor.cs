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
        [Inject]
        private IUserService _userService { get; set; }

        [Inject]
        private NavigationService _navService { get; set; }

        //declare variables
        private string _username;
        private string _password;
        private string _loginPrompt = String.Empty;

        protected override async Task OnInitializedAsync()
        {
            NavigateHomeIfLoggedIn();
            Init();
        }

        /// <summary>
        /// Add method to reset username and password fields
        /// </summary>
        /// <returns></returns>
        private async Task Init()
        {
            _username = String.Empty;
            _password = String.Empty;
        }

        /// <summary>
        /// Method checks whether a user is currently logged in and if so, redirects them to the home page
        /// This is called upon loading the page and after logging in
        /// </summary>
        private void NavigateHomeIfLoggedIn()
        {
            if (_userService.IsLoggedIn)
            {
                _navService.NavigateTo("Home");
            }
        }

        /// <summary>
        /// Method which gets called upon user pressing the Login button
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            //ensure that values have been entered ofor both fields
            if (String.IsNullOrEmpty(_username) || String.IsNullOrEmpty(_password))
            {
                _loginPrompt = "Please enter a username and password!";
                return;
            }

            //attempt login
            User user = _userService.RequestLogin(_username, _password);

            //check that the user logged in, if so redirect, if not then prompt the user to retry
            if (user == null)
            {
                _loginPrompt = "The username or password is incorrect";
                Init();
            }
            else
                NavigateHomeIfLoggedIn();
        }
    }
}
