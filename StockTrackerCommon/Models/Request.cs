using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    public class Request
    {
        public string RequestId { get; set; }
        public string Method { get; set; }
        public object Data { get; set; }
    }
}
