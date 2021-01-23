using System.Reflection;
using Communication.EventDataService;

namespace TestWunderMobilityCheckout.Actions.ProcessEvents
{
    /// <summary>
    /// Process occupations class
    /// </summary>
    public partial class ProcessEventsAct : IProcessEventsAct
    {
        private readonly string currentSourceId = Assembly.GetEntryAssembly().GetName().Name;
        private readonly string currentAssemblyName = Assembly.GetEntryAssembly().GetName().Name +
            Assembly.GetEntryAssembly().GetName().Version.ToString();

        private readonly IEventDataFactory<TestWunderMobilityCheckoutDBContext> _eventDataFactory;

        /// <summary> Process microservice related events such as subscribe to event </summary>
        /// <param name="eventDataFactory"> Event data factory </param>
        public ProcessEventsAct(IEventDataFactory<TestWunderMobilityCheckoutDBContext> eventDataFactory)
        {
            this._eventDataFactory = eventDataFactory;
        }
    }
}
