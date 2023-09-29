using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    public class Response
    {
        [JsonPropertyName("responseId")]
        public string ResponseId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}
