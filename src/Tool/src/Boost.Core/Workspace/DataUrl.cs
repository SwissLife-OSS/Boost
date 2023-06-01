using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Workspace;

public static class DataUrl
{
    public static string Create(byte[] data, string contentType)
    {
        return $"data:{contentType};base64,{Convert.ToBase64String(data)}";
    }

    public static async Task<string> CreateAsync(
        string fileName,
        string contentType,
        CancellationToken cancellationToken)
    {
        var data = await File.ReadAllBytesAsync(fileName, cancellationToken);

        return Create(data, contentType);
    }
}
