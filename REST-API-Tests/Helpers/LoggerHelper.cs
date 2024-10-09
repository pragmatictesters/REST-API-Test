using Microsoft.Extensions.Logging;  // Make sure this is present
using NLog.Extensions.Logging;

namespace REST_API_Tests.Helpers
{
    public static class LoggerHelper
    {
        public static ILogger<T> CreateLogger<T>()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);  // Explicitly specify LogLevel
                builder.AddNLog("nlog.config");
            });

            return loggerFactory.CreateLogger<T>();
        }

        public static void Shutdown()
        {
            NLog.LogManager.Shutdown();  // Use NLog LogManager explicitly
        }
    }
}