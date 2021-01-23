using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.EventDataService;
using Communication.SocketLibrary;
using Microsoft.Extensions.Logging;
using TestWunderMobilityCheckout.Actions.ProcessEvents;

namespace TestWunderMobilityCheckout
{
    /// <summary> Process events handled by this microservice class </summary>
    public class ScopedProcessEventsService : IScopedProcessEventsService
    {
        private static AutoResetEvent readyToRace = new AutoResetEvent(true); // Check first time for the former events waiting to race

        private readonly ILogger _logger;
        private readonly IEventDataAction<TestWunderMobilityCheckoutDBContext> _eventDataAction;
        private readonly IProcessEventsAct _processOccupationsAct;

        private Task raceTask;
        private CancellationTokenSource cancellationTokenSource;
        private SocketConnectionHandler currentSocketConnectionHandler;

        /// <summary> Constructor can use dependency injection because of the scoped visibility of class </summary>
        /// <param name="logger"> Logger </param>
        /// <param name="eventDataAction"> Event data action </param>
        /// <param name="processOccupationsAct"> Occupation action service </param>
        public ScopedProcessEventsService(
            ILogger<ScopedProcessEventsService> logger,
            IEventDataAction<TestWunderMobilityCheckoutDBContext> eventDataAction,
            IProcessEventsAct processOccupationsAct)
        {
            this._logger = logger;
            this._eventDataAction = eventDataAction;
            this._processOccupationsAct = processOccupationsAct;
        }

        /// <inheritdoc/>
        public async Task DoWork(
            CancellationToken cancellationToken,
            SocketConnectionHandler currentSocketConnectionHandler,
            Func<CancellationToken, Task> workFunctionAsync)
        {
            this._logger.LogInformation("Events processing service {TaskName} start working.", workFunctionAsync.Method.Name);

            this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            this.currentSocketConnectionHandler = currentSocketConnectionHandler;

            this.raceTask = Task.Run(() => workFunctionAsync(this.cancellationTokenSource.Token));

            while (!this.raceTask.IsCompleted)
            {
                try { await Task.Delay(SocketConnectionHandler.MlsecondsBeforeRecheckingConnection, this.cancellationTokenSource.Token); }
                catch (TaskCanceledException) { break; }

                // Provide connectivity check
                if (!await currentSocketConnectionHandler.ConnectionIsOkAsync())
                {
                    this._logger.LogError("Bad connection detected - reconnect initialized");
                    this.cancellationTokenSource.Cancel();
                    break;
                }
            }

            // Stop activity
            if (!this.cancellationTokenSource.IsCancellationRequested)
                this.cancellationTokenSource.Cancel();

            try
            {
                await this.raceTask;
            }

            // await unwraps exceptions into a real one and throw them
            catch (Exception ex)
            {
                this._logger.LogCritical(ex.Message);
                var additional = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" at "));
                this._logger.LogCritical(additional);
            }

            this._logger.LogInformation("Events processing service {TaskName} is finished.", workFunctionAsync.Method.Name);
        }

        /// <inheritdoc/>
        public async Task RaceTaskAsync(CancellationToken cancellationToken)
        {
            // Subscribe service to receive subscribe event
            await this._processOccupationsAct.SubscribeToEventsAsync();

            while (!cancellationToken.IsCancellationRequested)
            {
                var res = await Task.Run(() => readyToRace.WaitOne(SocketConnectionHandler.MlsecondsBeforeResendingUnconfirmedMessages));
                if (res) this._logger.LogInformation("*********** Send data {time}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                else this._logger.LogInformation("*********** Check connection and resend missed data if available {time}", DateTime.Now);

                if (res) await this._processOccupationsAct.DoEventActionAsync();

                // Process outgoing messages
                if (!await this._eventDataAction.SendOutgoingEventsAsync(this.currentSocketConnectionHandler))
                    break; // Error occured - break connection and repeat a new iteration
            }
        }

        /// <inheritdoc/>
        public async Task ReceiveTaskAsync(CancellationToken cancellationToken)
        {
            // Receive message
            // Check if it is already registered in database, if it is- send answer "message received"
            while (!cancellationToken.IsCancellationRequested)
            {
                var status = await this._eventDataAction.ProcessIncomingEventsAsync(this.currentSocketConnectionHandler);
                if (status.HasErrors)
                {
                    this._logger.LogError(status.GetAllErrors());
                    break;
                }
                else
                {
                    this._logger.LogInformation("Something was received *********** {time}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    readyToRace.Set();
                }
            }
        }
    }
}
