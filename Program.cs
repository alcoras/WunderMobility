using System;
using System.Threading.Tasks;
using Encryption.ConfigFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestWunderMobilityCheckout.Adds;

namespace TestWunderMobilityCheckout
{
    /// <summary> Start point startup are loaded from here </summary>
    public class Program
    {
        /// <summary>
        /// Start point
        /// </summary>
        /// <param name="args"> Args </param>
        /// <returns> Task </returns>
        public static async Task Main(string[] args)
        {
            using (var host = CreateHostBuilder(args)
               .Build())
            {
                await host.StartAsync();

                await host.WaitForShutdownAsync();
            }
        }

        /// <summary> Create host builder </summary>
        /// <param name="args"> args </param>
        /// <returns> Hostbuilder </returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostContext, config) =>
               {
                   Startup.Configure(hostContext, config);
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.Configure<HostOptions>(option =>
                   {
                       // Set up shutdown delay here
                       option.ShutdownTimeout = System.TimeSpan.FromSeconds(20);
                   });
                   Startup.ConfigureServices(services);
               })
               .ConfigureHostConfiguration((config) =>
               {
                   config.AddJsonFileSec("Configs/AppSettings.json", optional: true, reloadOnChange: true, dicConfigProvider: Constants.ConfigFileEncDictionary.DicConfigProvider);
                   var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                   if (env == null)
                       env = "Production";
                   config.AddJsonFileSec($"Configs/AppSettings.{env}.json", optional: true, reloadOnChange: true, dicConfigProvider: Constants.ConfigFileEncDictionary.DicConfigProvider);
               });
    }
}
