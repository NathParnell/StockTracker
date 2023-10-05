using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
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
                ClientIp = GetPrivateIpAddress(),
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


        #region "Get IP Address"
        /// <summary>
        /// We need to get the client's PUBLIC ip address if we want the server and clients to be able 
        /// to communicate on different networks
        /// </summary>
        /// <returns></returns>
        private static string GetPublicIpAddress()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //use this third party service to get the client's public ip address
                string ipApiUrl = "https://api.ipify.org?format=text";
                HttpResponseMessage response = httpClient.GetAsync(ipApiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    string ip = response.Content.ReadAsStringAsync().Result;
                    return ip.Trim();
                }
                else
                {
                    throw new Exception("Failed to retrieve public IP address.");
                }
            }
        }

        private static string GetPrivateIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        #endregion
    }
}
