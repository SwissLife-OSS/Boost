using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boost.Git;
using Boost.Settings;
using HotChocolate;
using HotChocolate.Types;

namespace Boost.Core.GraphQL
{
    public class LocalGitRepositoryType : ObjectType<GitLocalRepository>
    {
        protected override void Configure(IObjectTypeDescriptor<GitLocalRepository> descriptor)
        {
            descriptor
                .Field("service")
                .ResolveWith<Resolvers>(x => x.GetConnectedServiceAsync(default!, default!, default!));

            descriptor.Field("readme")
                .ResolveWith<Resolvers>(_ => _.GetReadMeAsync(default!, default!, default!));

            descriptor.Field("remote")
                .ResolveWith<Resolvers>(_ => _.GetRemoteAsync(default!, default!, default!));

            descriptor
                .Field("commits")
                .Argument("top", a =>
                {
                    a.DefaultValue(5);
                    a.Type(typeof(int));
                })
                .ResolveWith<Resolvers>(x =>
                    x.GetCommits(default!, default!));
        }

        class Resolvers
        {
            public async Task<ConnectedService?> GetConnectedServiceAsync(
                [Parent] GitLocalRepository repository,
                [DataLoader] ConnectedServiceByIdDataLoader byIdDataLoader,
                CancellationToken cancellationToken)
            {
                if (repository.RemoteServiceId.HasValue)
                {
                    return await byIdDataLoader.LoadAsync(repository.RemoteServiceId.Value, cancellationToken);
                }

                return null;
            }

            public IEnumerable<GitLocalCommit> GetCommits(
                [Parent] GitLocalRepository repository,
                int top)
            {
                return repository.Commits.Take(top);
            }


            public async Task<string?> GetReadMeAsync(
                [Parent] GitLocalRepository repository,
                [Service] IGitLocalRepositoryService gitService,
                CancellationToken cancellationToken)
            {
                byte[]? data = await gitService.GetFileContent(
                    repository.Id,
                    "README.md",
                    cancellationToken);

                if (data is { })
                {
                    return System.Text.Encoding.UTF8.GetString(data);
                }

                return null;
            }

            public async Task<GitRemoteRepository?> GetRemoteAsync(
                [Parent] GitLocalRepository repository,
                [Service] IGitRemoteService gitService,
                CancellationToken cancellationToken)
            {
                if (repository.RemoteReference is { } && repository.RemoteServiceId is { })
                {
                    GitRemoteRepository? remoteRepo = await gitService.GetByRemoteReferenceAsync(
                        repository.RemoteServiceId.Value,
                        repository.RemoteReference,
                        repository.Name,
                        cancellationToken);

                    return remoteRepo;
                }

                return null;
            }
        }
    }
}
