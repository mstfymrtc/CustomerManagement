using CustomerManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Models;

namespace CustomerManagement.Data
{
    public class DbSeeder
    {

        readonly ILogger _Logger;
        readonly CustomerDbContext _Context;

        public DbSeeder(ILoggerFactory loggerFactory, CustomerDbContext Context)
        {
            _Logger = loggerFactory.CreateLogger("DbInitiliazerLogger");
            _Context = Context;
        }



        public async Task SeedAsync()
        {
            //if (await _Context.Database.EnsureCreatedAsync())
            //{
            if (!await _Context.Customers.AnyAsync())
            {
                await InsertCustomersSampleData(_Context);
            }
            //}
        }


        public async Task InsertCustomersSampleData(CustomerDbContext db)
        {
            var states = GetStates();
            db.States.AddRange(states);
            try
            {
                int numAffected = await db.SaveChangesAsync();
                _Logger.LogInformation(@"Saved {numAffected} states");
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(CustomerDbContext)}: " + exp.Message);
                throw;
            }

            var customers = GetCustomers(states);
            db.Customers.AddRange(customers);

            try
            {
                int numAffected = await db.SaveChangesAsync();
                _Logger.LogInformation($"Saved {numAffected} customers");
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(CustomerDbContext)}: " + exp.Message);
                throw;
            }

        }

        private List<Customer> GetCustomers(List<State> states)
        {
            //Customers
            var customerNames = new string[]
            {
                "Serap,LowTower,Female,acmecorp.com",
                "Jesse,Smith,Female,gmail.com",
                "Albert,Einstein,Male,outlook.com",
                "Fevzi,Ozgul,Male,yahoo.com",
                "Ward,Thistle,Male,gmail.com",
                "Brad,Red,Male,gmail.com",
                "Lokum,Minare,Male,gmail.com",
                "Tuba,Hay,Male,gmail.com",
                "Michelle,Avery,Female,acmecorp.com",
                "Heedy,Dahlin,Female,hotmail.com",

            };
            var addresses = new string[]
            {
                "1234 Anywhere St.",
                "435 Main St.",
                "1 Atomic St.",
                "85 Cedar Dr.",
                "12 Ocean View St.",
                "1600 Amphitheatre Parkway",
                "1604 Amphitheatre Parkway",
                "1607 Amphitheatre Parkway",
                "346 Cedar Ave.",
                "4576 Main St."

            };

            var citiesStates = new string[]
            {
                "Phoenix,AZ,Arizona",
                "Encinitas,CA,California",
                "Seattle,WA,Washington",
                "Chandler,AZ,Arizona",
                "Dallas,TX,Texas",
                "Orlando,FL,Florida",
                "Carey,NC,North Carolina",
                "Anaheim,CA,California",
                "Dallas,TX,Texas",
                "New York,NY,New York"

            };

            var citiesIds = new int[] { 5, 9, 44, 5, 36, 17, 16, 9, 36, 14 };
            var zip = 85229;

            var orders = new List<Order>
            {
                new Order { Product = "Basket", Price = 29.99M, Quantity = 1 },
                new Order { Product = "Yarn", Price = 9.99M, Quantity = 1 },
                new Order { Product = "Needes", Price = 5.99M, Quantity = 1 },
                new Order { Product = "Speakers", Price = 499.99M, Quantity = 1 },
                new Order { Product = "iPod", Price = 399.99M, Quantity = 1 },
                new Order { Product = "Table", Price = 329.99M, Quantity = 1 },
                new Order { Product = "Chair", Price = 129.99M, Quantity = 4 },
                new Order { Product = "Lamp", Price = 89.99M, Quantity = 5 },
                new Order { Product = "Call of Duty", Price = 59.99M, Quantity = 1 },
                new Order { Product = "Controller", Price = 49.99M, Quantity = 1 },
                new Order { Product = "Gears of War", Price = 49.99M, Quantity = 1 },
                new Order { Product = "Lego City", Price = 49.99M, Quantity = 1 },
                new Order { Product = "Baseball", Price = 9.99M, Quantity = 5 },
                new Order { Product = "Bat", Price = 19.99M, Quantity = 1 }
            };

            int firstOrder, lastOrder, tempOrder = 0;
            var ordersLength = orders.Count;
            var customers = new List<Customer>();
            var random = new Random();

            for (var i = 0; i < customerNames.Length; i++)
            {
                var nameGenderHost = customerNames[i].Split(',');
                var cityState = citiesStates[i].Split(',');
                var state = states.Where(s => s.Abbreviation == cityState[1]).SingleOrDefault();

                var customer = new Customer
                {
                    FirstName = nameGenderHost[0],
                    LastName = nameGenderHost[1],
                    Email = nameGenderHost[0] + '.' + nameGenderHost[1] + '@' + nameGenderHost[3],
                    Address = addresses[i],
                    City = cityState[0],
                    State = state,
                    Zip = zip + i,
                    Gender = (Gender)Enum.Parse(typeof(Gender), nameGenderHost[2]),
                    OrderCount = 0
                };

                firstOrder = (int)Math.Floor(random.NextDouble() * orders.Count);
                lastOrder = (int)Math.Floor(random.NextDouble() * orders.Count);

                if (firstOrder > lastOrder)
                {
                    tempOrder = firstOrder;
                    firstOrder = lastOrder;
                    lastOrder = tempOrder;
                }

                customer.Orders = new List<Order>();

                for (var j = firstOrder; j <= lastOrder && j < ordersLength; j++)
                {
                    var order = new Order
                    {
                        Product = orders[j].Product,
                        Price = orders[j].Price,
                        Quantity = orders[j].Quantity
                    };
                    customer.Orders.Add(order);
                }
                customer.OrderCount = customer.Orders.Count;
                customers.Add(customer);
            }

            return customers;
        }

        private List<State> GetStates()
        {
            var states = new List<State>
            {
                new State { Name = "Alabama", Abbreviation = "AL" },
                new State { Name = "Montana", Abbreviation = "MT" },
                new State { Name = "Alaska", Abbreviation = "AK" },
                new State { Name = "Nebraska", Abbreviation = "NE" },
                new State { Name = "Arizona", Abbreviation = "AZ" },
                new State { Name = "Nevada", Abbreviation = "NV" },
                new State { Name = "Arkansas", Abbreviation = "AR" },
                new State { Name = "New Hampshire", Abbreviation = "NH" },
                new State { Name = "California", Abbreviation = "CA" },
                new State { Name = "New Jersey", Abbreviation = "NJ" },
                new State { Name = "Colorado", Abbreviation = "CO" },
                new State { Name = "New Mexico", Abbreviation = "NM" },
                new State { Name = "Connecticut", Abbreviation = "CT" },
                new State { Name = "New York", Abbreviation = "NY" },
                new State { Name = "Delaware", Abbreviation = "DE" },
                new State { Name = "North Carolina", Abbreviation = "NC" },
                new State { Name = "Florida", Abbreviation = "FL" },
                new State { Name = "North Dakota", Abbreviation = "ND" },
                new State { Name = "Georgia", Abbreviation = "GA" },
                new State { Name = "Ohio", Abbreviation = "OH" },
                new State { Name = "Hawaii", Abbreviation = "HI" },
                new State { Name = "Oklahoma", Abbreviation = "OK" },
                new State { Name = "Idaho", Abbreviation = "ID" },
                new State { Name = "Oregon", Abbreviation = "OR" },
                new State { Name = "Illinois", Abbreviation = "IL" },
                new State { Name = "Pennsylvania", Abbreviation = "PA" },
                new State { Name = "Indiana", Abbreviation = "IN" },
                new State { Name = "Rhode Island", Abbreviation = "RI" },
                new State { Name = "Iowa", Abbreviation = "IA" },
                new State { Name = "South Carolina", Abbreviation = "SC" },
                new State { Name = "Kansas", Abbreviation = "KS" },
                new State { Name = "South Dakota", Abbreviation = "SD" },
                new State { Name = "Kentucky", Abbreviation = "KY" },
                new State { Name = "Tennessee", Abbreviation = "TN" },
                new State { Name = "Louisiana", Abbreviation = "LA" },
                new State { Name = "Texas", Abbreviation = "TX" },
                new State { Name = "Maine", Abbreviation = "ME" },
                new State { Name = "Utah", Abbreviation = "UT" },
                new State { Name = "Maryland", Abbreviation = "MD" },
                new State { Name = "Vermont", Abbreviation = "VT" },
                new State { Name = "Massachusetts", Abbreviation = "MA" },
                new State { Name = "Virginia", Abbreviation = "VA" },
                new State { Name = "Michigan", Abbreviation = "MI" },
                new State { Name = "Washington", Abbreviation = "WA" },
                new State { Name = "Minnesota", Abbreviation = "MN" },
                new State { Name = "West Virginia", Abbreviation = "WV" },
                new State { Name = "Mississippi", Abbreviation = "MS" },
                new State { Name = "Wisconsin", Abbreviation = "WI" },
                new State { Name = "Missouri", Abbreviation = "MO" },
                new State { Name = "Wyoming", Abbreviation = "WY" }
            };

            return states;
        }



    }
}