using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StockTrackerCommon.Helpers
{
    public static class ResponseDeserializingHelper
    {
        /// <summary>
        /// Method which takes in a JSON string and we also give it the type of object we want the object to be as TModel
        /// The method returns an array of the type
        /// </summary>
        /// <param name="jsonResponseString"></param>
        /// <returns></returns>
        public static TModel[] DeserializeResponse<TModel>(string jsonResponseString)
        {
            Response response = JsonSerializer.Deserialize<Response>(jsonResponseString);

            //ensure that we are provided woth a response by the server
            if (response.Data == null)
                return null;

            TModel[] deserializedObject = JsonSerializer.Deserialize<TModel[]>(response.Data.ToString());
            return deserializedObject;
        }
    }
}
