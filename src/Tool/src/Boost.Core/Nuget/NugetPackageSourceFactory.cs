using System.Threading;
using System.Threading.Tasks;
using Boost.Core.Settings;
using NuGet.Configuration;

namespace Boost.Nuget
{
    public class NugetPackageSourceFactory
    {
        private readonly string AzureDevOpsSourceUrlTemplate =
            "https://pkgs.dev.azure.com/{0}/_packaging/{1}/nuget/v3/index.json";

        private readonly IConnectedServiceManager _connectedServiceManager;

        public NugetPackageSourceFactory(IConnectedServiceManager connectedServiceManager)
        {
            _connectedServiceManager = connectedServiceManager;
        }

        public async Task<PackageSource?> CreateAsync(string name, CancellationToken cancellationToken)
        {
            switch (name)
            {
                case "nuget.org":
                    return new PackageSource("https://api.nuget.org/v3/index.json");
                default:
                    var sourceUri = string.Format(AzureDevOpsSourceUrlTemplate, name);

                    var packageSource = new PackageSource(sourceUri)
                    {
                        Credentials = new PackageSourceCredential(
                        source: sourceUri,
                        username: "ado",
                        passwordText: "",
                        isPasswordClearText: true,
                        validAuthenticationTypesText: null)
                    };

                    return packageSource;
            }
        }
    }
}
