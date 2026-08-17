# Maxsys.Drawing

Biblioteca de manipulação de imagens via `System.Drawing.Common` — conversão entre `Image` e `byte[]` e gravação em arquivo JPG (síncrona e assíncrona). **Windows-only**: `System.Drawing.Common` só é suportado em Windows no .NET 10.

## Imagens

### ImageHelper

Classe estática com métodos para conversão e gravação de imagens. Os métodos de gravação retornam `ValidationResult` (FluentValidation) em vez de lançar exceção: falhas de IO viram `ValidationFailure` no resultado — verifique `IsValid` após a chamada.

+ `byte[] ImageToBytes(this Image image)`: converte um `Image` em array de bytes, preservando o formato original (`RawFormat`). Método de extensão.
+ `Image ImageFromBytes(byte[] rawImage)`: converte um array de bytes de volta em `Image`.
+ `ValidationResult SaveByteArrayImageIntoJpgFile(byte[] imageBytes, string targetFile)`: grava os bytes da imagem em arquivo, criando o diretório de destino se necessário. Sobrescreve arquivo existente.
+ `ValidationResult SaveImageIntoJpgFile(Image image, string targetFile)`: converte o `Image` em bytes e grava em arquivo. Sobrescreve arquivo existente.
+ `Task<ValidationResult> SaveByteArrayImageIntoJpgFileAsync(byte[] imageBytes, string targetFile)`: versão assíncrona da gravação de bytes.
+ `Task<ValidationResult> SaveImageIntoJpgFileAsync(Image image, string targetFile)`: versão assíncrona da gravação de `Image`.

Conversão entre `Image` e `byte[]`:

```csharp
using System.Drawing;
using Maxsys.Drawing;

// Image -> byte[] (ex.: para persistir em coluna binária)
using var image = Image.FromFile(@"C:\images\photo.png");
byte[] bytes = image.ImageToBytes();

// byte[] -> Image (ex.: bytes vindos do banco)
using var restored = ImageHelper.ImageFromBytes(bytes);
```

Gravação em arquivo com retorno `ValidationResult`:

```csharp
using FluentValidation.Results;

// Assíncrono, a partir de byte[]
ValidationResult result = await ImageHelper.SaveByteArrayImageIntoJpgFileAsync(
    bytes, @"C:\output\covers\album.jpg");

if (!result.IsValid)
{
    // Falhas de IO viram ValidationFailure (ErrorMessage + ErrorCode com a exception)
    foreach (var failure in result.Errors)
        logger.LogError("{Message}", failure.ErrorMessage);
}

// Assíncrono, a partir de Image
using var image = Image.FromFile(@"C:\images\photo.png");
var saveResult = await ImageHelper.SaveImageIntoJpgFileAsync(image, @"C:\output\photo.jpg");

// Versões síncronas equivalentes:
// ImageHelper.SaveByteArrayImageIntoJpgFile(bytes, targetFile);
// ImageHelper.SaveImageIntoJpgFile(image, targetFile);
```
