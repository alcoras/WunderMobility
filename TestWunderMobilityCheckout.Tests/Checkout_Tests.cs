using CommonConstants.EventIds;
using CommonTypes.EventDTOs;
using CommonTypes.EventDTOs.Adds;
using Communication.EventDataService;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWunderMobilityCheckout.Actions.ProcessEvents;
using TestWunderMobilityCheckout.Aggregates.Customers.Services;
using TestWunderMobilityCheckout.Aggregates.Products.Services;

namespace TestWunderMobilityCheckout.Tests
{
    [TestClass]
    public class ProcessEventsWunderMobilityAct_Tests
    {
        [TestMethod]
        public async Task DoEventActionAsync_checkoutAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<TestWunderMobilityCheckoutDBContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new TestWunderMobilityCheckoutDBContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new TestWunderMobilityCheckoutDBContext(options))
                {
                    var _productsFactory = new ProductsFactory(context);
                    var _customersFactory = new CustomersFactory(context);
                    var _eventDataFactory = new EventDataFactory<TestWunderMobilityCheckoutDBContext>(context);

                    _productsFactory.Create(new ProductParamsDTO(null, "001", "Curry Sauce", 1.95F, 0, 0, false));
                    _productsFactory.Create(new ProductParamsDTO(null, "002", "Pizza", 5.99F, 2, 3.99F, false));
                    _productsFactory.Create(new ProductParamsDTO(null, "003", "Men's T-Shirt", 25.0F, 0, 0, false));
                    
                    _customersFactory.Create(new CustomerParamsDTO(null, "All", 30, 10, false));

                    await context.SaveChangesAsync();

                    var service = new ProcessEventsWunderMobilityAct(_eventDataFactory, _productsFactory, _customersFactory);

                    var testEvent1 = new TestWunderMobilityCheckoutDoCheckout()
                    {
                        EventId = (int)ListOfIds.TestWunderMobilityDoCheckout,
                        ProductCodeList = new List<WunderMobilityCheckout>() 
                        {
                            new WunderMobilityCheckout() { ProductCode = "001", Quantity = 1 },
                            new WunderMobilityCheckout() { ProductCode = "002", Quantity = 1 },
                            new WunderMobilityCheckout() { ProductCode = "003", Quantity = 1 },
                        },
                    };

                    var testEvent2 = new TestWunderMobilityCheckoutDoCheckout()
                    {
                        EventId = (int)ListOfIds.TestWunderMobilityDoCheckout,
                        ProductCodeList = new List<WunderMobilityCheckout>() 
                        {
                            new WunderMobilityCheckout() { ProductCode = "001", Quantity = 1 },
                            new WunderMobilityCheckout() { ProductCode = "002", Quantity = 2 },
                        },
                    };

                    var testEvent3 = new TestWunderMobilityCheckoutDoCheckout()
                    {
                        EventId = (int)ListOfIds.TestWunderMobilityDoCheckout,
                        ProductCodeList = new List<WunderMobilityCheckout>() 
                        {
                            new WunderMobilityCheckout() { ProductCode = "001", Quantity = 1 },
                            new WunderMobilityCheckout() { ProductCode = "002", Quantity = 2 },
                            new WunderMobilityCheckout() { ProductCode = "003", Quantity = 1 },
                        },
                    };

                    _eventDataFactory.RegisterEvent( new EventDataParamsDTO(null, false, 1, null, JsonConvert.SerializeObject(testEvent1)));
                    _eventDataFactory.RegisterEvent( new EventDataParamsDTO(null, false, 2, null, JsonConvert.SerializeObject(testEvent2)));
                    _eventDataFactory.RegisterEvent( new EventDataParamsDTO(null, false, 3, null, JsonConvert.SerializeObject(testEvent3)));
                    await context.SaveChangesAsync();

                    await service.DoEventActionAsync();
                    await context.SaveChangesAsync();

                    var listOfEvents = _eventDataFactory.GetListOfUnprocessedEventsToResend(0);
                    Assert.IsTrue(_eventDataFactory.GetListOfUnprocessedEvents().Count == 0);
                    Assert.IsTrue(_eventDataFactory.GetListOfUnprocessedEventsToResend(0).Count == 3);

                    // test in reverse order
                    var eventDTO = JsonConvert.DeserializeObject<TestWunderMobilityCheckoutDoCheckoutResults>(listOfEvents[4]);
                    Assert.IsTrue(Math.Round(eventDTO.TotalPrice, 2) == 31.44);
                    
                    eventDTO = JsonConvert.DeserializeObject<TestWunderMobilityCheckoutDoCheckoutResults>(listOfEvents[5]);
                    Assert.IsTrue(Math.Round(eventDTO.TotalPrice, 2) == 9.93);

                    eventDTO = JsonConvert.DeserializeObject<TestWunderMobilityCheckoutDoCheckoutResults>(listOfEvents[6]);
                    Assert.IsTrue(Math.Round(eventDTO.TotalPrice, 2) == 29.65);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
