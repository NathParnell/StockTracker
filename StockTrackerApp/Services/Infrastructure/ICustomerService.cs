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
        Customer RequestLogin(string email, string password);
        bool Logout();
        Customer GetCustomerByCustomerId(string customerId);
        List<Customer> GetAllCustomers();
        List<Customer> GetCustomersByCustomerIds(List<string> customerIds);
    }
}
