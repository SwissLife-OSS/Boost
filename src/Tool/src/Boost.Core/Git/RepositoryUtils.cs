using System.Text;
using LibGit2Sharp;
using Microsoft.AspNetCore.WebUtilities;

namespace Boost.Git;

internal static class RepositoryUtils
{
    internal static string EncodeId(string path)
         => WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(path));

    internal static string DecodeId(string id)
        => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(id));

    internal static Repository? TryGetRepository(string path)
    {
        try
        {
            var repo = new Repository(path);

            return repo;
        }
        catch (RepositoryNotFoundException)
        {
            return null;
        }
        catch
        {
            throw;
        }
    }
}
