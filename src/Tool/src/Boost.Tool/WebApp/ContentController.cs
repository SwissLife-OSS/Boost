using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;

namespace Boost.Tool.WebApp
{
    [Route("api/file/content")]
    public class FileContentController : Controller
    {
        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(string id)
        {
            return GetFileResult(id);
        }

        [Route("download/{id}")]
        [HttpGet]
        public IActionResult Download(string id)
        {
            PhysicalFileResult file = GetFileResult(id);
            file.FileDownloadName = Path.GetFileName(file.FileName);

            return file;
        }

        private PhysicalFileResult GetFileResult(string id)
        {
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            var fileName = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(id));

            string contentType;

            if (!contentTypeProvider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return new PhysicalFileResult(fileName, contentType);
        }
    }
}
