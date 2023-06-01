using System;
using System.Linq;

namespace Boost.Settings;

public static class ConnectedServiceExtensions
{
    public static string GetPropertyValue(
        this ConnectedService service,
        string name)
    {
        var value = service.TryGetPropertyValue<string>(name);

        if ( value is null)
        {
            throw new ApplicationException($"Property {name} is null");
        }

        return value;
    }

    public static T? TryGetPropertyValue<T>(
    this ConnectedService service,
    string name)
    {
        ConnectedServiceProperty? value = service.Properties.FirstOrDefault(x => x.Name.Equals(
            name,
            StringComparison.InvariantCultureIgnoreCase));

        if (value is { })
        {
            Type returnType = typeof(T);

            return (T)Convert.ChangeType(value.Value, returnType);

        }

        return default;
    }
}
