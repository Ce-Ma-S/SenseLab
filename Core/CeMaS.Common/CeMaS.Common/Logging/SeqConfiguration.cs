using CeMaS.Common.Properties;

namespace CeMaS.Common.Logging
{
    /// <summary>
    /// Seq configuration keys and default values.
    /// </summary>
    /// <seealso cref="https://getseq.net"/>
    public static class SeqConfiguration
    {
        /// <summary>
        /// Log section name.
        /// </summary>
        public const string Name = "Seq";

        /// <summary>
        /// Server URL.
        /// </summary>
        /// <value>
        /// Key: "<see cref="ServerUrl"/>".
        /// Default value: "http://localhost:5341".
        /// </value>
        public static readonly NamedValue<string> ServerUrl =
            new NamedValue<string>(nameof(ServerUrl), "http://localhost:5341");
        /// <summary>
        /// API key.
        /// </summary>
        /// /// <value>
        /// Key: "<see cref="ApiKey"/>".
        /// Default value: <c>null</c>.
        /// </value>
        public static readonly NamedValue<string> ApiKey =
            new NamedValue<string>(nameof(ApiKey), null);
    }
}
