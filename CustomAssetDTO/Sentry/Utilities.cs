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
        internal const string Version = "1.1.0.0";

        public static JsonSerializerSettings options = new JsonSerializerSettings
        {
            Culture = CultureInfo.InvariantCulture,
            // Culture = CultureInfo.GetCultureInfo("de-DE"),
        };
        /*
        private static Action<Scope> _scope = scope =>
        {
            scope.User = new User
            {
                Username = BackendManager.Username,
            };
            scope.Release = Version;
        };

        public static void SentryInvoke(Action a, ManualLogSource _logger)
        {

        }

        private static void logFowarding(object o, LogEventArgs e)
        {
            if (useSentry != logToSentry.Enabled) return;
            switch (e.Level)
            {
                case LogLevel.Fatal:
                    SentrySdk.CaptureMessage(e.Data.ToString(), _scope, SentryLevel.Fatal);
                    break;
                case LogLevel.Error:
                    SentrySdk.CaptureMessage(e.Data.ToString(), _scope, SentryLevel.Error);
                    break;
            }
        }

        private static void relay(object o, LogEventArgs e)
        {
            switch (e.Level)
            {
                case BepInEx.Logging.LogLevel.Fatal:
                    SentrySdk.CaptureMessage(e.Data.ToString(), _scope, SentryLevel.Fatal);
                    break;
                case BepInEx.Logging.LogLevel.Error:
                    SentrySdk.CaptureMessage(e.Data.ToString(), _scope, SentryLevel.Error);
                    break;
            }
        }*/
    }
}
