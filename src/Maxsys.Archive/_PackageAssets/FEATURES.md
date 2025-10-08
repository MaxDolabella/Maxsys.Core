# Maxsys.Archive

Uma biblioteca para criação e leitura de pacotes de arquivos com metadados e anexos.

## 📋 Visão Geral

O Maxsys.Archive permite empacotar dados estruturados (JSON) junto com arquivos anexos em um único arquivo compactado. Ideal para cenários onde você precisa armazenar ou transmitir dados com seus arquivos relacionados.

## 📦 Como Usar

### Estruturas Básicas de exemplo

```csharp
// Representa um arquivo anexo para o exemplo
public record AttachmentFile(string Name, byte[] Content);

// Helper de Manipulação de Arquivos
public static class ArchiveTestHelper
{
    // Cria e salva um pacote com conteúdo e anexos
    internal static string SaveFile(object contents, IEnumerable<AttachmentFile> attachments)
    {
        var outputFile = Path.GetTempFileName();
        
        // Criar pacote em memória
        var pkg = new Package
        {
            Meta = new { Author = "Max", CreatedAt = DateTime.UtcNow },
            Contents = contents ?? new { }
        };
        
        // Adicionar anexos
        foreach (var file in attachments)
        {
            pkg.AttachFile(file.Name, file.Content);
        }
        
        // Salvar em disco
        PackageWriter.Write(outputFile, pkg);
        
        return outputFile;
    }
    
    // Lê um pacote de um stream e extrai suas informações
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
```

### API Controller

```csharp
[ApiController]
[Route("[controller]")]
public class ArchiveController : ControllerBase
{
    /// <summary>
    /// Cria um pacote arquivo a partir de dados JSON e arquivos
    /// </summary>
    /// <param name="data">String JSON com os dados principais</param>
    /// <param name="files">Coleção de arquivos a serem anexados</param>
    /// <returns>Arquivo ZIP para download</returns>
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
    
    /// <summary>
    /// Lê e extrai informações de um pacote arquivo
    /// </summary>
    /// <param name="file">Arquivo de pacote a ser lido</param>
    /// <returns>JSON com versão, metadados, conteúdo e lista de anexos</returns>
    [HttpPost("read")]
    public IActionResult ReadFile(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var outputFile = ArchiveTestHelper.LoadFile(stream);
        
        return Ok(outputFile);
    }
    
    /// <summary>
    /// Converte IFormFile em AttachmentFile
    /// </summary>
    [NonAction]
    static AttachmentFile FormFileToAttachmentFile(IFormFile formFile)
    {
        if (!(formFile?.Length > 0))
            throw new ArgumentException("FormFile não pode ser nulo ou vazio.", nameof(formFile));
        
        byte[] fileBytes;
        using (var ms = new MemoryStream())
        {
            formFile.CopyTo(ms);
            fileBytes = ms.ToArray();
        }
        
        return new AttachmentFile(formFile.FileName, fileBytes);
    }
}
```

## 🔧 Exemplo de Uso

### Criando um Pacote

**Request (Multipart/Form-Data):**
```
POST /archive/create
Content-Type: multipart/form-data

data: {"titulo": "Meu Documento", "descricao": "Exemplo"}
files: documento.pdf
files: imagem.jpg
```

**Response:**
- Arquivo ZIP para download contendo os dados e anexos

### Lendo um Pacote

**Request (Multipart/Form-Data):**
```
POST /archive/read
Content-Type: multipart/form-data

file: meu-pacote.zip
```

**Response (JSON):**
```json
{
  "version": "1.0.0",
  "meta": {
    "author": "Max",
    "createdAt": "2025-10-08T10:30:00Z"
  },
  "contents": {
    "titulo": "Meu Documento",
    "descricao": "Exemplo"
  },
  "attachments": [
    "documento.pdf",
    "imagem.jpg"
  ]
}
```

## 📚 Estrutura do Pacote

Um pacote Maxsys.Archive contém:

- **Version**: Versão da aplicação pacote
- **Meta**: Metadados customizáveis (autor, data, etc.)
- **Contents**: Dados principais em formato objeto
- **Files**: Lista de arquivos anexados