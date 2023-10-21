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
        private NavigationService _navService { get; set; }
        [Inject]
        private IUserService _userService { get; set; }

        private async Task NavigateHome()
        {
            _navService.NavigateTo("Home", true);
        }
        private async Task NavigateFetchData()
        {
            _navService.NavigateTo("FetchData", true);
        }
        private async Task NavigateCounter()
        {
            _navService.NavigateTo("Counter", true);
        }
        private async Task Logout()
        {
            _userService.SetCurrentUser();
            _navService.NavigateTo("", true);
        }
    }
}
