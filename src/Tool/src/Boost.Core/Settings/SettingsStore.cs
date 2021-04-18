using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Boost.Settings;

namespace Boost.Core.Settings
{
    public class SettingsStore : ISettingsStore
    {
        private const string AppName = "boost";

        public async Task SaveAsync<T>(
            T userSettings,
            string fileName,
            string directory = "",
            CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(userSettings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var path = Path.Combine(GetUserDirectory(directory), $"{fileName}.json");

            await File.WriteAllTextAsync(path, json, cancellationToken);
        }

        public async Task<T?> GetAsync<T>(
            string fileName,
            string directory = "",
            CancellationToken cancellationToken = default)
        {
            var path = Path.Combine(GetUserDirectory(directory), $"{fileName}.json");

            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path, cancellationToken);
                return JsonSerializer.Deserialize<T>(json);
            }

            return default;
        }

        internal static string GetUserDirectory(string directory = "")
        {
            var appData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                AppName, directory);

            if (!Directory.Exists(appData))
            {
                Directory.CreateDirectory(appData);
            }

            return appData;
        }
    }
}
