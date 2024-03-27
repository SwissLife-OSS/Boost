using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Settings;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL;

public class GitRemoteRepositoryType : ObjectType<GitRemoteRepository>
{
    protected override void Configure(IObjectTypeDescriptor<GitRemoteRepository> descriptor)
    {
        descriptor
            .Field("service")
            .ResolveWith<Resolvers>(x => x.GetConnectedServiceAsync(default!, default!, default!));

        descriptor.Field("readme")
            .ResolveWith<Resolvers>(_ => _.GetReadMeAsync(default!, default!, default!));

        descriptor.Field("local")
            .ResolveWith<Resolvers>(_ => _.GetLocalRepoAsync(default!, default!, default!));

        descriptor
            .Field("commits")
            .Argument("top", a =>
            {
                a.DefaultValue(5);
                a.Type(typeof(int));
            })
            .ResolveWith<Resolvers>(x =>
                x.GetCommitsAsync(default!, default!, default!, default!));
    }

    class Resolvers
    {
        public Task<ConnectedService> GetConnectedServiceAsync(
            [Parent] GitRemoteRepository repository,
            ConnectedServiceByIdDataLoader byIdDataLoader,
            CancellationToken cancellationToken)
        {
            return byIdDataLoader.LoadAsync(repository.ServiceId, cancellationToken);
        }

        public Task<IEnumerable<GitRemoteCommit>> GetCommitsAsync(
            [Parent] GitRemoteRepository repository,
            [Service] IGitRemoteService gitService,
            int top,
            CancellationToken cancellationToken)
        {
            return gitService.GetLastCommitsAsync(
                repository.ServiceId,
                repository.Id,
                top,
                cancellationToken);
        }

        public Task<GitLocalRepository?> GetLocalRepoAsync(
            [Parent] GitRemoteRepository repository,
            [Service] IGitLocalRepositoryService gitService,
            CancellationToken cancellationToken)
        {
            return gitService.GetByRemoteAsync(
                repository.ServiceId,
                repository.Name,
                cancellationToken);
        }

        public async Task<string?> GetReadMeAsync(
            [Parent] GitRemoteRepository repository,
            [Service] IGitRemoteService gitService,
            CancellationToken cancellationToken)
        {
            byte[]? data = await gitService.GetFileContentAsync(
                repository.ServiceId,
                repository.Id,
                "/README.md",
                cancellationToken);

            if (data is { })
            {
                return System.Text.Encoding.UTF8.GetString(data);
            }

            return null;
        }
    }
}
