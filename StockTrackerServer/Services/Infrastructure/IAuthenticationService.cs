using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services.Infrastructure
{
    public interface IAuthenticationService
    {
        Supplier? AuthenticateSupplier(string email, string password);
        Customer? AuthenticateCustomer(string email, string password);
    }
}
