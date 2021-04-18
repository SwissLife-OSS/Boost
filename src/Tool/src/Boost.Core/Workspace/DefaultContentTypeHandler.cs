using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Workspace
{
    public class DefaultContentTypeHandler : IFileContentTypeHandler
    {
        public int Order => int.MaxValue;

        public bool CanHandle(WorkspaceFile file, HandleFileOptions options)
        {
            return true;
        }

        public async Task HandleAsync(
            WorkspaceFile file,
            HandleFileOptions options,
            CancellationToken cancellationToken)
        {
            if (options.Converter is { })
            {
                switch (options.Converter)
                {
                    case "BASE64":
                        file.Content = await GetBase64(
                            file.Path,
                            Base64FormattingOptions.None,
                            cancellationToken);
                        break;
                    case "BASE64-ILB":
                        file.Content = await GetBase64(
                            file.Path,
                            Base64FormattingOptions.InsertLineBreaks,
                            cancellationToken);
                        break;
                    case "DATA_URL":
                        file.Content = await DataUrl.CreateAsync(
                            file.Path,
                            file.ContentType,
                            cancellationToken);

                        file.Meta.Converters.Add(options.Converter);
                        break;
                }
            }
            else
            {
                file.Content = await File.ReadAllTextAsync(
                    file.Path,
                    cancellationToken);
            }
        }

        private async Task<string> GetBase64(
            string filename,
            Base64FormattingOptions options,
            CancellationToken cancellationToken)
        {
            byte[] data = await File.ReadAllBytesAsync(
                        filename,
                        cancellationToken);

            return Convert.ToBase64String(
                data, options);
        }
    }
}
