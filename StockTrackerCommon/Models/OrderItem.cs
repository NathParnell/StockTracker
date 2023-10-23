using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    public class OrderItem
    {
        public OrderItem() { }

        public string OrderItemId { get; set; }
        public string ProductId { get; set; }
        public string Quantity { get; set; }
        public decimal OrderPrice { get; set; }
    }
}
