<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Core</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Core** é uma biblioteca desenvolvida em C# contendo itens básicos para criação de aplicações Maxsys.

Esse pacote contém interfaces e classes bases como `IRepository`, `IService`, e toda uma infraestrutura como filtros, critérios de listagem, além de classes *helpers* e *extensions* como `GuidGen` para gerar Guids sequenciais e `ValidationResultExtensions` que possui métodos de extensão para a classe `FluentValidation.ValidationResult`.

Essa biblioteca auxilia no desenvolvimento de minhas aplicações pessoais `Windows Forms`, `WPF` e `ASP.NET Core (WebAPI, MVC)`.


## :package: Nuget
![Nuget](https://img.shields.io/nuget/v/Maxsys.Core)

```xml
    <PackageReference Include="Maxsys.Core" Version="16.0.0" />
```

## :link: Dependências

![AutoMapper](https://img.shields.io/badge/AutoMapper-14.0.0-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FAutoMapper)  
![FluentValidation.DependencyInjectionExtensions](https://img.shields.io/badge/FluentValidation.DependencyInjectionExtensions-12.0.0-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FFluentValidation.DependencyInjectionExtensions)  
![MediatR](https://img.shields.io/badge/MediatR-12.5.0-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMediatR)  
![Microsoft.Extensions.Caching.Abstractions](https://img.shields.io/badge/Microsoft.Extensions.Caching.Abstractions-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Caching.Abstractions)  
![Microsoft.Extensions.Configuration.Binder](https://img.shields.io/badge/Microsoft.Extensions.Configuration.Binder-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Configuration.Binder)  
![Microsoft.Extensions.DependencyInjection.Abstractions](https://img.shields.io/badge/Microsoft.Extensions.DependencyInjection.Abstractions-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.DependencyInjection.Abstractions)  
![Microsoft.Extensions.Hosting.Abstractions](https://img.shields.io/badge/Microsoft.Extensions.Hosting.Abstractions-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Hosting.Abstractions)  
![Microsoft.Extensions.Http](https://img.shields.io/badge/Microsoft.Extensions.Http-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Http)  
![Microsoft.Extensions.Logging.Abstractions](https://img.shields.io/badge/Microsoft.Extensions.Logging.Abstractions-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Logging.Abstractions)  
![Microsoft.Extensions.Options.ConfigurationExtensions](https://img.shields.io/badge/Microsoft.Extensions.Options.ConfigurationExtensions-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Options.ConfigurationExtensions)  
![System.Drawing.Common](https://img.shields.io/badge/System.Drawing.Common-9.0.9-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FSystem.Drawing.Common)  


## :star2: Features
As principais *features* do projeto podem ser vistas [aqui](FEATURES.md).

## :black_nib: Autores
[@MaxDolabella](https://www.github.com/MaxDolabella)

Aqui uma menção à [Jeremy H. Todd](https://github.com/jhtodd), autor de uma das features usadas nesse projeto (geração de guid sequencial).

## :monocle_face: Aprendizagem
Através desse projeto, tenho a oportunidade de por em prática parte do conhecimento adquirido. Obviamente, ainda é limitado, mas a intenção é sempre buscar a melhora.

## :old_key: Licença
Este código possui licença MIT e está liberado para uso da maneira que se desejar.
  
## :email: Feedback
Quaisquer sugestões ou outro contato, escreva-me nesse [e-mail](mailto:maxsystech@outlook.com?subject=Github%20contact).

## :new: Release Notes
Os *changelogs* do projeto podem ser vistos [aqui](CHANGELOGS.md)

<style>
  .warning { color: DarkGoldenRod; }
  h1 { color: Snow; }
  h2 { color: Crimson; }
  h3 { color: SteelBlue; }
  h4 { color: SeaGreen; }
</style>