using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Boost.Workspace;

public static class FileSystemExtensions
{
    public static IEnumerable<FileInfo> GetFilesByExtensions(
        this DirectoryInfo dir,
        params string[] extensions)
    {
        if (extensions == null)
            throw new ArgumentNullException(nameof(extensions));

        IEnumerable<FileInfo> files = dir.EnumerateFiles("*.*", SearchOption.AllDirectories);
        return files.Where(f => extensions.Contains(
            f.Extension,
            StringComparer.InvariantCultureIgnoreCase));
    }
}
