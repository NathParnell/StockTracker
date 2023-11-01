using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Services.Infrastructure
{
    public interface ISupplierService
    {
        Supplier CurrentUser { get; }
        Supplier RequestLogin(string email, string password);
        bool Logout();
        Supplier GetSupplierBySupplierId(string supplierId);
        List<Supplier> GetAllSuppliers();
    }
}
