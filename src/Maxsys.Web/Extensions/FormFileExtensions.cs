using System.Text;
using Microsoft.AspNetCore.Http;

namespace Maxsys.Web.Extensions;

public static partial class FormFileExtensions
{
    extension(IFormFile? formFile)
    {
        public async Task<byte[]> ToByteArrayAsync(CancellationToken cancellationToken = default)
        {
            if (!(formFile?.Length > 0))
                return [];

            using var ms = new MemoryStream();
            await formFile.CopyToAsync(ms, cancellationToken);
            return ms.ToArray();
        }

        public async Task<string> ToBase64StringAsync(CancellationToken cancellationToken = default)
        {
            if (!(formFile?.Length > 0))
                return string.Empty;

            return Convert.ToBase64String(await formFile.ToByteArrayAsync(cancellationToken));
        }

        public async Task<string> ReadContentAsync(CancellationToken cancellationToken = default)
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
}