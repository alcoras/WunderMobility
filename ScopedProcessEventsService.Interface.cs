using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.SocketLibrary;

namespace TestWunderMobilityCheckout
{
    /// <summary> Process events handled by this microservice interface </summary>
    public interface IScopedProcessEventsService
    {
        /// <summary>
        /// Scoped process event service starting point
        /// </summary>
        /// <param name="stoppingToken"> Cancellation token </param>
        /// <param name="currentSocketConnectionHandler"> Socket connection class </param>
        /// <param name="workFunctionAsync"> What to run </param>
        /// <returns> Task </returns>
        Task DoWork(
            CancellationToken stoppingToken,
            SocketConnectionHandler currentSocketConnectionHandler,
            Func<CancellationToken, Task> workFunctionAsync);

        /// <summary>
        /// Process incoming events ready to be processed
        /// Subscribe (save to database), generate event "Subscription completed", mark event
        /// as done - all in one transaction
        /// </summary>
        /// <param name="cancellationToken"> Cancellation token </param>
        /// <returns> Task </returns>
        Task RaceTaskAsync(CancellationToken cancellationToken);

        /// <summary> Receive data until fail or cancellation </summary>
        /// <param name="cancellationToken"> Cancellation token </param>
        /// <returns> Task </returns>
        Task ReceiveTaskAsync(CancellationToken cancellationToken);
    }
}
