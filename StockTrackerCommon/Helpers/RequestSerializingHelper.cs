using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerCommon.Helpers
{
    public static class RequestSerializingHelper
    {
        /// <summary>
        /// Accepts parameters such as a Method Name and the data which we want to pass to the server
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateRequest(string methodName, object[] data)
        {
            var request = new Request
            {
                RequestId = Taikandi.SequentialGuid.NewGuid().ToString(),
                Method = methodName,
                Data = data
            };
            return JsonSerializer.Serialize(request);
        }

        /// <summary>
        /// Creates a JSON string of a Validate Login Request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateLoginRequest(string username, string password)
        {
            string methodName = "ValidateLogin";
            object[] data = new object[] { username, password };
            return CreateRequest(methodName, data);
        }
    }
}
