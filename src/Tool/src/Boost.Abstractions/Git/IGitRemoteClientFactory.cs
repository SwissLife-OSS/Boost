namespace Boost.Git;

public interface IGitRemoteClientFactory
{
    IGitRemoteClient Create(string serviceType);
}