using Microsoft.AspNetCore.Components;
using StockTrackerApp.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages
{
    public partial class Home
    {
        //Inject Services
        [Inject]
        private ISupplierService _supplierService { get; set; }

        [Inject]
        private ICustomerService _customerService { get; set; }
    }
}
