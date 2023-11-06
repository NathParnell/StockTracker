using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages
{
    public partial class Messages
    {
        //Inject Services
        [Inject] private IAuthorizationService _authorizationService { get; set; }
    }
}
