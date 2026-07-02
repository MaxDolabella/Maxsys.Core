# Maxsys.Swagger

## 17.0.0
* :warning: Pacote renomeado de `Maxsys.Core.Web.Swagger` para `Maxsys.Swagger` (namespace raiz `Maxsys.Swagger`);
* :warning: Interface de entry point renomeada de `ICoreSwaggerEntry` para `ISwaggerEntry`;
* :warning: Atualização de framework (`.NET 10`);
* :package: Dependência trocada de `Swashbuckle.AspNetCore.Annotations` para `Swashbuckle.AspNetCore` (10.x);
* :sparkles: `ActionIdentifierOperationFilter` — prefixa o summary do endpoint com o `Title` do `ActionIdentifierAttribute` (substitui `IdentifiedEndpointOperationFilter`);
* :hammer_and_wrench: Filtros revisados para a nova API do Swashbuckle/Microsoft.OpenApi (`IOpenApiSchema`/`IOpenApiParameter`): `EnumItemsDescriptionSchemaFilter`, `EnumParameterFilter`, `FromJsonParameterFilter` e `RemoveXmlDocListSchemaFilter` (antes `RemoveXMLDocListSchemaFilter`);
* :recycle: Mantido `IncludeXmlComments<TEntry>()` (agora em `MaxsysSwaggerGenOptionsExtensions`);

---
## 16.0.0
* :warning: Atualização de pacotes;
* :sparkles: Adicionado IdentifiedEndpointOperationFilter;
* :triangular_flag_on_post: Removido TitledEndpointOperationFilter

---
## 14.0.0
* :warning: Atualização de dependências;

---
## 13.0.0
* Primeiro release.


<style>
  .warning { color: DarkGoldenRod; }
  h1 { color: Snow; }
  h2 { color: Crimson; }
  h3 { color: SteelBlue; }
  h4 { color: SeaGreen; }
</style>
