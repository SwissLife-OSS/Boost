using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace Boost
{
    public class EmbeddedUIMiddleware
    {
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly IFileProvider _fileProvider;
        private readonly RequestDelegate _next;

        public EmbeddedUIMiddleware(
            RequestDelegate next)
        {
            _next = next;
            _fileProvider = CreateManifestFileProvider();
            _contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        private IFileProvider CreateManifestFileProvider()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                return new ManifestEmbeddedFileProvider(assembly, "UI");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "ManifestEmbeddedFileProvider Error");
                if (Debugger.IsAttached)
                {
                    return new PhysicalFileProvider(Directory.GetCurrentDirectory());
                }
                throw;
            }

        }

        public Task Invoke(HttpContext context)

        {
            var ct = _contentTypeProvider.TryGetContentType(context.Request.Path, out string contentType);

            var matchPath = context.Request.Path.Value;

            IFileInfo fileInfo = _fileProvider.GetFileInfo(matchPath);
            if (!fileInfo.Exists && !ct)
            {
                context.Request.ContentType = "text/html";
                fileInfo = _fileProvider.GetFileInfo("/index.html");
            }

            if (fileInfo.Exists)
            {
                return context.Response.SendFileAsync(fileInfo);
            }

            return _next(context);
        }
    }
}
