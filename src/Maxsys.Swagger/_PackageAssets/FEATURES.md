# Maxsys.Swagger
Biblioteca Maxsys para configuração do Swagger/OpenAPI (Swashbuckle.AspNetCore): filtros para enums, parâmetros `[FromJson]`, identificador de endpoint e documentação XML. Requer .NET 10 e complementa o pacote `Maxsys.Web`.

## Filters

### ActionIdentifierOperationFilter

`IOperationFilter` que prefixa o summary da operação com o `Title` do `ActionIdentifierAttribute` (de `Maxsys.Web.Attributes`), quando o endpoint possuir o atributo. Se já houver summary (XML doc), o resultado fica no formato `TITLE | summary`.

```csharp
// Na Action:
[HttpGet]
[ActionIdentifier("CONTACT.LIST")]
public async Task<IActionResult> ListAsync(...) { ... }

// No Swagger:
services.AddSwaggerGen(c =>
{
    c.OperationFilter<ActionIdentifierOperationFilter>();
});
// Summary exibido: "CONTACT.LIST | Lista os contatos."
```

### EnumItemsDescriptionSchemaFilter

`ISchemaFilter` que, quando o schema é um enum, acrescenta à description os detalhes dos literais, um por linha, no formato `{valor} - {literal} ({Description do atributo, quando houver})`.

```csharp
public enum SampleEnum : byte
{
    [Description("Este é o tipo A.")]
    TipoA = 1,

    [Description("Este é o tipo B.")]
    TipoB,

    // Sem description
    TipoC = 99
}

// Description gerada no schema:
// Valores possíveis:
// 1 - TipoA (Este é o tipo A.)
// 2 - TipoB (Este é o tipo B.)
// 99 - TipoC
```

```csharp
services.AddSwaggerGen(c =>
{
    c.SchemaFilter<EnumItemsDescriptionSchemaFilter>();
});
```

### EnumParameterFilter

`IParameterFilter` equivalente ao filtro acima, mas aplicado a parâmetros de endpoint do tipo enum: a description do parâmetro recebe o link para o schema do enum e a lista de valores possíveis no mesmo formato `{valor} - {literal} ({description})`.

```csharp
services.AddSwaggerGen(c =>
{
    c.ParameterFilter<EnumParameterFilter>();
});
```

### FromJsonParameterFilter

`IParameterFilter` que, quando o parâmetro do endpoint possui o atributo `[FromJson]` (de `Maxsys.Web`), ajusta a documentação do parâmetro: a description passa a indicar que se espera uma string JSON do tipo de destino (com link para o schema) e o nome exibido recebe o tipo — ex.: `filter (ContactFilter)`.

```csharp
// Na Action:
[HttpGet]
public async Task<IActionResult> ListAsync([FromJson] ContactFilter filter, ...) { ... }

// No Swagger:
services.AddSwaggerGen(c =>
{
    c.ParameterFilter<FromJsonParameterFilter>();
});
```

### RemoveXmlDocListSchemaFilter

`ISchemaFilter` que remove a tag `<list>` (e seu conteúdo) da description dos schemas — útil quando o XML doc dos DTOs usa `<list>` para tabelas/listas que não renderizam bem no Swagger UI.

```csharp
services.AddSwaggerGen(c =>
{
    c.SchemaFilter<RemoveXmlDocListSchemaFilter>();
});
```

## Extensions

### MaxsysSwaggerGenOptionsExtensions

- `SwaggerGenOptions.IncludeXmlComments<TEntry>()` — obtém o assembly do tipo `TEntry` e inclui sua documentação XML, caso o arquivo exista, usando a convention padrão `[baseDir]/[assemblyName].xml`. Ideal com as interfaces de entry point dos pacotes (`ICoreEntry`, `IWebEntry`, `ISwaggerEntry`, ...) ou qualquer tipo do assembly desejado.

```csharp
services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments<Maxsys.Core.ICoreEntry>();   // XML doc do Maxsys.Core
    c.IncludeXmlComments<Maxsys.Web.IWebEntry>(); // XML doc do Maxsys.Web
    c.IncludeXmlComments<Program>();                  // XML doc da própria API
});
```

## Configuração completa

Exemplo de configuração do Swagger com todos os recursos do pacote:

```csharp
using Maxsys.Swagger.Extensions;
using Maxsys.Swagger.Filters;
using Microsoft.OpenApi.Models;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Documentação XML (requer <GenerateDocumentationFile> nos projetos)
            c.IncludeXmlComments<Maxsys.Core.ICoreEntry>();
            c.IncludeXmlComments<Maxsys.Web.IWebEntry>();
            c.IncludeXmlComments<Program>();

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sample Web API",
                Version = "v1",
                Description = "WebAPI de exemplo."
            });

            // Filters do Maxsys.Swagger
            c.SchemaFilter<RemoveXmlDocListSchemaFilter>();
            c.SchemaFilter<EnumItemsDescriptionSchemaFilter>();

            c.ParameterFilter<FromJsonParameterFilter>();
            c.ParameterFilter<EnumParameterFilter>();

            c.OperationFilter<ActionIdentifierOperationFilter>();
        });

        return services;
    }
}
```

Dica: para que os enums apareçam como literais (e não números) nas respostas, configure o JSON da API com `ConfigureJsonOptions()` do pacote `Maxsys.Web`.

## Entry point do assembly

### ISwaggerEntry

Interface vazia usada como referência ao assembly — por exemplo, em `IncludeXmlComments<ISwaggerEntry>()` ou em scans de assembly.
