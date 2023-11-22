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
            //Ensure that we don't already have a mock db
            if (!seeded && context.Customers.Count() == 0
                && context.Suppliers.Count() == 0
                && context.Products.Count() == 0 
                && context.ProductCategories.Count() == 0
                && context.Orders.Count() == 0
                && context.OrderItems.Count() == 0
                && context.Messages.Count() == 0)
            {
                lock (synchlock)
                {
                    if (!seeded)
                    {
                        var customers = GenerateCustomers();
                        var suppliers = GenerateSuppliers();
                        var productCategories = GenerateProductCategories();
                        var products = GenerateProducts();
                        var orders = GenerateOrders();
                        var orderItems = GenerateOrderItems();
                        var messages = GenerateMessages();

                        context.Customers.AddRange(customers);
                        context.Suppliers.AddRange(suppliers);
                        context.Products.AddRange(products);
                        context.ProductCategories.AddRange(productCategories);
                        context.Orders.AddRange(orders);
                        context.OrderItems.AddRange(orderItems);
                        context.Messages.AddRange(messages);

                        context.SaveChanges();
                        seeded = true;
                    }
                }
            }
        }

        #region "Generate Data Methods"
        
        /// <summary>
        /// This Method generates Customers for the mock database
        /// </summary>
        /// <returns></returns>
        private static Customer[] GenerateCustomers()
        {
            return new Customer[]
            {
                new Customer
                {
                    CustomerId = "3369956f-abed-42e1-9d99-08dbc77c7200",
                    FirstNames = "John",
                    Surname = "Smith",
                    Email = "customer1@test.com",
                    Password = "1234",
                    Address = "345 Fake Street",
                    City = "Leeds",
                    Postcode = "LS1 1AA",
                    CountryCode = "UK",
                    TelephoneNumber = "0113 123 4564",
                    ProductSubscriptions = new List<string>() { "56692403-07f3-46a4-c187-08dbd596aa89", "b2c13aaa-d8e6-4ed1-9dbd-08dbc77c7200" },
                    SupplierSubscriptions = new List<string> { "15c30fdd-e762-4e22-9d9c-08dbc77c7200" }
                },
                new Customer
                {
                    CustomerId = "838895f6-76b0-40fb-9d9a-08dbc77c7200",
                    FirstNames = "Jane",
                    Surname = "Doe",
                    Email = "customer2@test.com",
                    Password = "1234",
                    Address = "456 Fake Street",
                    City = "Leeds",
                    Postcode = "LS1 1AA",
                    CountryCode = "UK",
                    TelephoneNumber = "0113 123 4565",
                    ProductSubscriptions = new List<string>() { "56692403-07f3-46a4-c187-08dbd596aa89", "b2c13aaa-d8e6-4ed1-9dbd-08dbc77c7200" },
                    SupplierSubscriptions = new List<string> { "15c30fdd-e762-4e22-9d9c-08dbc77c7200" }
                },
                new Customer
                {
                    CustomerId = "46eb954d-2224-4290-4499-08dbc9a79af4",
                    FirstNames = "Nath",
                    Surname = "Parnell",
                    Email = "customer3@test.com",
                    Password = "1234",
                    Address = "567 Fake Street",
                    City = "Leeds",
                    Postcode = "LS1 1AA",
                    CountryCode = "UK",
                    TelephoneNumber = "0113 123 4566",
                    ProductSubscriptions = new List<string>() { "56692403-07f3-46a4-c187-08dbd596aa89", "b2c13aaa-d8e6-4ed1-9dbd-08dbc77c7200" },
                    SupplierSubscriptions = new List<string> { "15c30fdd-e762-4e22-9d9c-08dbc77c7200" }
                }
            };
        }

        /// <summary>
        /// This method generates Suppliers for the mock database
        /// </summary>
        /// <returns></returns>
        private static Supplier[] GenerateSuppliers()
        {
            return new Supplier[]
            {
                new Supplier
                {
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    CompanyName = "Pizza Shop Supplier 1",
                    Email = "supplier1@test.com",
                    Password = "1234",
                    Address = "123 Fake Street",
                    City = "Leeds",
                    Postcode = "LS1 1AA",
                    CountryCode = "UK",
                    TelephoneNumber = "0113 123 4567",
                    Description = "We are a catering supplier who sells pizza bases, cheese, toppings etc"
                },
                new Supplier
                {
                    SupplierId = "b13e2fe3-59c9-4566-060c-08dbd4c80d02",
                    CompanyName = "Pizza Shop Supplier 2",
                    Email = "supplier2@test.com",
                    Password = "1234",
                    Address = "234 Fake Street",
                    City = "Leeds",
                    Postcode = "LS1 1AA",
                    CountryCode = "UK",
                    TelephoneNumber = "0113 123 4568",
                    Description = "We are a catering supplier who sells pizza bases, cheese, toppings etc"
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
                    ProductId = "56692403-07f3-46a4-c187-08dbd596aa89",
                    ProductCode = "637489",
                    ProductName = "36 Stuffed Crust Pizza Bases",
                    ProductBrand = "Pizza Base Brand",
                    ProductSize = 3.6,
                    ProductMeasurementUnit = ProductMeasurementUnit.Kg,
                    ProductCategoryId = "e5704a65-c138-4788-9da4-08dbc77c7200",
                    ProductQuantity = 100,
                    Price = 25.50m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "2fd902fc-90f1-40ef-5435-08dbcc265888",
                    ProductCode = "918273",
                    ProductName = "24 Stuffed Crust Pizza Bases",
                    ProductBrand = "Pizza Base Brand",
                    ProductSize = 2.4,
                    ProductMeasurementUnit = ProductMeasurementUnit.Kg,
                    ProductCategoryId = "e5704a65-c138-4788-9da4-08dbc77c7200",
                    ProductQuantity = 200,
                    Price = 18.50m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"

                },
                new Product
                {
                    ProductId = "682ab1ad-0e81-465b-9dbf-08dbc77c7200",
                    ProductCode = "123456",
                    ProductName = "12 Stuffed Crust Pizza Bases",
                    ProductBrand = "Pizza Base Brand",
                    ProductSize = 1.2,
                    ProductMeasurementUnit = ProductMeasurementUnit.Kg,
                    ProductCategoryId = "e5704a65-c138-4788-9da4-08dbc77c7200",
                    ProductQuantity = 300,
                    Price = 10.50m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "b2c13aaa-d8e6-4ed1-9dbd-08dbc77c7200",
                    ProductCode = "234567",
                    ProductName = "Small Pepperoni",
                    ProductBrand = "Toppings Company",
                    ProductSize = 2,
                    ProductMeasurementUnit = ProductMeasurementUnit.Kg,
                    ProductCategoryId = "9af33c70-f565-4c14-9dad-08dbc77c7200",
                    ProductQuantity = 70,
                    Price = 6.00m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "ca0eb2ef-2d0c-4ec3-9dba-08dbc77c7200",
                    ProductCode = "345678",
                    ProductName = "Large Pepperoni",
                    ProductBrand = "Toppings Company",
                    ProductSize = 5,
                    ProductMeasurementUnit = ProductMeasurementUnit.Kg,
                    ProductCategoryId = "9af33c70-f565-4c14-9dad-08dbc77c7200",
                    ProductQuantity = 150,
                    Price = 9.00m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "82ba46be-2ddd-443c-9db8-08dbc77c7200",
                    ProductCode = "456789",
                    ProductName = "Garlic Oil",
                    ProductBrand = "Oils Brand",
                    ProductSize = 500,
                    ProductMeasurementUnit = ProductMeasurementUnit.ml,
                    ProductCategoryId = "46689ea1-a123-493d-9db0-08dbc77c7200",
                    ProductQuantity = 50,
                    Price = 3.00m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "850bc9f5-8138-4f5f-9d9f-08dbc77c7200",
                    ProductCode = "567890",
                    ProductName = "Tomato Pizza Sauce",
                    ProductBrand = "Sauce Brand",
                    ProductSize = 2,
                    ProductMeasurementUnit = ProductMeasurementUnit.L,
                    ProductCategoryId = "7f897028-cf37-452f-9da7-08dbc77c7200",
                    ProductQuantity = 350,
                    Price = 6.00m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "a85c0fa1-5a29-4b0e-9db3-08dbc77c7200",
                    ProductCode = "678901",
                    ProductName = "Large Mozzerella",
                    ProductBrand = "Cheese Company",
                    ProductSize = 2,
                    ProductMeasurementUnit = ProductMeasurementUnit.Kg,
                    ProductCategoryId = "cf261956-8b6d-470e-9daa-08dbc77c7200",
                    ProductQuantity = 250,
                    Price = 15.50m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"
                },
                new Product
                {
                    ProductId = "c4f8a7eb-fe76-41d5-9db5-08dbc77c7200",
                    ProductCode = "789012",
                    ProductName = "Small Mozzerella",
                    ProductBrand = "Cheese Company",
                    ProductSize = 500,
                    ProductMeasurementUnit = ProductMeasurementUnit.g,
                    ProductCategoryId = "cf261956-8b6d-470e-9daa-08dbc77c7200",
                    ProductQuantity = 340,
                    Price = 9.00m,
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200"

                }
            };
        }
        private static Order[] GenerateOrders()
        {
            return new Order[]
            {
                new Order
                {
                    OrderId = "92428116-ce24-48de-15c9-08dbe2d3e428",
                    CustomerId = "3369956f-abed-42e1-9d99-08dbc77c7200",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    OrderDate = DateTime.Now,
                    OrderStatus = OrderStatus.Pending,
                    OrderItemIds = new List<string>(){ "50ab31d9-355d-472a-15ca-08dbe2d3e428" },
                    TotalPrice = 12.00m,
                    OrderNotes = ""
                },
                new Order
                {
                    OrderId = "6ac9f6ae-549c-48f8-15d3-08dbe2d3e428",
                    CustomerId = "3369956f-abed-42e1-9d99-08dbc77c7200",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    OrderDate = DateTime.Now,
                    OrderStatus = OrderStatus.Accepted,
                    OrderItemIds = new List<string>(){ "520d8145-d50f-4840-15d6-08dbe2d3e428", "dc3f3126-210a-440c-15d9-08dbe2d3e428" },
                    TotalPrice = 42.00m,
                    OrderNotes = ""
                },
                new Order
                {
                    OrderId = "06cebbb8-ab36-48a9-15dc-08dbe2d3e428",
                    CustomerId = "838895f6-76b0-40fb-9d9a-08dbc77c7200",
                    SupplierId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    OrderDate = DateTime.Now,
                    OrderStatus = OrderStatus.Pending,
                    OrderItemIds = new List<string>(){ "4622954b-d54a-4657-15e2-08dbe2d3e428", "d374f8ef-1430-4cb3-15e6-08dbe2d3e428" },
                    TotalPrice = 57.00m,
                    OrderNotes = ""
                }
            };
        }

        private static OrderItem[] GenerateOrderItems()
        {
            return new OrderItem[]
            {
                new OrderItem
                {
                    OrderItemId = "50ab31d9-355d-472a-15ca-08dbe2d3e428",
                    ProductId = "850bc9f5-8138-4f5f-9d9f-08dbc77c7200",
                    Quantity = 2,
                    OrderPrice = 12.00m
                },
                new OrderItem
                {
                    OrderItemId = "520d8145-d50f-4840-15d6-08dbe2d3e428",
                    ProductId = "82ba46be-2ddd-443c-9db8-08dbc77c7200",
                    Quantity = 5,
                    OrderPrice = 15.00m
                },
                new OrderItem
                {
                    OrderItemId = "dc3f3126-210a-440c-15d9-08dbe2d3e428",
                    ProductId = "c4f8a7eb-fe76-41d5-9db5-08dbc77c7200",
                    Quantity = 3,
                    OrderPrice = 27.00m
                },
                new OrderItem
                {
                    OrderItemId = "4622954b-d54a-4657-15e2-08dbe2d3e428",
                    ProductId = "ca0eb2ef-2d0c-4ec3-9dba-08dbc77c7200",
                    Quantity = 4,
                    OrderPrice = 36.00m
                },
                new OrderItem
                {
                    OrderItemId = "d374f8ef-1430-4cb3-15e6-08dbe2d3e428",
                    ProductId = "682ab1ad-0e81-465b-9dbf-08dbc77c7200",
                    Quantity = 2,
                    OrderPrice = 21.00m
                }
            };
        }

        private static Message[] GenerateMessages()
        {
            return new Message[]
            {
                new Message
                {
                    MessageId = "36c559dd-df8c-45b5-73e0-08dbdf012f23",
                    SentTime = DateTime.Now,
                    SenderId = "3369956f-abed-42e1-9d99-08dbc77c7200",
                    ReceiverId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    Subject = "Message",
                    MessageBody = "Message from Customer 1 to Supplier 1"
                },
                new Message
                {
                    MessageId = "fc0cfc4b-9fc6-406e-73e2-08dbdf012f23",
                    SentTime = DateTime.Now,
                    SenderId = "15c30fdd-e762-4e22-9d9c-08dbc77c7200",
                    ReceiverId = "3369956f-abed-42e1-9d99-08dbc77c7200",
                    Subject = "Message",
                    MessageBody = "Message from Supplier 1 to Customer 1"
                }
            };
        }

        #endregion
    }
}
