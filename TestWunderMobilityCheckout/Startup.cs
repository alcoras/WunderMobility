using Encryption.ConfigFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TestWunderMobilityCheckout.Adds;

namespace TestWunderMobilityCheckout
{
    /// <summary> Startup class </summary>
    public static partial class Startup
    {
        /// <summary> Configuration saving variable </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary> Load configuration from files </summary>
        /// <param name="hostContext"> host context </param>
        /// <param name="config"> Configuration </param>
        public static void Configure(HostBuilderContext hostContext, IConfigurationBuilder config)
        {
            // Encrypt some sections of config file
            EncryptionLibrary.Encrypt(
                hostContext.HostingEnvironment.ContentRootPath + "/Configs/AppSettings.Production.json",
                Constants.ConfigFileEncDictionary.Dic,
                GetComputerSpecificInfo.GetComputerSpecificInfoStr());

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                .AddJsonFileSec("Configs/AppSettings.json", optional: true, reloadOnChange: true, dicConfigProvider: Constants.ConfigFileEncDictionary.DicConfigProvider)
                .AddJsonFileSec($"Configs/AppSettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true, dicConfigProvider: Constants.ConfigFileEncDictionary.DicConfigProvider)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}
