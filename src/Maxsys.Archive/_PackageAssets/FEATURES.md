# Maxsys.Archive

## Como usar

```csharp
// Criar pacote em memória
var pkg = new Package
{
    Version = "1.0",
    Meta = new { Author = "Max", CreatedAt = DateTime.UtcNow },
    Contents = new { Title = "Relatório", Attachments = new[] { "FILES/foto.png" } }
};
pkg.Files.Add(new PackageFile("foto.png", File.ReadAllBytes("caminho/foto.png")));
pkg.Files.Add(new PackageFile("doc1.pdf", File.ReadAllBytes("caminho/doc1.pdf")));

// Salvar em disco
PackageWriter.Write("teste.zip", pkg);

// Ler de volta
var loaded = PackageReader.Read("teste.zip");
Console.WriteLine($"Versão: {loaded.Version}");
Console.WriteLine($"Meta: {JsonSerializer.Serialize(loaded.Meta)}");
Console.WriteLine($"Contents: {JsonSerializer.Serialize(loaded.Contents)}");

// Extrair arquivo
var foto = loaded.GetFile("foto.png");
if (foto != null)
    File.WriteAllBytes("foto_extraida.png", foto.Data);

```
#### [README](README.md)