using StockTrackerCommon.Helpers;
using StockTrackerCommon.Models;
using StockTrackerCommon.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerCommon.Services
{
    public class RequestService : IRequestService
    {
        IAuthenticationService _authenticationService;

        public RequestService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public string ProcessRequest(string jsonRequestString)
        {
            Request request = JsonSerializer.Deserialize<Request>(jsonRequestString);
            string response;

            MethodInfo methodInfo = this.GetType().GetMethod(request.Method);
            if (methodInfo != null)
            {
                response = methodInfo.Invoke(this, new object[] { new object[] { request } }).ToString();
            }
            else
                response = string.Empty;

            return response;
        }

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
