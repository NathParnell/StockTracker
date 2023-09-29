using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerCommon.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerCommon.Services
{
    public class UserService : IUserService
    {
        private readonly IClientTransportService _clientTransportService;

        public UserService(IClientTransportService clientTransportService)
        {
            _clientTransportService = clientTransportService;
        }

        public User CurrentUser { get; private set; }

        /// <summary>
        /// Method which sets the Current User variable to manage the user on the client side
        /// </summary>
        /// <param name="user"></param>
        public void SetCurrentUser(User user = null)
        {
            CurrentUser = user;
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

            //Response response = JsonSerializer.Deserialize<Response>(jsonResponse);
            //User user = JsonSerializer.Deserialize<List<User>>(response.Data.ToString())[0];

            User user = (User)ResponseDeserializingHelper.DeserializeResponse(jsonResponse).First();

            //set the current user to be the user who has just logged into the system
            SetCurrentUser(user);
            return user;
        }
    }
}
