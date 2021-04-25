using System.Diagnostics;
using Serilog;

namespace Boost.Infrastructure
{
    internal class LogConfiguration
    {
        internal static void CreateLogger()
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
