using System.Diagnostics;
using Serilog;

namespace Boost.Infrastructure
{
    public class LogConfiguration
    {
        public static void CreateLogger()
        {
            LoggerConfiguration logBuilder = new LoggerConfiguration()
                .WriteTo.Console();

            if (Debugger.IsAttached)
            {
                logBuilder.MinimumLevel.Debug();
            }
            else
            {
                logBuilder.MinimumLevel.Warning();
            }

            Log.Logger = logBuilder.CreateLogger();
        }
    }
}
