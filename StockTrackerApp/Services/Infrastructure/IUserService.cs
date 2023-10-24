using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface IUserService
    {
        User CurrentUser { get; }
        bool IsLoggedIn { get; }    
        void SetCurrentUser(User user = null);
        User RequestLogin(string username, string password);
        List<User> GetUsersByUserType(UserType userType);

    }
}
