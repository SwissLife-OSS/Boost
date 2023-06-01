using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Boost.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;

namespace Boost.Workspace;

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
        FileInfo file = new(fileName);

        switch (action)
        {
            case "Execute":
                IWebShell shell = _webShellFactory.CreateShell(GetShellByFile(file));

                return await shell.ExecuteAsync(
                    $".{Path.DirectorySeparatorChar}{file.Name}",
                    null,
                    file.Directory!.Name);
            case "Open":
                ProcessHelpers.Open(file.FullName);
                break;
        }

        return 0;
    }

    public IEnumerable<WebLink> GetWebLinks(string? directory)
    {
        directory = directory ?? _applicationContext.WorkingDirectory.FullName;
        WorkspaceConfig? wsConfig = GetWorkspaceConfig(directory);

        return wsConfig?.WebLinks ?? Array.Empty<WebLink>();
    }

    public IEnumerable<QuickAction> GetQuickActions(string? directory)
    {
        directory = directory ?? _applicationContext.WorkingDirectory.FullName;
        var actions = new List<QuickAction>();

        if (Directory.Exists(directory))
        {
            WorkspaceConfig? wsConfig = GetWorkspaceConfig(directory);

            if (wsConfig is { })
            {
                foreach (WorkspaceSuperBoost superBoost in wsConfig.SuperBoosts)
                {
                    actions.Add(new QuickAction
                    {
                        Type = QuickActionTypes.RunSuperBoost,
                        Title = superBoost.Name,
                        Description = superBoost.Description,
                        Value = directory
                    });
                }
            }

            actions.Add(new QuickAction
            {
                Type = QuickActionTypes.OpenDirectoryInCode,
                Title = directory,
                Value = directory,
                Description = "Open directory in vscode"
            });
            actions.Add(new QuickAction
            {
                Type = QuickActionTypes.OpenDirectoryInExplorer,
                Title = directory,
                Value = directory,
                Description = "Open directory"
            });
            actions.Add(new QuickAction
            {
                Type = QuickActionTypes.OpenDirectoryInTerminal,
                Title = directory,
                Value = directory,
                Description = "Open in Terminal"
            });

            actions.AddRange(GetVisualStudioSolutions(directory));
            actions.AddRange(GetJavascriptProjects(directory));
        }

        return actions;
    }

    private IEnumerable<QuickAction> GetVisualStudioSolutions(string directory)
    {
        int nr = 0;

        foreach (FileInfo file in new DirectoryInfo(directory)
            .GetFilesByExtensions(".slnf", ".sln"))
        {
            if ( nr > 10)
            {
                break;
            }

            yield return new QuickAction
            {
                Type = QuickActionTypes.OpenVisualStudioSolution,
                Title = file.Name,
                Description = $"Open solution '{file.Name}'",
                Value = file.FullName
            };
        }
    }

    private IEnumerable<QuickAction> GetJavascriptProjects(string directory)
    {
        int nr = 0;

        foreach (string filename in
            Directory.EnumerateFiles(directory, "package.json", SearchOption.AllDirectories))
        {
            if (nr > 10)
            {
                break;
            }

            var file = new FileInfo(filename);

            if (file.Directory!.FullName is { } && !file.FullName.Contains("node_module"))
            {
                var directoryName = file.Directory!.FullName;

                yield return new QuickAction
                {
                    Type = QuickActionTypes.OpenDirectoryInCode,
                    Title = directoryName,
                    Description = $"Open js project in Code " +
                    $"'{directoryName.Split(Path.DirectorySeparatorChar).Last()}'",
                    Value = directoryName,
                    Tags = new[] { "js" }
                };
            }
        }
    }

    public Task<int> OpenInCode(string directory)
    {
        IWebShell shell = _webShellFactory.CreateShell();

        return shell.ExecuteAsync("code", ".", directory);
    }

    public Task<int> OpenFileInCode(string fileName)
    {
        IWebShell shell = _webShellFactory.CreateShell();

        return shell.ExecuteAsync("code", fileName, null);
    }

    public Task<int> OpenFile(string fileName)
    {
        IWebShell shell = _webShellFactory.CreateShell();

        return shell.ExecuteAsync(fileName, null, null);
    }

    public Task<int> OpenInExplorer(string directory)
    {
        IWebShell shell = _webShellFactory.CreateShell();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return shell.ExecuteAsync("ii", ".", directory);
        }
        else
        {
            return shell.ExecuteAsync("open", ".", directory);
        }
    }

    public async Task<int> OpenInTerminal(string directory)
    {
        IWebShell shell = _webShellFactory.CreateShell();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return await shell.ExecuteAsync("wt.exe", $"-w 0 nt -d {directory}", directory);
        }

        return 0;
    }

    public async Task<int> RunSuperBoostAsync(string name, string directory)
    {
        WorkspaceConfig? wsConfig = GetWorkspaceConfig(directory);

        WorkspaceSuperBoost? superBoost = wsConfig?.SuperBoosts
            .FirstOrDefault(x => x.Name == name);

        int resultCode = 0;

        if (superBoost is { })
        {
            foreach (SuperBoostAction? action in superBoost.Actions)
            {
                var path = BuildPath(action.Value);

                switch (action.Type)
                {
                    case QuickActionTypes.OpenDirectoryInCode:
                        resultCode += await OpenInCode(path);
                        break;
                    case QuickActionTypes.OpenVisualStudioSolution:
                        resultCode += await OpenFile(path);
                        break;
                    case QuickActionTypes.OpenDirectoryInExplorer:
                        resultCode += await OpenInExplorer(path);
                        break;
                    case QuickActionTypes.OpenDirectoryInTerminal:
                        resultCode += await OpenInTerminal(path);
                        break;
                }
            }

            string BuildPath(string? value)
            {
                if (value is { })
                {
                    return Path.Combine(
                        directory,
                        value.Replace('/', Path.DirectorySeparatorChar));
                }

                return directory;
            }
        }

        return resultCode;
    }

    private WorkspaceConfig? GetWorkspaceConfig(string directory)
    {
        var path = Path.Combine(directory, "boost.json");

        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<WorkspaceConfig>(json);
        }

        return null;
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
