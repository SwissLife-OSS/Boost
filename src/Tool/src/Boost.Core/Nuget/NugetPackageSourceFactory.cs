using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using Boost.Account;

namespace Boost.Nuget
{
    public class NugetPackageSourceFactory
    {
        private readonly ICredentialStore _credentialStore;
        private readonly string AzureDevOpsSourceUrlTemplate =
            "https://pkgs.dev.azure.com/swisslife/_packaging/{0}/nuget/v3/index.json";

        public NugetPackageSourceFactory(ICredentialStore credentialStore)
        {
            _credentialStore = credentialStore;
        }

        public async Task<PackageSource?> CreateAsync(string name, CancellationToken cancellationToken)
        {
            switch (name)
            {
                case "nuget.org":
                    return new PackageSource("https://api.nuget.org/v3/index.json");
                default:
                    var sourceUri = string.Format(AzureDevOpsSourceUrlTemplate, name);
                    Credential? credentials = await _credentialStore.GetAsync(
                        CredentialNames.AzureDevOpsToken, global: true, cancellationToken);

                    if (credentials is null)
                    {
                        throw new ApplicationException("No credentials store for AzureDevOps");
                    }

                    var packageSource = new PackageSource(sourceUri)
                    {
                        Credentials = new PackageSourceCredential(
                        source: sourceUri,
                        username: "ado",
                        passwordText: credentials.Secret,
                        isPasswordClearText: true,
                        validAuthenticationTypesText: null)
                    };

                    return packageSource;
            }
        }
    }
}
