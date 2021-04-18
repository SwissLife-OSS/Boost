using System.Collections.Generic;

namespace Boost.Workspace
{
    public record FileSystemItem(string Name, string Path, FileSystemType Type);

    public class WorkspaceFile
    {
        public WorkspaceFile(string name, string path, string contentType)
        {
            Name = name;
            Path = path;
            ContentType = contentType;
            Editor = "Code";
        }

        public string Name { get; set; }

        public string Path { get; set; }
        public string ContentType { get; set; }

        public string? Content { get; set; }

        public string Editor { get; set; }

        public FileEditorInfo Meta { get; set; } = default!;
    }

    public class FileEditorInfo
    {
        public FileEditorInfo(string laguage)
        {
            Language = laguage;
        }
 
        public string Language { get; set; }

        public string Id { get; set; } = default!;

        public IList<string> Actions { get; set; } = new List<string>();

        public IList<string> Converters { get; set; } = new List<string>();

        public string? GetUrl { get; set; }


    };
}
