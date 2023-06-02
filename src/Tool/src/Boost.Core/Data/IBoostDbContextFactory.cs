namespace Boost.Data;

public interface IBoostDbContextFactory
{
    IBoostDbContext Open(DbOpenMode mode);
}
