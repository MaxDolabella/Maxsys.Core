using Microsoft.AspNetCore.StaticFiles;

namespace Maxsys.Core.Web.Extensions;

public static class MimeTypeExtensions
{
    public static string GetMimeTypeForFileExtension(this string filePath)
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