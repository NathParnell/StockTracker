using StockTrackerCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTrackerCommon.Database
{
    public static class StockTrackerDbContextSeedData
    {
        static object synchlock = new object();
        static volatile bool seeded = false;

        /// <summary>
        /// We need to prefill all of the data within the in memory database
        /// This method takes generated data and adds it to the in memory database
        /// </summary>
        public static void EnsureSeedData(this StockTrackerDbContext context)
        {
            //Ensure that we dont already have a mock db
            if (!seeded && context.Users.Count() == 0 && context.Products.Count() == 0 && context.ProductCategories.Count() == 0 && context.Stock.Count() == 0)
            {
                lock (synchlock)
                {
                    if (!seeded)
                    {
                        var users = GenerateUsers();
                        var productCategories = GenerateProductCategories();
                        var products = GenerateProducts();
                        var stock = GenerateStock();

                        context.Users.AddRange(users);
                        context.Products.AddRange(products);
                        context.ProductCategories.AddRange(productCategories);
                        context.Stock.AddRange(stock);

                        context.SaveChanges();
                        seeded = true;
                    }
                }
            }
        }

        #region "Generate Data Methods"
        /// <summary>
        /// This method generates Users for the mock database
        /// </summary>
        /// <returns></returns>
        private static User[] GenerateUsers()
        {
            return new User[]
            {
                new User
                {
                    UserId = "3369956f-abed-42e1-9d99-08dbc77c7200",
                    Username = "PizzaShop1",
                    Password = "1234",
                    UserType = UserType.Customer,
                    CountryCode = "UK"
                },
                new User
                {
                    UserId = "838895f6-76b0-40fb-9d9a-08dbc77c7200",
                    Username = "PizzaShop2",
                    Password = "1234",
                    UserType = UserType.Customer,
                    CountryCode = "UK"
                },
                new User
                {
                    UserId = "46eb954d-2224-4290-4499-08dbc9a79af4",
                    Username = "PizzaShop3",
                    Password = "1234",
                    UserType = UserType.Customer,
                    CountryCode = "US"
                },
                new User
                {
                    UserId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    Username = "PizzaShopSupplier",
                    Password = "1234",
                    UserType = UserType.Supplier,
                    CountryCode = "UK"
                }
            };
        }

        /// <summary>
        /// This method generates Product Categories for the mock database
        /// </summary>
        /// <returns></returns>
        private static ProductCategory[] GenerateProductCategories()
        {
            return new ProductCategory[]
            {
                new ProductCategory
                {
                    ProductCategoryId = "e5704a65-c138-4788-9da4-08dbc77c7200",
                    ProductCategoryName = "Pizza Bases"
                },
                new ProductCategory
                {
                    ProductCategoryId = "7f897028-cf37-452f-9da7-08dbc77c7200",
                    ProductCategoryName = "Pizza Sauce"
                },
                new ProductCategory
                {
                    ProductCategoryId = "cf261956-8b6d-470e-9daa-08dbc77c7200",
                    ProductCategoryName = "Cheese"
                },
                new ProductCategory
                {
                    ProductCategoryId = "9af33c70-f565-4c14-9dad-08dbc77c7200",
                    ProductCategoryName = "Toppings"
                },
                new ProductCategory
                {
                    ProductCategoryId = "46689ea1-a123-493d-9db0-08dbc77c7200",
                    ProductCategoryName = "Oil"
                }
            };
        }

        /// <summary>
        /// This method generates Products for the mock database
        /// </summary>
        /// <returns></returns>
        private static Product[] GenerateProducts()
        {
            return new Product[]
            {
                new Product
                {
                    ProductId = "2fd902fc-90f1-40ef-5435-08dbcc265888",
                    ProductCode = "918273",
                    ProductName = "24 Stuffed Crust Pizza Bases",
                    ProductBrand = "Warburtons",
                    ProductSize = 2.4,
                    ProductMeasurementUnit = MeasurementUnit.Kg,
                    ProductCategoryId = "e5704a65-c138-4788-9da4-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "682ab1ad-0e81-465b-9dbf-08dbc77c7200",
                    ProductCode = "123456",
                    ProductName = "12 Stuffed Crust Pizza Bases",
                    ProductBrand = "Warburtons",
                    ProductSize = 1.2,
                    ProductMeasurementUnit = MeasurementUnit.Kg,
                    ProductCategoryId = "e5704a65-c138-4788-9da4-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "b2c13aaa-d8e6-4ed1-9dbd-08dbc77c7200",
                    ProductCode = "234567",
                    ProductName = "Small Pepperoni",
                    ProductBrand = "Yorkshire Meat Company",
                    ProductSize = 2,
                    ProductMeasurementUnit = MeasurementUnit.Kg,
                    ProductCategoryId = "9af33c70-f565-4c14-9dad-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "ca0eb2ef-2d0c-4ec3-9dba-08dbc77c7200",
                    ProductCode = "345678",
                    ProductName = "Large Pepperoni",
                    ProductBrand = "Yorkshire Meat Company",
                    ProductSize = 5,
                    ProductMeasurementUnit = MeasurementUnit.Kg,
                    ProductCategoryId = "9af33c70-f565-4c14-9dad-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "82ba46be-2ddd-443c-9db8-08dbc77c7200",
                    ProductCode = "456789",
                    ProductName = "Garlic Oil",
                    ProductBrand = "Flora",
                    ProductSize = 500,
                    ProductMeasurementUnit = MeasurementUnit.ml,
                    ProductCategoryId = "46689ea1-a123-493d-9db0-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "850bc9f5-8138-4f5f-9d9f-08dbc77c7200",
                    ProductCode = "567890",
                    ProductName = "Tomato Pizza Sauce",
                    ProductBrand = "Dolmio",
                    ProductSize = 2,
                    ProductMeasurementUnit = MeasurementUnit.l,
                    ProductCategoryId = "7f897028-cf37-452f-9da7-08dbc77c7200",
                },
                new Product
                {
                    ProductId = "a85c0fa1-5a29-4b0e-9db3-08dbc77c7200",
                    ProductCode = "678901",
                    ProductName = "Large Mozzerella",
                    ProductBrand = "Cathedral",
                    ProductSize = 2,
                    ProductMeasurementUnit = MeasurementUnit.Kg,
                    ProductCategoryId = "cf261956-8b6d-470e-9daa-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "c4f8a7eb-fe76-41d5-9db5-08dbc77c7200",
                    ProductCode = "789012",
                    ProductName = "Small Mozzerella",
                    ProductBrand = "Cathedral",
                    ProductSize = 500,
                    ProductMeasurementUnit = MeasurementUnit.g,
                    ProductCategoryId = "cf261956-8b6d-470e-9daa-08dbc77c7200"
                }
            };
        }

        /// <summary>
        /// This method generates Stock for the mock database
        /// </summary>
        /// <returns></returns>
        private static Stock[] GenerateStock()
        {
            return new Stock[]
            {
                new Stock
                {
                    StockId = "23def5d7-9aa6-4820-5439-08dbcc265888",
                    ProductId = "2fd902fc-90f1-40ef-5435-08dbcc265888",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    StockQuantity = 200,
                    StockPrice = 7.50m
                },
                new Stock
                {
                    StockId = "23271213-d265-477c-d148-08dbc80689ae",
                    ProductId = "a85c0fa1-5a29-4b0e-9db3-08dbc77c7200",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    StockQuantity = 1000,
                    StockPrice = 12.50m
                },
                new Stock
                {
                    StockId = "c14d0153-4801-4ab9-d145-08dbc80689ae",
                    ProductId = "b2c13aaa-d8e6-4ed1-9dbd-08dbc77c7200",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    StockQuantity = 200,
                    StockPrice = 15.50m
                },
                new Stock
                {
                    StockId = "602012e6-a458-4923-d141-08dbc80689ae",
                    ProductId = "850bc9f5-8138-4f5f-9d9f-08dbc77c7200",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    StockQuantity = 500,
                    StockPrice = 10.50m
                },
                new Stock
                {
                    StockId = "fd0ce5b9-8a96-4713-d143-08dbc80689ae",
                    ProductId = "682ab1ad-0e81-465b-9dbf-08dbc77c7200",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    StockQuantity = 800,
                    StockPrice = 5.00m
                }
            };
        }

        #endregion
    }
}
