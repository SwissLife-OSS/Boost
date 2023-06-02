using Boost.Git;
using HotChocolate.Types;

namespace Boost.Core.GraphQL;

public class GitRemoteReferenceType : ObjectType<IGitRemoteReference>
{
    protected override void Configure(IObjectTypeDescriptor<IGitRemoteReference> descriptor)
    {
        base.Configure(descriptor);
    }
}
