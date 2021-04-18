using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boost.Workspace
{
    public interface IWorkspaceService
    {
        Task<int> ExecuteFileActionAsync(string fileName, string action);
        Task<WorkspaceFile> GetFileAsync(
            GetFileRequest request,
            CancellationToken cancellationToken);

        IEnumerable<FileSystemItem> GetFileSystemItems(string directory);
        IEnumerable<QuickAction> GetQuickActions(string? directory);
        WorkspaceContext GetWorkspace(string? directory = null);
        Task<int> OpenFileInCode(string fileName);
        Task<int> OpenInCode(string directory);
        Task<int> OpenFile(string fileName);
        Task<WorkspaceFile> CreateFileFromBase64Async(
            string value,
            string? fileType,
            CancellationToken cancellationToken);
    }
}
