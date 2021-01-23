using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Communication.SocketLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestWunderMobilityCheckout
{
    /// <summary> Hosted background service class </summary>
    public class HostedService : BackgroundService
    {
        private const int DELAYtoReconnectMsec = 5000;

        private readonly string currentSourceId = Assembly.GetEntryAssembly().GetName().Name;
        private readonly string currentAssemblyName = Assembly.GetEntryAssembly().GetName().Name +
            Assembly.GetEntryAssembly().GetName().Version.ToString();

        private readonly ILogger<HostedService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly IConfiguration _configuration;

        private int listeningPort;

        private IServiceProvider _services { get; }

        private Task doWorkTask;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="services"> Service provider </param>
        /// <param name="logger"> Logger </param>
        /// <param name="appLifetime"> App life time </param>
        /// <param name="configuration"> Configuration </param>
        public HostedService(
            IServiceProvider services,
            ILogger<HostedService> logger,
            IHostApplicationLifetime appLifetime,
            IConfiguration configuration)
        {
            this._services = services;
            this._logger = logger;
            this._appLifetime = appLifetime;
            this.cancellationTokenSource = new CancellationTokenSource();
            this._configuration = configuration;
            this.listeningPort = this._configuration.GetValue<int>("SocketConnection:Port");
        }

        /// <summary> Dispose all available classes </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// Hosted service stop point
        /// </summary>
        /// <param name="stoppingToken"> Stopping token </param>
        /// <returns> Task </returns>
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            if (!this.cancellationTokenSource.IsCancellationRequested)
            {
                this._logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");
                this.cancellationTokenSource.Cancel();
                try
                {
                    await Task.WhenAll(this.doWorkTask);
                }

                // await unwraps exceptions into a real one and throw them
                catch (Exception ex)
                {
                    this._logger.LogCritical(ex.Message);
                    var additional = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" at "));
                    this._logger.LogCritical(additional);
                }

                this._logger.LogInformation("Consume Scoped Service Hosted Service is stopped.");
            }
        }

        /// <summary>
        /// Hosted service staring point
        /// </summary>
        /// <param name="stoppingToken"> Stopping token </param>
        /// <returns> Task </returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._logger.LogInformation("Consume Scoped Service Hosted Service running using port {Port}", this.listeningPort);
            this._appLifetime.ApplicationStopping.Register(this.OnStopping);

            this.doWorkTask = Task.Run(async () => await this.DoWork(this.cancellationTokenSource.Token));
            try
            {
                await Task.WhenAll(this.doWorkTask);
            }

            // await unwraps exceptions into a real one and throw them
            catch (Exception ex)
            {
                this._logger.LogCritical(ex.Message);
                var additional = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" at "));
                this._logger.LogCritical(additional);
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            this._logger.LogInformation("Try connect EventBroker...");
            var processEventsTask = Task.Run(async () => await this.processEventsTaskAsync(cancellationToken));

            while (!cancellationToken.IsCancellationRequested)
            {
                // Repeat connection until cancellation token activated
                if (processEventsTask.IsCompleted)
                {
                    if (processEventsTask.Status == TaskStatus.Faulted)
                    {
                        // Exception property holds a list of exceptions in a unwrapped
                        foreach (var ex in processEventsTask.Exception.InnerExceptions)
                        {
                            this._logger.LogCritical(ex.Message);
                            var additional = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" at "));
                            this._logger.LogCritical(additional);
                        }
                    }

                    this._logger.LogInformation("Try connect EventBroker...");
                    processEventsTask = Task.Run(async () => await this.processEventsTaskAsync(cancellationToken));
                }

                // Connection timeout- wait
                await Task.Delay(DELAYtoReconnectMsec);
            }

            try
            {
                await Task.WhenAll(processEventsTask);
            }

            // await unwraps exceptions into a real one and throw them
            catch (Exception ex)
            {
                this._logger.LogCritical(ex.Message);
                var additional = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" at "));
                this._logger.LogCritical(additional);
            }
        }

        private void OnStopping()
        {
            this.StopAsync(this.cancellationTokenSource.Token).Wait();
        }

        /// <summary> Task to process events that handled by this microservice </summary>
        private async Task processEventsTaskAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                using (var socketConnect = new SocketConnectionHandler(this.cancellationTokenSource.Token))
                {
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddress = this.firstIpAddress(AddressFamily.InterNetwork);
                    socketConnect.StartConnection(ipAddress, this.listeningPort);
                    var connectedSocketTask = await socketConnect.GetConnectedSocketAsync(SocketConnectionHandler.MlsecondsBeforeRecheckingConnection);

                    if (connectedSocketTask != null)
                    {
                        // Init connection with microservice Id
                        if (await socketConnect.SendMicroserviceIdTroughConnectionAsync(this.currentSourceId) == true)
                        {
                            using (var scope1 = this._services.CreateScope())
                            using (var scope2 = this._services.CreateScope())
                            {
                                this._logger.LogInformation("* Outgoing connection to EventBroker established");
                                var scopedProcessEventsService1 = scope1.ServiceProvider.GetRequiredService<IScopedProcessEventsService>();
                                var scopedProcessEventsService2 = scope2.ServiceProvider.GetRequiredService<IScopedProcessEventsService>();

                                var task1 = scopedProcessEventsService1.DoWork(cancellationToken, socketConnect, scopedProcessEventsService1.RaceTaskAsync);
                                var task2 = scopedProcessEventsService2.DoWork(cancellationToken, socketConnect, scopedProcessEventsService2.ReceiveTaskAsync);

                                try
                                {
                                    Task.WaitAll(task1, task2);
                                }
                                catch (AggregateException ae)
                                {
                                    foreach (var ex in ae.Flatten().InnerExceptions)
                                    {
                                        this._logger.LogCritical(ex.Message);
                                        var additional = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" at "));
                                        this._logger.LogCritical(additional);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private IPAddress firstIpAddress(AddressFamily currentAddressFamily)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = null;
            foreach (var ipAddressCurrent in ipHostInfo.AddressList)
            {
                if (ipAddressCurrent.AddressFamily == currentAddressFamily)
                {
                    ipAddress = ipAddressCurrent;
                    break;
                }
            }

            return ipAddress;
        }
    }
}
