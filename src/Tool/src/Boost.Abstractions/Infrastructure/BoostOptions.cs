using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boost.Infrastructure
{
    public class BoostOptions
    {
        public string Environment { get; set; } = "A";

        public SecurityOptions Security { get; set; }
    }

    public class SecurityOptions
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }

    public static class AzureSettings
    {
        public const string KeyVaultUrl = "https://spc-a-config-dev.vault.azure.net/";
        public const string TenantId = "ab3ae8a3-fd32-4b83-831e-919c6fcd28b2";
        public const string AzureDevOpsTokenKeyVaultSecretName = "Build-AzureDevOps-Token";
    }
}
