# Maxsys.Core.Web.Swagger
Biblioteca Maxsys Core para utilização de recursos Swagger.

## SwaggerFeatures

### Filters
- `EnumItemsDescriptionSchemaFilter`: Adicionada descrição aos literais do enum (Schema).
- `EnumParameterFilter`: Adicionada descrição aos literais do enum (Parameter).
- `FromJsonParameterFilter`: Adicionada descrição ao parameter quando BinderModel for `FromJson`.
- `RemoveXMLDocListSchemaFilter`: Remove a tag &lt;list&gt; (e seu conteúdo) da description.
- `TitledEndpointOperationFilter`: Adiciona o title do endpoint ao summary, quando houver (obtido do atributo `TitledActionAttribute`).

### Extensions
- SwaggerGenOptionsExtensions
	- `IncludeXmlComments<TEntry>()`: Obtém o assembly ao qual pertence o tipo `TEntry` e inclui a documentação XML caso encontre o arquivo utilizando convention padrão: [baseDir]/[assemblyName].xml.

### Configuração Swagger

Para utilizar as features acima, recomenda-se criar um extension method para `ServiceCollectionExtensions`. Abaixo um exemplo:

```csharp
public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        // habilita os atributos contidos em Swashbuckle.AspNetCore.Annotations
        c.EnableAnnotations();

        // Adiciona a documentação xml (em código).
        c.IncludeXmlComments<Maxsys.Core.Entry>();
        c.IncludeXmlComments<Maxsys.Core.Data.Entry>();
        c.IncludeXmlComments<SomeNamespace.Entry>();
        c.IncludeXmlComments<Entry>();

        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Some Web API",
            Version = "v1",
            Description = "WebAPI para alguma coisa.",
            TermsOfService = new Uri("https://www.webapi.com.br/terms"),
            Contact = new OpenApiContact
            {
                Name = "Maxsys Desenvolvimento de Sistemas",
                Email = "max.dolabella@webapi.com.br",
                Url = new Uri("https://www.webapi.com.br"),
            },
            License = new OpenApiLicense
            {
                Name = "Licença",
                Url = new Uri("https://www.webapi.com.br/license"),
            }
        });

        c.AddServer(new OpenApiServer
        {
            Url = "https://localhost:1234",
            Description = "Server local para testes HTTP"
        });

        // Adiciona Filters personalizados
        c.SchemaFilter<RemoveXMLDocListSchemaFilter>();
        c.SchemaFilter<EnumItemsDescriptionSchemaFilter>();

        c.ParameterFilter<FromJsonParameterFilter>();
        c.ParameterFilter<EnumParameterFilter>();
        
        c.OperationFilter<TitledEndpointOperationFilter>();
    });

    return services;
}
```