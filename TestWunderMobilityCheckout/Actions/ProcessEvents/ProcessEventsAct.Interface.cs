using System.Threading.Tasks;

namespace TestWunderMobilityCheckout.Actions.ProcessEvents
{
    /// <summary>
    /// Process occupations interface
    /// </summary>
    public interface IProcessEventsAct
    {
        /// <summary>
        /// Subscribe to microservice events
        /// </summary>
        /// <returns> Async task </returns>
        public Task SubscribeToEventsAsync();

        /// <summary>
        /// Process incoming events
        /// </summary>
        /// <returns> Async task </returns>
        public Task DoEventActionAsync();
    }
}
