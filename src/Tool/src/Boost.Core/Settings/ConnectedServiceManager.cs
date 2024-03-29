using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Infrastructure;
using Boost.Settings;

namespace Boost.Core.Settings;

public class ConnectedServiceManager : IConnectedServiceManager
{
    private readonly ISettingsStore _settingsStore;
    private readonly IUserDataProtector _userDataProtector;
    private readonly IEnumerable<IConnectedServiceProvider> _providers;
    private const string SettingPath = "connected";

    public ConnectedServiceManager(
        ISettingsStore settingsStore,
        IUserDataProtector userDataProtector,
        IEnumerable<IConnectedServiceProvider> providers)
    {
        _settingsStore = settingsStore;
        _userDataProtector = userDataProtector;
        _providers = providers;
    }

    private IEnumerable<IConnectedServiceProvider> GitProviders => _providers.Where(x =>
                x.Type.Features.Contains(ConnectedServiceFeature.GitRemoteRepository));

    public IEnumerable<ConnectedServiceType> GetServiceTypes()
    {
        return _providers.Select(x => x.Type);
    }

    public async Task<ConnectedService> SaveAsync(
        ConnectedService connectedService,
        CancellationToken cancellationToken)
    {
        ProtectSecrets(connectedService);

        await _settingsStore.SaveAsync(
            connectedService,
            connectedService.Id.ToString("N"),
            SettingPath,
            cancellationToken);

        return connectedService;
    }

    private void ProtectSecrets(ConnectedService connectedService)
    {
        IConnectedServiceProvider provider = _providers
            .First(x => x.Type.Name == connectedService.Type);

        foreach (ConnectedServiceProperty property in connectedService.Properties
            .Where(x => provider.Type.SecretProperties.Contains(x.Name)))
        {
            if (property.Value is string)
            {
                property.Value = _userDataProtector.Protect(property.Value);
                property.IsSecret = true;
            }
        }
    }

    private void UnProtectSecrets(ConnectedService connectedService)
    {
        foreach (ConnectedServiceProperty property in connectedService.Properties
            .Where(x => x.IsSecret.GetValueOrDefault()))
        {
            if (property.Value is string)
            {
                property.Value = _userDataProtector.UnProtect(property.Value);
            }
        }
    }

    public async Task<IEnumerable<IConnectedService>> GetServicesAsync(
        CancellationToken cancellationToken)
    {
        var path = SettingsStore.GetUserDirectory(SettingPath);
        var services = new List<IConnectedService>();

        foreach (FileInfo file in new DirectoryInfo(path).GetFiles("*.json"))
        {
            if (Guid.TryParse(Path.GetFileNameWithoutExtension(file.Name), out Guid id))
            {
                IConnectedService? service = await GetAsync(
                    id,
                    cancellationToken);

                if (service is { })
                {
                    services.Add(service);
                }
            }
        }

        return services;
    }

    public async Task<IEnumerable<IConnectedService>> GetServicesByFeatureAsync(
        ConnectedServiceFeature feature,
        CancellationToken cancellationToken)
    {
        List<IConnectedService>? byFeature = new List<IConnectedService>();

        IEnumerable<IConnectedService> services = await GetServicesAsync(cancellationToken);
        IEnumerable<ConnectedServiceType>? types = GetServiceTypes();

        foreach (IConnectedService service in services)
        {
            ConnectedServiceType type = types.Single(x => x.Name == service.Type);

            if (type.Features.Contains(feature))
            {
                byFeature.Add(service);
            }
        }

        return byFeature;
    }

    public async Task<ConnectedService?> GetServiceAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        ConnectedService? service = await _settingsStore
            .GetAsync<ConnectedService>(
                id.ToString("N"),
                SettingPath,
                cancellationToken);

        if (service is { })
        {
            UnProtectSecrets(service);
        }

        return service;
    }

    public async Task<IConnectedService?> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        ConnectedService? service = await GetServiceAsync(id, cancellationToken);

        if (service is { })
        {
            IConnectedServiceProvider? mapper = _providers
                .FirstOrDefault(x => x.Type.Name == service.Type);

            if (mapper is null)
            {
                throw new Exception(
                    $"Not mapper registred for ConnectedService type: {service.Type}");
            }

            return mapper.MapService(service);
        }

        return null;
    }

    public IGitRemoteReference? GetGitRemoteReference(IEnumerable<string> urls)
    {
        foreach (IConnectedServiceProvider? sp in GitProviders)
        {
            IGitRemoteReference? remote = sp.ParseRemoteUrl(urls);

            if (remote is { })
            {
                return remote;
            }
        }

        return null;
    }

    public IConnectedService? MatchServiceFromGitRemote(
        IGitRemoteReference? gitRemoteReference,
        IEnumerable<IConnectedService> connectedServices)
    {
        if (gitRemoteReference is { })
        {
            foreach (IConnectedServiceProvider? provider in GitProviders)
            {
                IConnectedService? service = provider.MatchServiceFromGitRemoteReference(
                    gitRemoteReference,
                    connectedServices);

                if (service is { })
                {
                    return service;
                }
            }
        }

        return null;
    }
}
