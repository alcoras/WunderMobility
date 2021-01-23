using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWunderMobilityCheckout.Aggregates.Customers.Services
{
    /// <inheritdoc/>
    public partial class CustomersFactory : ICustomersFactory
    {
        private readonly TestWunderMobilityCheckoutDBContext _currentDBContext;

        /// <summary>
        /// Constructor <see cref="CustomersFactory"/>
        /// </summary>
        /// <param name="currentDBContext"> DBContext from dependency injection </param>
        public CustomersFactory(TestWunderMobilityCheckoutDBContext currentDBContext)
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
