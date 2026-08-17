<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Drawing</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Drawing** contém utilitários para manipulação de imagens via `System.Drawing`.

Fornece o `ImageHelper` com conversão imagem ↔ `byte[]` e gravação de arquivos `.jpg` (síncrona e assíncrona).

> :warning: **Windows-only.** `System.Drawing.Common` é suportado apenas no Windows a partir do .NET 6+. Por isso essa funcionalidade foi isolada neste pacote, fora do `Maxsys.Core`.

## :package: Nuget
![Nuget](https://img.shields.io/nuget/v/Maxsys.Drawing)

```xml
    <PackageReference Include="Maxsys.Drawing" Version="17.0.0" />
```

## :link: Dependências

- `System.Drawing.Common`
- `FluentValidation`

## :dart: Target
`.NET 10` (Windows)
