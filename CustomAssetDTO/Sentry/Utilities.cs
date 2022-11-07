using Newtonsoft.Json;
using System.Globalization;

namespace CustomAssetDTO.Sentry
{
    public static class Utilities
    {
        /*
        private static SentryOptions _sentryOptions = new SentryOptions
        {
            // Tells which project in Sentry to send events to:
            Dsn = "https://77b85586308d445184b518ccab1542cb@o1208746.ingest.sentry.io/6778961",
            Debug = true,
            TracesSampleRate = 0.2,
            IsGlobalModeEnabled = true,
            AttachStacktrace = true
        };
        */
        internal const string Version = "1.2.0.0";

        public static JsonSerializerSettings options = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
        };
    }
}
