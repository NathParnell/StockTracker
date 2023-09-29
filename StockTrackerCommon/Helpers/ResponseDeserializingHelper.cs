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
        /// Method which takes in a JSON string and determines what the type is.
        /// The the method deserializes the data based on the type and returns the data as an object array
        /// </summary>
        /// <param name="jsonResponseString"></param>
        /// <returns></returns>
        public static object[] DeserializeResponse(string jsonResponseString)
        {
            Response response = JsonSerializer.Deserialize<Response>(jsonResponseString);

            object[] deserializationObject;

            if (response.Data == null)
                return null;

            //We check what type the object is and then we set the deserialization generic object accordingly
            switch (response.Type)
            {
                case "User":
                    deserializationObject = JsonSerializer.Deserialize<User[]>(response.Data.ToString());
                    break;
                default:
                    deserializationObject = null;
                    break;
            }

            return deserializationObject;

            //List<User> user = JsonSerializer.Deserialize<List<User>>(response.Data.ToString());

            //User user = JsonSerializer.Deserialize(jsonResponseString, User);

            //var deserializedObject = CreateObjectByType(response.Type, jsonResponseString);

            //return deserializedObject;
        }

        private static object CreateObjectByType(string objectType, object jsonData)
        {
            Type type = Type.GetType($"StockTrackerCommon.Models.{objectType}");

            if (type != null)
            {
                //// Convert the jsonData to a JSON string first
                //string jsonString = JsonSerializer.Serialize(jsonData);

                var listType = typeof(List<>).MakeGenericType(type);
                var obj = JsonSerializer.Deserialize(jsonData.ToString(), listType);


                // Deserialize the JSON string into the specified object type
                //var obj = JsonSerializer.Deserialize(jsonString, type);

                return null;
            }
            else
            {
                throw new ArgumentException($"Type '{objectType}' not found.");
            }
        }
    }
}
