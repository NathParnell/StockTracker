using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    /// <summary>
    /// Customer which inherits a generic user class
    /// </summary>
    public class Customer : User
    {
        public Customer() { }

        public string CustomerId { get; set; }
        public string FirstNames { get; set; }
        public string Surname { get; set; }
        public List<string> ProductSubscriptions { get; set; }
        public List<string> SupplierSubscriptions { get; set; }

    }
}
