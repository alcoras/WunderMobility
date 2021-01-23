using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestWunderMobilityCheckout.Aggregates.Customers.Models;

namespace TestWunderMobilityCheckout.Aggregates.Customers.Services
{
    public partial class CustomersFactory
    {
        /// <inheritdoc/>
        public async Task<List<CustomerParamsDTO>> ReadFilteredAsync(string Name = null)
        {
            var queryResult = await (from customerElement in this.DBContext.Set<CustomersList>()
                              where customerElement.Name == Name || string.IsNullOrEmpty(Name)
                              select customerElement).ToListAsync();

            var ret = new List<CustomerParamsDTO>();

            foreach (var p in queryResult)
            {
                ret.Add(new CustomerParamsDTO(p.Id, p.Name, p.PromotionalSum, p.PromotionalDiscount, p.IsDeleted));
            }

            return ret;
        }
    }
}
