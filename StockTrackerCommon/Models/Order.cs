using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    public class Order
    {
        public Order() { } 

        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string SupplierId { get; set; }
        List<string> OrderItemIds { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderNotes { get; set; }
    }
}
