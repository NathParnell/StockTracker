using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    /// <summary>
    /// Generic user base class which is inherited by the customer, supplier and administrator classes
    /// </summary>
    public class User
    {
        public User() { }

        public string Email { get; set; }
        public string Password { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string CountryCode { get; set; }
    }
}
