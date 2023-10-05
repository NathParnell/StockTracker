using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerServer.Services
{
    public class RequestService : IRequestService
    {
        IAuthenticationService _authenticationService;

        public RequestService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Method takes in a JSON string and we deserialize it into our Response object. From this we can get the name of the Method
        /// in which we intend to call from the request, for example "ValidateLogin"
        /// We pass the methods an object array, and we interpret the data within the method
        /// From this method we get provided with another JSON string which we return
        /// </summary>
        /// <param name="jsonRequestString"></param>
        /// <returns></returns>
        public string ProcessRequest(string jsonRequestString, ref string clientIpAddress)
        {
            string response = String.Empty;

            Request request = JsonSerializer.Deserialize<Request>(jsonRequestString);

            if (request != null)
            {
                clientIpAddress = request.ClientIp;

                MethodInfo methodInfo = this.GetType().GetMethod(request.Method);
                if (methodInfo != null)
                {
                    response = methodInfo.Invoke(this, new object[] { new object[] { request } }).ToString();
                }
            }
            return response;
        }

        /// <summary>
        /// Method which takes an object array and turns it into a username and password
        /// With this information we call our AuthenticateUser method which returns the user who has just logged in (or null if invalid user creds)
        /// We then create and return a user authentication response 
        /// </summary>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public string ValidateLogin(object[] requestObject)
        {
            Request request = (Request)requestObject[0];
            List<string> parameters = JsonSerializer.Deserialize<List<string>>(request.Data.ToString());
            User user = _authenticationService.AuthenticateUser(parameters[0], parameters[1]);
            //need to make a response with this
            return ResponseSerializingHelper.CreateUserAuthenticationResponse(user);
        }
    }
}
