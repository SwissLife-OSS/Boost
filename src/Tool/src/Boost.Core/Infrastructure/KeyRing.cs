using System.IO;
using System.Text.Json;
using Boost.Core.Settings;

namespace Boost.Infrastructure;

public class KeyRing
{
    private static string GetPath => Path.Combine(SettingsStore.GetUserDirectory(), "Keyring.json");

    public static DataProtectorKeyRing Load()
    {
        if (File.Exists(GetPath))
        {
            var json = File.ReadAllText(GetPath);
            DataProtectorKeyRing? data = JsonSerializer.Deserialize<DataProtectorKeyRing>(json);

            return data ?? new DataProtectorKeyRing();
        }

        return new DataProtectorKeyRing();
    }

    public static void Save(DataProtectorKeyRing keyRing)
    {
        string json = JsonSerializer.Serialize(keyRing);

        File.WriteAllTextAsync(GetPath, json);
    }
}
