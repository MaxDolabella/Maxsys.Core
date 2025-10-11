using System.Text.Json;
using Maxsys.Archive;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Tests.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ArchiveController : ControllerBase
{
    [HttpPost("create")]
    public IActionResult CreateFile([FromForm] string data, IFormFileCollection files)
    {
        // Converter arquivos do formulário
        var attachments = files.Select(FormFileToAttachmentFile).ToList();

        // Deserializar dados
        var contents = JsonSerializer.Deserialize<object>(data) ?? new { };

        // Criar pacote
        var tmpFilePath = ArchiveTestHelper.SaveFile(contents, attachments);

        try
        {
            // Preparar download
            var fileContents = System.IO.File.ReadAllBytes(tmpFilePath);
            var contentType = new FileExtensionContentTypeProvider()
                .TryGetContentType(tmpFilePath, out var contentTypeResult)
                ? contentTypeResult
                : "application/octet-stream";
            var fileDownloadName = Path.GetFileName(tmpFilePath).Replace(".tmp", ".zip");

            return File(fileContents, contentType, fileDownloadName);
        }
        finally
        {
            // Limpar arquivo temporário
            if (System.IO.File.Exists(tmpFilePath))
                System.IO.File.Delete(tmpFilePath);
        }
    }

    [HttpPost("read")]
    public IActionResult ReadFile(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        var outputFile = ArchiveTestHelper.LoadFile(stream);

        return Ok(outputFile);
    }

    [NonAction]
    private static AttachmentFile FormFileToAttachmentFile(IFormFile formFile)
    {
        ArgumentNullException.ThrowIfNull(formFile, nameof(formFile));

        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            formFile.CopyTo(ms);
            fileBytes = ms.ToArray();
        }

        return new AttachmentFile(formFile.FileName, fileBytes);
    }
}

public record AttachmentFile(string Name, byte[] Content);

public static class ArchiveTestHelper
{
    internal static string SaveFile(object contents, IEnumerable<AttachmentFile> attachments)
    {
        var outputFile = Path.GetTempFileName();

        // Criar pacote em memória
        var pkg = new Package
        {
            Meta = new { Author = "Max", CreatedAt = DateTime.UtcNow },
            Contents = contents ?? new { }
        };

        foreach (var file in attachments)
        {
            pkg.AttachFile(file.Name, file.Content);
        }

        // Salvar em disco
        PackageWriter.Write(outputFile, pkg);

        return outputFile;
    }

    internal static object? LoadFile(Stream stream)
    {
        var pkg = PackageReader.Read(stream);
        if (pkg is null)
        {
            return null;
        }

        var result = new
        {
            pkg.Version,
            pkg.Meta,
            pkg.Contents,
            Attachments = pkg.Files.Select(x => x.Name).ToList()
        };

        return result;
    }
}