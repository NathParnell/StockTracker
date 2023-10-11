using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerApp.Services
{
    public class UserService : IUserService
    {
        //setup logger
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(UserService));

        //define services
        private readonly IClientTransportService _clientTransportService;

        public UserService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }


        //Define public variables 
        public User CurrentUser { get; private set; }
        public bool IsLoggedIn { get; private set; } = false;


        /// <summary>
        /// Method which sets the Current User variable to manage the user on the client side
        /// Method also sets the IsLoggedIn variable, which assists in managing the state of the app
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(User user = null)
        {
            CurrentUser = user;
            if (CurrentUser == null)
                IsLoggedIn = false;
            else
                IsLoggedIn = true;
        }


        /// <summary>
        /// This method takes in a username and password, and it sends a request to the server to attempt
        /// a login. We are then provided with a response which should be an object of the user who has logged in.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User RequestLogin(string username, string password)
        {
            //We create a JSON string of our Login Request and pass it to the TCP handler which handles our request
            //We are then returned a JSON string of our response from the server
            string jsonResponse = _clientTransportService.TcpHandler(RequestSerializingHelper.CreateLoginRequest(username, password));

            //if the method we tried to call did not exist
            if (String.IsNullOrEmpty(jsonResponse))
                return null;

            //deserialize the JSON as a list of users and get the first (and only) User
            User user = ResponseDeserializingHelper.DeserializeResponse<User>(jsonResponse).First();

            //set the current user to be the user who has just logged into the system
            SetCurrentUser(user);

            if (this.CurrentUser != null)
                Logger.Info($"RequestLogin(), User: {this.CurrentUser.Username} has logged into the system");
            else
                Logger.Info($"RequestLogin(), User login attempt failed");

            return user;
        }
    }
}
