using Microsoft.AspNetCore.StaticFiles;

namespace Maxsys.Web.Extensions;

public static class MimeTypeExtensions
{
    extension(string filePath)
    {
        public string GetMimeTypeForFileExtension()
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string? contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }
    }
}