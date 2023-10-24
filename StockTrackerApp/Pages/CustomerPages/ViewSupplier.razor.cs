using Microsoft.AspNetCore.Components;
using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerApp.Pages.CustomerPages
{
    public partial class ViewSupplier
    {
        //Inject Services


        //Define Parameters
        [Parameter]
        public string SupplierId { get; set; }

        //define variables
        private User Supplier = new User();


    }
}
