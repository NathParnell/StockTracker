using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IAuthorizationService
    {
        bool IsLoggedIn { get; set; }
        UserType? UserType { get; set; }
    }
}
