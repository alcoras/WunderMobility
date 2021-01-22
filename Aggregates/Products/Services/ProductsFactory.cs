using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWunderMobilityCheckout.Aggregates.Products.Services
{
    /// <inheritdoc/>
    public partial class ProductsFactory : IProductsFactory
    {
        private readonly TestWunderMobilityCheckoutDBContext _currentDBContext;

        /// <summary>
        /// Constructor <see cref="ProductsFactory"/>
        /// </summary>
        /// <param name="currentDBContext"> DBContext from dependency injection </param>
        public ProductsFactory(TestWunderMobilityCheckoutDBContext currentDBContext)
        {
            this._currentDBContext = currentDBContext;
        }

        /// <summary> property to get or set (prefarably none but in rare cases) Dbcontext (transactional purposes) </summary>
        public TestWunderMobilityCheckoutDBContext DBContext
        {
            get => this._currentDBContext;
        }
    }
}
