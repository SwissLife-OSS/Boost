using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Boost
{
    internal static class BoostApplicationFactory
    {
        private static IDataProtector? _protector;

        private static Dictionary<string, int> _environmentPortMapping;

        static BoostApplicationFactory()
        {
            _environmentPortMapping = new(StringComparer.InvariantCultureIgnoreCase)
            {
                ["a"] = 3003,
                ["uat"] = 3004,
                ["pav"] = 3005,
            };
        }

        public static int GetPort(string environment)
        {
            if (!_environmentPortMapping.ContainsKey(environment))
            {
                throw new ApplicationException(
                    $"Invalid environment: {environment}. " +
                    $"Use: {string.Join(",", _environmentPortMapping.Select(x => x.Key).ToArray())}");
            }

            return _environmentPortMapping[environment];
        }

        private static IDataProtector GetProtector()
        {
            if (_protector is { })
            {
                return _protector;
            }
            else
            {
                _protector = new ServiceCollection()
                    .AddBoostDataProtection()
                    .BuildServiceProvider()
                    .GetRequiredService<IDataProtectionProvider>()
                    .CreateProtector("boost");

                return _protector;
            }
        }
    }
}
