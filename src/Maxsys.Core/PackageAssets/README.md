<div align="center">
<img src="logo.png" alt="drawing" width="256" />
<h1>Maxsys Core</h1>
</div>

![Nuget](https://img.shields.io/nuget/v/Maxsys.Core)
[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Core** é uma biblioteca desenvolvida em C# contendo itens básicos para criação de aplicações Maxsys.
O framework de destino utilizado é o `.NET 8.0`.

Esse pacote contém interfaces e classes bases como `IRepository`, `IService`, e toda uma infraestrutura como filtros, critérios de listagem, além de classes *helpers* e *extensions* como `GuidGen` para gerar Guids sequenciais e `ValidationResultExtensions` que possui métodos de extensão para a classe `FluentValidation.ValidationResult`.

Essa biblioteca auxilia no desenvolvimento de minhas aplicações pessoais `Windows Forms`, `WPF` e `ASP.NET Core (WebAPI, MVC)`.


## Nuget

```xml
    <PackageReference Include="Maxsys.Core" Version="10.0.0" />
```

## ⛓ Dependências

- AutoMapper.Extensions.Microsoft.DependencyInjection Version=12.0.1
- FluentValidation.DependencyInjectionExtensions Version=11.9.0
- Microsoft.Extensions.Caching.Abstractions Version=8.0.0
- Microsoft.Extensions.Configuration.Binder Version=8.0.1
- Microsoft.Extensions.DependencyInjection.Abstractions Version=8.0.0
- Microsoft.Extensions.Hosting.Abstractions Version=8.0.0
- Microsoft.Extensions.Logging.Abstractions Version=8.0.0
- Microsoft.Extensions.Options.ConfigurationExtensions Version=8.0.0
- System.Drawing.Common Version=8.0.1

## 🌟 Features

As principais *features* do projeto, podem ser vistas [aqui](features.md).

## ✒️ Autores

* [@MaxDolabella](https://www.github.com/MaxDolabella)

Aqui uma menção à [Jeremy H. Todd](https://github.com/jhtodd), autor de uma das features usadas nesse projeto (geração de guid sequencial).

## 🧐 Aprendizagem

Através desse projeto, tenho a oportunidade de por em prática parte do conhecimento adquirido. Obviamente, ainda é limitado, mas a intenção é sempre buscar a melhora.

## 🗝 Licença

Este código possui licença MIT e está liberado para uso da maneira que se desejar.
  
## 📧 Feedback

Quaisquer sugestões ou outro contato, escreva-me nesse [e-mail](mailto:maxsystech@outlook.com?subject=Github%20contact).

## 🆕 Release Notes

### 10.0.0

+ :warning: Projeto totalmente refatorado;

#### [Old Releases](old-releases.md)