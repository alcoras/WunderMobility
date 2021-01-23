using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWunderMobilityCheckout.Aggregates.Products.Services;

namespace TestWunderMobilityCheckout.Tests
{
    [TestClass]
    public class ProductsFactory_Tests
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
                    var service = new ProductsFactory(context);
                    var productParams = new ProductParamsDTO(null, "001", "Pizza", 5.99F, 2, 3.99F, false);
                    var status = service.Create(productParams);

                    Assert.IsTrue(!status.HasErrors);
                    await context.SaveChangesAsync();

                    var received = await service.ReadFilteredAsync();
                    
                    Assert.IsTrue(received.Count == 1);

                    Assert.IsTrue(received[0].ProductCode == "001");
                    Assert.IsTrue(received[0].Name == "Pizza");
                    Assert.IsTrue(received[0].Price == 5.99F);
                    Assert.IsTrue(received[0].PromotionalQuantity == 2);
                    Assert.IsTrue(received[0].PromotionalPrice == 3.99F);
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
                    var service = new ProductsFactory(context);
                    var productParams = new ProductParamsDTO(null, "001", "Pizza", 5.99F, 2, 3.99F, false);
                    var status = service.Create(productParams);

                    Assert.IsTrue(!status.HasErrors);
                    await context.SaveChangesAsync();

                    var received = await service.ReadFilteredAsync("001");

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
