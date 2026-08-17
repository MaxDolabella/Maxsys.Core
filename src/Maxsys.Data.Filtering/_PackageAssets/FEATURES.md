# Maxsys.Data.Filtering

Implementação **EF Core** dos filtros tipados (*specification*) de `Maxsys.Core.Filtering`. Fecha o ciclo: `Maxsys.Core.Filtering` define os contratos (`IFilter`, `FilterBase`, `IRepository<TEntity, TFilter>`, `ModelServiceBase` de aridade 4) e este pacote fornece o repositório concreto.

## Repositório

### RepositoryBase&lt;TEntity, TFilter&gt;
Namespace `Maxsys.Data` (mesmo do `RepositoryBase<TEntity>`, que ele estende). Implementa `IRepository<TEntity, TFilter>` onde `TFilter : IFilter<TEntity>`.

O coração é o overload protegido de `GetQueryable`:

```csharp
protected virtual async ValueTask<IQueryable<TEntity>> GetQueryable(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default)
{
    var query = await base.GetQueryable(predicate: null, @readonly, cancellationToken);

    filters.ApplyFilter(ref query); // ConfigureExpressions() + Where por expression

    return query;
}
```

Toda consulta do repositório parte dele — o filtro tipado é aplicado **antes** de projeção, criteria, ordenação e paginação.

+ **QTD** — `CountAsync(filter)`, `AnyAsync(filter)` e `CountAsync<TDestination>(filter, criteria)` (projeta e aplica `ColumnFilter`s + `Search` do `ListCriteria` **pós-projeção** — para o `Count` do `ListDTO` refletir o grid).
+ **LIST** — `ToListAsync` de `TEntity` ou `TDestination`; projeção via mapeador (`IQueryProjector`) ou por `Expression<Func<TEntity, TDestination>>`; combinável com `ListCriteria` ou `Pagination` + `sortKeySelector`/`SortDirection`.
+ **GET** — `GetAsync` (primeiro item) com overloads de `includeNavigation`, ordenação e projeção; `GetSingleOrDefaultAsync` (não lança) e `GetSingleOrThrowsAsync`.

### Projeção via chokepoints (IQueryProjector)
As projeções usam os mesmos `ApplyProjection` herdados de `RepositoryBase<TEntity>`:

+ `ApplyProjection<TDestination>(source)` → `IQueryProjector.Project` (ex.: `ProjectTo` do AutoMapper via `Maxsys.Mapping.AutoMapper`);
+ `ApplyProjection(source, projection)` → `Select(projection)`.

Subclasses que sobrescrevem os chokepoints (ex.: Field-Level Security) afetam também as consultas por filtro tipado — nenhum caminho de projeção escapa.

## Uso completo

```csharp
// Filtro (Maxsys.Core.Filtering)
public class ProductFilter : FilterBase<Guid, Product>
{
    public string? Name { get; set; }
    public PeriodFilter? CreatedAt { get; set; }

    public override void ConfigureExpressions()
    {
        if (!string.IsNullOrWhiteSpace(Name))
            AddExpression(p => p.Name.Contains(Name));

        if (CreatedAt?.MinValue is not null)
            AddExpression(p => p.CreatedAt >= CreatedAt.MinValue);
    }
}

// Repositório (este pacote)
public interface IProductRepository : IRepository<Product, ProductFilter>;

public class ProductRepository(AppDbContext context, IQueryProjector projector)
    : RepositoryBase<Product, ProductFilter>(context, projector), IProductRepository;

// Service (Maxsys.Core.Filtering)
public class ProductService(IProductRepository repository, IUnitOfWork uow, IObjectMapper mapper)
    : ModelServiceBase<Product, IProductRepository, Guid, ProductFilter>(repository, uow, mapper);

// Consumo
var page = await service.GetListAsync<ProductListDTO>(filter, criteria, ct);
var one  = await service.GetAsync<ProductDetailsDTO>(filter, ct);
```

### [README](README.md)
