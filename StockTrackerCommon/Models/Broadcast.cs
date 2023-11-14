using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    public class Broadcast
    {
        public Broadcast() { }

        [JsonPropertyName("senderId")]
        public string SenderId { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("messageBody")]
        public string MessageBody { get; set; }
    }
}
