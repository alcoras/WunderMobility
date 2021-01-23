using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestWunderMobilityCheckout.Aggregates.Products.Models;

namespace TestWunderMobilityCheckout.Aggregates.Products.Services
{
    public partial class ProductsFactory
    {
        /// <inheritdoc/>
        public async Task<List<ProductParamsDTO>> ReadFilteredAsync(List<string> productCodeList = null)
        {
            var query = from productElement in this.DBContext.Set<ProductList>()
                              select productElement;
            if (productCodeList != null)
            {
                query = query.Where(p => productCodeList.Contains(p.ProductCode));
            }

            var queryResult = await query.ToListAsync();

            var ret = new List<ProductParamsDTO>();

            foreach (var p in queryResult)
            {
                ret.Add(new ProductParamsDTO(p.Id, p.ProductCode, p.Name, p.Price, p.PromotionalQuantity, p.PromotionalPrice, p.IsDeleted));
            }

            return ret;
        }
    }
}
