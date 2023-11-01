using StockTrackerApp.Services.Infrastructure;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    /// <summary>
    /// Class which contains variables to manage authorization
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        public bool IsLoggedIn { get; set; } = false;
        public UserType? UserType { get; set; } = null;
    }
}
