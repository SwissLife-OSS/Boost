using System.IO;
using Boost.Core.Settings;
using LiteDB;

namespace Boost.Data;

public class BoostDbContextFactory : IBoostDbContextFactory
{
    IBoostDbContext? _dbContext = null;

    public IBoostDbContext Open(DbOpenMode mode)
    {
        string path = Path.Combine(SettingsStore.GetUserDirectory(), "boost.db");
        bool readOnly = (mode == DbOpenMode.ReadOnly) ? true : false;

        readOnly = false;

        if (_dbContext is null)
        {
            _dbContext = new BoostDbContext(
                new LiteDatabase($"Filename={path};ReadOnly={readOnly.ToString().ToLower()}"));
        }

        return _dbContext;
    }
}

public enum DbOpenMode
{
    ReadOnly,
    ReadWrite
}
