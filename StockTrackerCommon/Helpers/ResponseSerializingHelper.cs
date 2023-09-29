using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockTrackerCommon.Helpers
{
    public static class ResponseSerializingHelper
    {
        //accepts the data which we want to pass through to the client
        public static string CreateResponse(string type, object[] data)
        {
            var response = new Response 
            { 
                ResponseId = Taikandi.SequentialGuid.NewGuid().ToString(),
                Type = type,
                Data = data
            };
            return JsonSerializer.Serialize(response);
        }

        public static string CreateUserAuthenticationResponse(User user)
        {
            object[] data = new object[] { user };
            return CreateResponse("User", data);
        }

    }
}
