using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Boost
{
    public static class ProcessHelpers
    {
        public static Process? Open(string filename)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = filename;
                startInfo.UseShellExecute = true;

                return Process.Start(startInfo);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return Process.Start(new ProcessStartInfo("cmd", $"/c start {filename}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return Process.Start("xdg-open", filename);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return Process.Start("open", filename);
                }
                else
                {
                    throw;
                }
            }
        }

        public static Process OpenBrowser(string url)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = url;
                startInfo.UseShellExecute = true;

                return Process.Start(startInfo);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    return Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
