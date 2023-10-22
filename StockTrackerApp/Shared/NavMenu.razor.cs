using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Shared
{
    public partial class NavMenu
    {
        [Inject]
        private NavigationManager _navManager { get; set; }
        [Inject]
        private IUserService _userService { get; set; }

        private async Task NavigateHome()
        {
            _navManager.NavigateTo("Home", true);
        }
        private async Task NavigateFetchData()
        {
            _navManager.NavigateTo("FetchData", true);
        }
        private async Task NavigateCounter()
        {
            _navManager.NavigateTo("Counter", true);
        }
        private async Task Logout()
        {
            _userService.SetCurrentUser();
            _navManager.NavigateTo("", true);
        }
    }
}
