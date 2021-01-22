using System.Collections.Generic;

namespace TestWunderMobilityCheckout.Adds
{
    /// <summary> Embedded constants </summary>
    public static class Constants
    {
        /// <summary> Embedded constants </summary>
        public static class Strings
        {
            /// <summary> Embedded constants </summary>
            public static class JwtClaimIdentifiers
            {
                /// <summary> Embedded constants </summary>
                public const string Rol = "rol";

                /// <summary> Embedded constants </summary>
                public const string Id = "id";
            }

            /// <summary> Embedded constants </summary>
            public static class JwtClaims
            {
                /// <summary>Inbedded constants</summary>
                public const string ApiAccess = "api_access";
            }
        }

        /// <summary> Encrypt some sections of config file </summary>
        public static class ConfigFileEncDictionary
        {
#pragma warning disable SA1401 // Fields should be private
            /// <summary>The list of sections to encrypt</summary>
            public static Dictionary<string, string> Dic = new Dictionary<string, string>
            {
                { "Database.ConnectionAuth", "Data Source=" },
            };

            /// <summary> The list of sections to decrypt for configuration provider ("." => ":" in dic key)</summary>
            public static Dictionary<string, string> DicConfigProvider = new Dictionary<string, string>
            {
                { "Database:ConnectionAuth", "Data Source=" },
            };
        }
    }
}
