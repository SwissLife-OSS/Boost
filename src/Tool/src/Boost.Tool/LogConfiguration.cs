using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Boost.Tool
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
