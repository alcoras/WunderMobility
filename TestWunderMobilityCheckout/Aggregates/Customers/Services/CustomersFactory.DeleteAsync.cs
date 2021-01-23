using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestWunderMobilityCheckout.Aggregates.Customers.Services
{
    public partial class CustomersFactory
    {
        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            var customerToDelete = await this.DBContext.CustomersList.SingleOrDefaultAsync(e => e.Id == id);
            if (customerToDelete != null)
                this.DBContext.CustomersList.Remove(customerToDelete);
        }
    }
}
