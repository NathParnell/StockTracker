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
        void SetCurrentUser(Supplier user = null);
        Supplier RequestLogin(string email, string password);
        Supplier GetSupplierBySupplierId(string supplierId);
        List<Supplier> GetAllSuppliers();
    }
}
