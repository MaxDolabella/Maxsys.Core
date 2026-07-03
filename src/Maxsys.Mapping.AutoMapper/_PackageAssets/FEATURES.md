# Maxsys.Mapping.AutoMapper

Adaptador **AutoMapper** para as abstrações de mapeamento do `Maxsys.Core` (`IObjectMapper` e `IQueryProjector`), com auto-registro de `Profile`s por scan de assembly.

## Registro (DI)

### AddMaxsysAutoMapper
Registra o AutoMapper (scan de `Profile`s) **e** os adaptadores `IObjectMapper`/`IQueryProjector` consumidos por `ModelServiceBase` (`Maxsys.Core`) e `RepositoryBase`/`JoinRepositoryBase` (`Maxsys.Data`).

+ `AddMaxsysAutoMapper<TEntry>(configure?)` — scan no assembly do tipo âncora `TEntry`.
+ `AddMaxsysAutoMapper(params Assembly[] assemblies)` — scan em múltiplos assemblies.
+ `AddMaxsysAutoMapper(configure, params Assembly[] assemblies)` — com configuração adicional do AutoMapper.

```csharp
// Program.cs do consumidor
services.AddMaxsysAutoMapper<IApplicationEntry>();

// com configuração adicional + múltiplos assemblies
services.AddMaxsysAutoMapper(
    cfg => cfg.AllowNullCollections = true,
    typeof(IApplicationEntry).Assembly,
    typeof(IContractsEntry).Assembly);
```

Os `Profile`s continuam AutoMapper puro:

```csharp
public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDTO>();
        CreateMap<ProductCreateDTO, Product>();
    }
}
```

## Adaptador

### AutoMapperAdapter (interno)
Implementa as duas abstrações delegando ao `IMapper`:

+ `IObjectMapper.Map<TDestination>(object)` → `mapper.Map<TDestination>(source)` — usado no CRUD do `ModelServiceBase` (create).
+ `IObjectMapper.Map<TDestination>(object, Action<TDestination> afterMap)` → map + pós-processamento no momento da chamada (roda **após** o pipeline do AutoMapper, inclusive `AfterMap`s de Profile).
+ `IObjectMapper.Map<TSource, TDestination>(source, destination)` → `mapper.Map(source, destination)` — mapeamento *in-place* (update).
+ `IQueryProjector.Project<TDestination>(IQueryable)` → `source.ProjectTo<TDestination>(mapper.ConfigurationProvider)` — projeção composta na *expression tree* (EF Core traduz para SQL; nada é materializado).

## AfterMap / BeforeMap e recursos do Profile

O adaptador delega ao `IMapper` real — o pipeline completo do AutoMapper roda normalmente em `IObjectMapper.Map`:

+ `AfterMap`/`BeforeMap` configurados no `Profile` (inclusive `AfterMap<TMappingAction>()` com DI) — **funcionam**.
+ `ValueResolver`s, `TypeConverter`s, `Condition`s — **funcionam**.
+ `AfterMap` **não roda em projeção** (`IQueryProjector.Project`/`ProjectTo`): comportamento do próprio AutoMapper — *expression trees* não executam código arbitrário. Vale desde sempre.
+ `AfterMap` no momento do map: use `IObjectMapper.Map<TDestination>(source, afterMap)` — overload agnóstico de mapeador:

```csharp
var dto = _mapper.Map<ProductDTO>(entity, d => d.DisplayName = $"{d.Code} - {d.Name}");
```

+ Demais options por chamada do AutoMapper (`opts.Items`, `ConstructServicesUsing`...) não são expostas por `IObjectMapper` (vazariam tipos do AutoMapper na abstração). Onde precisar disso no aplicativo, injete `IMapper` diretamente — o app pode referenciar AutoMapper; a restrição vale só para `Maxsys.Core`/`Maxsys.Data`.

```csharp
public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateDTO, Product>()
            .AfterMap((src, dest) => dest.Slug = dest.Name.ToSlug()); // roda no ModelServiceBase.AddAsync
    }
}
```

## Trocando de mapeador

`Maxsys.Core`/`Maxsys.Data` conhecem apenas as interfaces. Para usar outro mapeador (ex.: Mapster) ou projeções manuais, basta implementar `IObjectMapper`/`IQueryProjector` e registrá-los no DI — nenhuma mudança nas libs ou nos seus services/repositories.

> :warning: `AutoMapper` está fixado em **14.0.0** (última versão gratuita). Este pacote é o único do ecossistema Maxsys que referencia AutoMapper.

### [README](README.md)
