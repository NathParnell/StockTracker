using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface ICustomerService
    {
        Customer CurrentUser { get; }
        void SetCurrentUser(Customer user = null);
        Customer RequestLogin(string email, string password);
        Customer GetCustomerByCustomerId(string customerId);
        List<Customer> GetAllCustomers();
    }
}
