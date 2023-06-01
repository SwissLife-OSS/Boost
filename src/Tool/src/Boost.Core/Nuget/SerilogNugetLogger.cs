using System;
using System.Threading.Tasks;
using NuGet.Common;

namespace Boost.Nuget;

public class SerilogNugetLogger : NuGet.Common.ILogger
{
    public void Log(LogLevel level, string data)
    {
        switch (level)
        {
            case LogLevel.Debug:
            case LogLevel.Verbose:
                Serilog.Log.Debug(data);
                break;
            case LogLevel.Information:
            case LogLevel.Minimal:

                Serilog.Log.Information(data);
                break;
            case LogLevel.Warning:
                Serilog.Log.Warning(data);
                break;
            case LogLevel.Error:
                Serilog.Log.Error(data);
                break;
        }
    }

    public void Log(ILogMessage message)
    {
        Log(message.Level, message.Message);
    }

    public Task LogAsync(LogLevel level, string data)
    {
        Log(level, data);

        return Task.CompletedTask;
    }

    public Task LogAsync(ILogMessage message)
    {
        Log(message.Level, message.Message);

        return Task.CompletedTask;
    }

    public void LogDebug(string data)
    {
        Serilog.Log.Debug(data);
    }

    public void LogError(string data)
    {
        Serilog.Log.Error(data);
    }

    public void LogInformation(string data)
    {
        Serilog.Log.Information(data);
    }

    public void LogInformationSummary(string data)
    {
        Serilog.Log.Information(data);
    }

    public void LogMinimal(string data)
    {
        Serilog.Log.Information(data);
    }

    public void LogVerbose(string data)
    {
        Serilog.Log.Verbose(data);
    }

    public void LogWarning(string data)
    {
        Serilog.Log.Warning(data);
    }
}
