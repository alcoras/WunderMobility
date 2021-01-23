using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWunderMobilityCheckout.Aggregates.Customers.Services;

namespace TestWunderMobilityCheckout.Tests
{
    [TestClass]
    public class CustomersFactory_Tests
    {
        [TestMethod]
        public async Task Create00ReadFilteredAsync_UsualData_ReadCorrespondsWhatsCreated()
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
                    var service = new CustomersFactory(context);
                    var customersParams = new CustomerParamsDTO(null, "All", 30, 10, false);
                    var status = service.Create(customersParams);

                    Assert.IsTrue(!status.HasErrors);
                    await context.SaveChangesAsync();

                    var received = await service.ReadFilteredAsync();
                    
                    Assert.IsTrue(received.Count == 1);

                    Assert.IsTrue(received[0].Name == "All");
                    Assert.IsTrue(received[0].PromotionalSum == 30);
                    Assert.IsTrue(received[0].PromotionalDiscount == 10);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task DeleteAsync_UsualData_SuccessDelete()
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
                    var service = new CustomersFactory(context);
                    var customersParams = new CustomerParamsDTO(null, "All", 30, 10, false);
                    var status = service.Create(customersParams);

                    Assert.IsTrue(!status.HasErrors);
                    await context.SaveChangesAsync();

                    var received = await service.ReadFilteredAsync("All");

                    Assert.IsTrue(received.Count == 1);

                    await service.DeleteAsync(1);
                    await context.SaveChangesAsync();

                    received = await service.ReadFilteredAsync();
                    Assert.IsTrue(received.Count == 0);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
