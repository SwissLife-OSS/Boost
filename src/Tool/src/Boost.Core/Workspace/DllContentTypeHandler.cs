using System;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using Serilog;

namespace Boost.Workspace
{
    public class DllContentTypeHandler : IFileContentTypeHandler
    {
        public int Order => 200;

        public bool CanHandle(WorkspaceFile file, HandleFileOptions options)
        {
            return file.ContentType.Equals("boost/dll");
        }

        public Task HandleAsync(WorkspaceFile file, HandleFileOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var decompiler = new CSharpDecompiler(
                    file.Path,
                    new DecompilerSettings());

                file.Content = decompiler.DecompileWholeModuleAsString();

                file.Meta.Language = "csharp";
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Could not decompile: {File}", file.Path);
            }


            return Task.CompletedTask;
        }
    }
}
