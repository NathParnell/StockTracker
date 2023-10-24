using StockTrackerCommon.Models;
using StockTrackerServer.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerServer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        //Set up Logger
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AuthenticationService));

        //define services
        IDataService _dataService;

        public AuthenticationService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public Supplier? AuthenticateSupplier(string email, string password)
        {
            Supplier targetSupplier = _dataService.GetSupplierByEmail(email.ToLower()).Result;

            //if a supplier with that email does not exist
            if (targetSupplier == null)
            {
                logger.Info($"AuthenticateSupplier(), Supplier Authentication failed");
                return null;
            }

            //if the supplier is actually authenticated
            if (targetSupplier.Email == email && targetSupplier.Password == password)
            {
                logger.Info($"AuthenticateSupplier(), Supplier {targetSupplier.Email} Authenticated");
                return targetSupplier;
            }

            //if the supplier gets their password wrong
            logger.Info($"AuthenticateSupplier(), Supplier Authentication failed");
            return null;
        }

        public Customer? AuthenticateCustomer(string email, string password)
        {
            Customer targetCustomer = _dataService.GetCustomerByEmail(email.ToLower()).Result;

            //if a customer with that email does not exist
            if (targetCustomer == null)
            {
                logger.Info($"AuthenticateCustomer(), Customer Authentication failed");
                return null;
            }

            //if the customer is actually authenticated
            if (targetCustomer.Email == email && targetCustomer.Password == password)
            {
                logger.Info($"AuthenticateCustomer(), Customer {targetCustomer.Email} Authenticated");
                return targetCustomer;
            }

            //if the customer gets their password wrong
            logger.Info($"AuthenticateCustomer(), Customer Authentication failed");
            return null;
        }
    }
}
