using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    public class Stock
    {
        public Stock() { }

        public string StockId { get; set; }
        public string SupplierId { get; set; }
        public string ProductId { get; set; }
        public int StockQuantity { get; set; }
        public decimal StockPrice { get; set; }
    }
}
