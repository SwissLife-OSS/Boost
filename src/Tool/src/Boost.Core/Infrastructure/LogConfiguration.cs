using System.Diagnostics;
using Boost.Core.Settings;
using Serilog;
using Serilog.Events;

namespace Boost.Infrastructure
{
    public class LogConfiguration
    {
        public static void CreateLogger(string level="warning")
        {
            var logPath = SettingsStore.GetUserDirectory("logs");

            if (Debugger.IsAttached)
            {
                level = "debug";
            }

            LoggerConfiguration logBuilder = new LoggerConfiguration()
                .WriteTo.File($"{logPath}/boost.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console();

            switch (level.ToLower())
            {
                case "error":
                    logBuilder.MinimumLevel.Error();
                    break;
                case "warning":
                default:
                    logBuilder.MinimumLevel.Warning();
                    break;
                case "info":
                    logBuilder.MinimumLevel.Information();
                    break;
                case "debug":
                    logBuilder.MinimumLevel.Debug();
                    break;
            }

            Log.Logger = logBuilder.CreateLogger();
        }
    }
}
