using System.Text;
using Microsoft.AspNetCore.Http;

namespace Maxsys.Core.Web.Extensions;

public static class IFormFileExtensions
{
    public static byte[] ToByteArray(this IFormFile? formFile)
    {
        if (!(formFile?.Length > 0))
            return Array.Empty<byte>();

        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            formFile.CopyTo(ms);
            fileBytes = ms.ToArray();
        }

        return fileBytes;
    }

    public static string ToBase64String(this IFormFile? formFile)
    {
        if (!(formFile?.Length > 0))
            return string.Empty;

        return Convert.ToBase64String(formFile.ToByteArray());
    }

    public static async Task<string> ReadContentAsync(this IFormFile? formFile, CancellationToken cancellationToken = default)
    {
        if (!(formFile?.Length > 0))
            return string.Empty;

        var result = new StringBuilder();
        using (var reader = new StreamReader(formFile.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                result.AppendLine(await reader.ReadLineAsync(cancellationToken));
        }
        return result.ToString();
    }
}