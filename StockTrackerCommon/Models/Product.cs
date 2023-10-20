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
        public double ProductSize { get; set; }
        public ProductMeasurementUnit? ProductMeasurementUnit { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Price { get; set; }
        public string ProductCategoryId { get; set; }
        public string SupplierId { get; set; }
    }

    public enum ProductMeasurementUnit
    {
        Kg = 0,
        g = 1,
        l = 2,
        ml = 3,
        units = 4
    }
}
