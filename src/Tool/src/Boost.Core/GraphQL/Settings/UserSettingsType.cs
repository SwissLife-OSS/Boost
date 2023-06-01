using Boost.Core.Settings;
using Boost.Settings;
using HotChocolate.Types;

namespace Boost.Core.GraphQL;

public class UserSettingsType : ObjectType<UserSettings>
{
    protected override void Configure(
        IObjectTypeDescriptor<UserSettings> descriptor)
    {
        descriptor.Field("location")
            .Resolve(c => SettingsStore.GetUserDirectory());
    }
}
