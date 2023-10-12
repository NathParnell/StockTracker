using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Models
{
    public class Product
    {
        public Product() { }

        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public double ProductSize { get ; set; }
        public MeasurementUnit ProductMeasurementUnit { get; set; }
        public string ProductCategoryId { get; set; }

    }

    public enum MeasurementUnit
    {
        Kg = 0,
        g = 1,
        l = 2,
        ml = 3,
        units = 4
    }
}
