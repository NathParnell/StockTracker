using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    /// <summary>
    /// Supplier which inherits a generic user class
    /// </summary>
    public class Supplier : User
    {
        public Supplier() { }

        public string SupplierId { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }

    }
}
