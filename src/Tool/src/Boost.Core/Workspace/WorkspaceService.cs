using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boost.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;

namespace Boost.Workspace
{
    public class WorkspaceService : IWorkspaceService
    {
        public WorkspaceService(
            IWebShellFactory webShellFactory,
            IBoostApplicationContext applicationContext,
            IEnumerable<IFileContentTypeHandler> fileHandlers)
        {
            _webShellFactory = webShellFactory;
            _applicationContext = applicationContext;
            _fileHandlers = fileHandlers.OrderBy(x => x.Order);
            _contentTypeProvider = new FileExtensionContentTypeProvider();
            _contentTypeProvider.Mappings.Add(".dll", "boost/dll");
        }

        public IEnumerable<FileSystemItem> GetFileSystemItems(string directory)
        {
            var root = new DirectoryInfo(directory);

            foreach (DirectoryInfo dir in root.GetDirectories())
            {
                yield return new FileSystemItem(dir.Name, dir.FullName, FileSystemType.Directory);
            }

            foreach (FileInfo file in root.GetFiles())
            {
                yield return new FileSystemItem(file.Name, file.FullName, FileSystemType.File);
            }
        }

        public WorkspaceContext GetWorkspace(string? directory = null)
        {
            directory = directory ?? _applicationContext.WorkingDirectory.FullName;

            var workplace = new WorkspaceContext
            {
                CurrentDirectory = directory,
                Files = GetFileSystemItems(directory)
            };

            return workplace;
        }

        public async Task<WorkspaceFile> GetFileAsync(
            GetFileRequest request,
            CancellationToken cancellationToken)
        {
            var file = new FileInfo(request.FileName);

            string contentType;

            if (!_contentTypeProvider.TryGetContentType(file.Name, out contentType))
            {
                contentType = "application/octet-stream";
            }

            FileEditorInfo meta = GetEditorInfo(file);

            var wsFile = new WorkspaceFile(
                file.Name,
                file.FullName,
                contentType)
            {
                Meta = meta
            };

            var options = new HandleFileOptions(request.Converter);

            foreach (IFileContentTypeHandler handler in _fileHandlers)
            {
                if (handler.CanHandle(wsFile, options))
                {
                    await handler.HandleAsync(wsFile, options, cancellationToken);
                    break;
                }
            }

            return wsFile;
        }

        public async Task<int> ExecuteFileActionAsync(string fileName, string action)
        {
            FileInfo? file = new FileInfo(fileName);

            switch (action)
            {
                case "Execute":
                    IWebShell shell = _webShellFactory.CreateShell(GetShellByFile(file));

                    return await shell.ExecuteAsync(new ShellCommand($".{Path.DirectorySeparatorChar}{file.Name}")
                    {
                        WorkDirectory = file.Directory!.FullName
                    });
                case "Open":
                    ProcessHelpers.Open(file.FullName);
                    break;
            }

            return 0;
        }

        public IEnumerable<QuickAction> GetQuickActions(string? directory)
        {
            directory = directory ?? _applicationContext.WorkingDirectory.FullName;
            var actions = new List<QuickAction>();

            if (Directory.Exists(directory))
            {
                actions.Add(new QuickAction
                {
                    Type = "CODE_DIRECTORY",
                    Title = directory,
                    Value = directory,
                    Description = "Open directory"
                });

                actions.AddRange(GetVisualStudioSolutions(directory));
            }

            return actions;
        }

        private IEnumerable<QuickAction> GetVisualStudioSolutions(string directory)
        {
            foreach (FileInfo file in new DirectoryInfo(directory)
                .GetFilesByExtensions(".slnf", ".sln"))
            {
                yield return new QuickAction
                {
                    Type = "VS_SOLUTION",
                    Title = file.Name,
                    Description = file.Directory?.Name,
                    Value = file.FullName
                };
            }
        }

        public Task<int> OpenInCode(string directory)
        {
            IWebShell shell = _webShellFactory.CreateShell();
            var cmd = new ShellCommand("code")
            {
                Arguments = ".",
                WorkDirectory = directory
            };

            return shell.ExecuteAsync(cmd);
        }

        public Task<int> OpenFileInCode(string fileName)
        {
            IWebShell shell = _webShellFactory.CreateShell();
            var cmd = new ShellCommand("code")
            {
                Arguments = fileName,
            };

            return shell.ExecuteAsync(cmd);
        }

        public Task<int> OpenFile(string fileName)
        {
            IWebShell shell = _webShellFactory.CreateShell();
            var cmd = new ShellCommand(fileName);

            return shell.ExecuteAsync(cmd);
        }

        private string GetShellByFile(FileInfo file)
        {
            var extension = file.Extension.Replace(".", "").ToLower();

            if (_shellMapping.ContainsKey(extension))
            {
                return _shellMapping[extension];
            }

            return extension;
        }

        public async Task<WorkspaceFile> CreateFileFromBase64Async(
            string value,
            string? fileType,
            CancellationToken cancellationToken)
        {
            fileType = fileType ?? "txt";
            fileType = fileType.Replace(".", "");

            var data = Convert.FromBase64String(value);

            var path = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid():N}.{fileType}");

            await File.WriteAllBytesAsync(path, data, cancellationToken);

            return await GetFileAsync(new GetFileRequest(path), cancellationToken);
        }

        private static Dictionary<string, string> _shellMapping = new()
        {
            ["ps1"] = "pwsh"
        };

        private static Dictionary<string, FileEditorInfo> _languageMapping = new()
        {
            ["cs"] = new FileEditorInfo("csharp"),
            ["csproj"] = new FileEditorInfo("xml"),
            ["props"] = new FileEditorInfo("xml"),
            ["yml"] = new FileEditorInfo("yaml"),
            ["ps1"] = new FileEditorInfo("powershell") { Actions = new List<string> { "Execute" } },
            ["cmd"] = new FileEditorInfo("powershell") { Actions = new List<string> { "Execute" } },
            ["js"] = new FileEditorInfo("javascript"),
            ["ts"] = new FileEditorInfo("typescript"),
            ["md"] = new FileEditorInfo("markdown"),
            ["sln"] = new FileEditorInfo("xml") { Actions = new List<string> { "Open" } },
        };
        private readonly IWebShellFactory _webShellFactory;
        private readonly IBoostApplicationContext _applicationContext;
        private readonly IEnumerable<IFileContentTypeHandler> _fileHandlers;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider;

        private FileEditorInfo GetEditorInfo(FileInfo file)
        {
            var extension = file.Extension.Replace(".", "").ToLower();

            FileEditorInfo editorInfo = new FileEditorInfo(extension);

            if (_languageMapping.ContainsKey(extension))
            {
                editorInfo = _languageMapping[extension];
            }

            editorInfo.Id = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(file.FullName));

            return editorInfo;
        }
    }
}
