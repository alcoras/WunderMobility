using System.Reflection;
using Communication.EventDataService;
using TestWunderMobilityCheckout.Aggregates.Customers.Services;
using TestWunderMobilityCheckout.Aggregates.Products.Services;

namespace TestWunderMobilityCheckout.Actions.ProcessEvents
{
    /// <summary>
    /// Process occupations class
    /// </summary>
    public partial class ProcessEventsWunderMobilityAct : IProcessEventsAct
    {
        private readonly string currentSourceId = Assembly.GetEntryAssembly().GetName().Name;
        private readonly string currentAssemblyName = Assembly.GetEntryAssembly().GetName().Name +
            Assembly.GetEntryAssembly().GetName().Version.ToString();

        private readonly IEventDataFactory<TestWunderMobilityCheckoutDBContext> _eventDataFactory;
        private readonly IProductsFactory _productsFactory;
        private readonly ICustomersFactory _customersFactory;

        /// <summary> Process microservice related events such as subscribe to event </summary>
        /// <param name="eventDataFactory"> Event data factory </param>
        /// <param name="productsFactory"> Products service </param>
        /// <param name="customersFactory"> Customers service </param>
        public ProcessEventsWunderMobilityAct(
            IEventDataFactory<TestWunderMobilityCheckoutDBContext> eventDataFactory,
            IProductsFactory productsFactory,
            ICustomersFactory customersFactory)
        {
            this._eventDataFactory = eventDataFactory;
            this._productsFactory = productsFactory;
            this._customersFactory = customersFactory;
        }
    }
}
