using System.Diagnostics;
using Boost.Core.Settings;
using Serilog;
using Serilog.Events;

namespace Boost.Infrastructure
{
    public class LogConfiguration
    {
        public static void CreateLogger()
        {
            var logPath = SettingsStore.GetUserDirectory("logs");
            LogEventLevel minLevel = LogEventLevel.Information;

            if (Debugger.IsAttached)
            {
                minLevel = LogEventLevel.Debug;
            }

            LoggerConfiguration logBuilder = new LoggerConfiguration()
                .WriteTo.File($"{logPath}/boost.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console(restrictedToMinimumLevel: minLevel);

            logBuilder.MinimumLevel.Debug();
            Log.Logger = logBuilder.CreateLogger();
        }
    }
}
