using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Boost.Infrastructure
{
    public class BoostApplicationContext : IBoostApplicationContext
    {
        public DirectoryInfo WorkingDirectory
            => GetWorkingDirectory();

        public string? Version => typeof(BoostApplicationContext)
             .Assembly?
             .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
             .InformationalVersion;

        private DirectoryInfo GetWorkingDirectory()
        {
            if (Debugger.IsAttached)
            {
                var debugDir = Environment.GetEnvironmentVariable("BOOST_DEBUG_WORKDIR");

                if ( debugDir is { })
                {
                    return new DirectoryInfo(debugDir);
                }
            }

            return new DirectoryInfo(Directory.GetCurrentDirectory());
        }
    }

    public class BoostApplication
    {
        public string WorkingDirectory { get; set; } = default!;

        public bool ConfigurationRequired { get; set; }

        public string Version { get; set; } = default!;
    }
}
