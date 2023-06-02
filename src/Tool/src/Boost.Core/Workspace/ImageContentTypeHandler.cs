using System.Threading;
using System.Threading.Tasks;

namespace Boost.Workspace;

public class PdfContentTypeHandler : IFileContentTypeHandler
{
    public int Order => 110;

    public bool CanHandle(WorkspaceFile file, HandleFileOptions options)
    {
        return file.ContentType.Equals("application/pdf") && options.Converter is null;
    }

    public Task HandleAsync(WorkspaceFile file, HandleFileOptions options, CancellationToken cancellationToken)
    {
        file.Editor = "Pdf";
        file.Meta.GetUrl = $"/api/file/content/{file.Meta.Id}";

        return Task.CompletedTask;
    }
}


public class ImageContentTypeHandler : IFileContentTypeHandler
{
    public int Order => 100;

    public bool CanHandle(WorkspaceFile file, HandleFileOptions options)
    {
        return file.ContentType.StartsWith("image") && options.Converter is null;
    }

    public Task HandleAsync(
        WorkspaceFile file,
        HandleFileOptions options,
        CancellationToken cancellationToken)
    {
        file.Editor = "Image";
        file.Meta.GetUrl = $"/api/file/content/{file.Meta.Id}";
        file.Meta.Converters.Add("DATA_URL");

        return Task.CompletedTask;
    }
}
