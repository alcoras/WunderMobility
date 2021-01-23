using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestWunderMobilityCheckout.Aggregates.Customers.Services;

namespace TestWunderMobilityCheckout.Tests
{
    [TestClass]
    public class Checkout_Tests
    {
        [TestMethod]
        public async Task Checkout_Test()
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
                    
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
