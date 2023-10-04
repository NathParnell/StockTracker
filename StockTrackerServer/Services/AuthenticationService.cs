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
        //Set up Logger
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AuthenticationService));

        //define services
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
            {
                logger.Info($"AuthenticateUser(), User Authentication failed");
                return null;
            }

            //if the user is actually authenticated
            if (targetUser.Username == username && targetUser.Password == password)
            {
                logger.Info($"AuthenticateUser(), User {targetUser.Username} Authenticated");
                return targetUser;
            }

            //if the username gets their password wrong
            logger.Info($"AuthenticateUser(), User Authentication failed");
            return null;
        }
    }
}
