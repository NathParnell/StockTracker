using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        IDataService _dataService;
        public AuthenticationService(IDataService dataService) 
        {
            _dataService = dataService;
        }


        public User? AuthenticateUser(string username, string password)
        {
            User targetUser = _dataService.GetUserByUsername(username).Result;
            
            //if a user with that username does not exist
            if (targetUser == null)
                return null;

            //if the user is actually authenticated
            if (targetUser.Username == username && targetUser.Password == password)
                return targetUser;

            //if the username gets their password wrong
            return null;
        }
    }
}
