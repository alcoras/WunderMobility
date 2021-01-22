using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestWunderMobilityCheckout.Aggregates.Products.Services
{
    public partial class ProductsFactory
    {
        /// <inheritdoc/>
        public async Task DeleteAsync(long id)
        {
            var productToDelete = await this.DBContext.ProductList.SingleOrDefaultAsync(e => e.Id == id);
            if (productToDelete != null)
                this.DBContext.ProductList.Remove(productToDelete);
        }
    }
}
