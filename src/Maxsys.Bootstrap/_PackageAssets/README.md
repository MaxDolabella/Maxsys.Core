<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Bootstrap</h1>
</div>

![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)

**Maxsys.Bootstrap** oferece componentes **Bootstrap 5.3** para projetos *ASP.NET Core MVC* em duas abordagens complementares: **TagHelpers** (`<bs-*>`, conteúdo projetado na view — Accordion, Alert, Button, Card, Modal, Table, Toast e muitos outros) e **ViewComponents** (`<vc:bs-*>`, data-driven — Pagination, Breadcrumb e Carousel gerados a partir de um modelo). A biblioteca emite apenas markup/`data-bs-*` — o Bootstrap (CSS/JS) é referenciado pela própria aplicação.

## :dart: Target
`.NET 10`

## :package: Nuget
![Nuget](https://img.shields.io/nuget/v/Maxsys.Bootstrap)

```xml
    <PackageReference Include="Maxsys.Bootstrap" Version="0.1.0" />
```

## :wrench: Utilização

TagHelpers — registre no `_ViewImports.cshtml`:

```cshtml
@addTagHelper *, Maxsys.Bootstrap
```

ViewComponents — registre no `Program.cs`:

```csharp
builder.Services.AddControllersWithViews().AddMaxsysBootstrap();
```

Exemplo:

```html
<bs-button variant="Success" type="Submit" icon="CheckLg">Salvar</bs-button>

<vc:bs-pagination current-page="Model.Page" total-pages="Model.TotalPages" url-format="?page={0}" />
```

## :link: Dependências
![Microsoft.AspNetCore.App](https://img.shields.io/badge/Microsoft.AspNetCore.App-Framework-red?style=for-the-badge)

## :star2: Features
As principais *features* do projeto podem ser vistas [aqui](FEATURES.md).

## :black_nib: Autores
[@MaxDolabella](https://www.github.com/MaxDolabella)

## :monocle_face: Aprendizagem
Através desse projeto, tenho a oportunidade de por em prática parte do conhecimento adquirido. Obviamente, ainda é limitado, mas a intenção é sempre buscar a melhora.

## :old_key: Licença
Este código possui licença MIT e está liberado para uso da maneira que se desejar.

## :email: Feedback
Quaisquer sugestões ou outro contato, escreva-me nesse [e-mail](mailto:maxsystech@outlook.com?subject=Github%20contact).

## :new: Release Notes
Os *changelogs* do projeto podem ser vistos [aqui](CHANGELOGS.md)
