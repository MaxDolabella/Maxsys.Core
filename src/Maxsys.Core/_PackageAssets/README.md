<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Core</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Core** é uma biblioteca desenvolvida em C# contendo os itens básicos para a criação de aplicações Maxsys.

Esse pacote contém as abstrações e classes base do ecossistema: `Entity`, `IRepository`, `IService`/`ModelServiceBase`, o resultado de operação `OperationResult`/`OperationResult<T>` (com a *factory* `Result`), filtragem dinâmica via `ColumnFilter`, critérios de listagem (`ListCriteria`, `Pagination`), ordenação, além de *helpers* e *extensions* (geração de Guid sequencial, criptografia, reflexão, etc.).

É a dependência transitiva de todos os demais pacotes Maxsys (`Maxsys.Data`, `Maxsys.Web`, `Maxsys.Excel`, `Maxsys.Swagger`, `Maxsys.Messaging`, `Maxsys.EventSourcing`).

## :dart: Target
`.NET 10`

## :package: Nuget
![Nuget](https://img.shields.io/nuget/v/Maxsys.Core)

```xml
    <PackageReference Include="Maxsys.Core" Version="17.0.0" />
```

## :link: Dependências

![FluentValidation.DependencyInjectionExtensions](https://img.shields.io/badge/FluentValidation.DependencyInjectionExtensions-12.1.1-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FFluentValidation.DependencyInjectionExtensions)  
![Microsoft.Extensions.Caching.Memory](https://img.shields.io/badge/Microsoft.Extensions.Caching.Memory-10.0.7-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Caching.Memory)  
![Microsoft.Extensions.Configuration.Binder](https://img.shields.io/badge/Microsoft.Extensions.Configuration.Binder-10.0.7-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Configuration.Binder)  
![Microsoft.Extensions.DependencyInjection.Abstractions](https://img.shields.io/badge/Microsoft.Extensions.DependencyInjection.Abstractions-10.0.7-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.DependencyInjection.Abstractions)  
![Microsoft.Extensions.Hosting.Abstractions](https://img.shields.io/badge/Microsoft.Extensions.Hosting.Abstractions-10.0.7-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Hosting.Abstractions)  
![Microsoft.Extensions.Http](https://img.shields.io/badge/Microsoft.Extensions.Http-10.0.7-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Http)  
![Microsoft.Extensions.Logging.Abstractions](https://img.shields.io/badge/Microsoft.Extensions.Logging.Abstractions-10.0.7-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Logging.Abstractions)  
![Microsoft.Extensions.Options.ConfigurationExtensions](https://img.shields.io/badge/Microsoft.Extensions.Options.ConfigurationExtensions-10.0.7-blue?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMicrosoft.Extensions.Options.ConfigurationExtensions)  

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
