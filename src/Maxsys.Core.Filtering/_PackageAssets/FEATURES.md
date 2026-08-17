# Maxsys.Core.Filtering

Extensão do `Maxsys.Core` para **filtros tipados** (*specification*): o filtro é uma classe do contrato da API que se traduz em `Expression`s aplicadas à query, com variantes de repositório e service que consultam a partir dele.

> :bulb: Complementa (não substitui) a filtragem dinâmica via `ColumnFilter`/`ListCriteria` do `Maxsys.Core`. Os dois convivem: `TFilter` restringe o universo (regra de negócio) e `ListCriteria` pagina/ordena/refina (grid dinâmico).

## Contratos

### IFilter / IKeyFilter&lt;TKey&gt; / IFilter&lt;TEntity&gt;
Namespace `Maxsys.Core.Filtering`.

+ `IFilter` — interface de marcação para tipificar um filtro.
+ `IKeyFilter<TKey>` — filtro com `KeyList<TKey> IdList` (Include/Exclude de chaves).
+ `IFilter<TEntity>` — filtro executável: expõe `List<Expression<Func<TEntity, bool>>> Expressions` e `ApplyFilter(ref IQueryable<TEntity>)`.

## Bases de filtro

### FilterBase / FilterBase&lt;TKey&gt; / FilterBase&lt;TKey, TEntity&gt;
Hierarquia base para implementação de filtros:

+ `FilterBase` — traz `SearchTerm? Search` (busca textual) e `ActiveTypes ActiveType` (default `OnlyActives`).
+ `FilterBase<TKey>` — adiciona `KeyList<TKey> IdList`.
+ `FilterBase<TKey, TEntity>` — o filtro executável. Implemente `ConfigureExpressions()` traduzindo as props em expressions via `AddExpression()`; `ApplyFilter` chama `ConfigureExpressions` e aplica cada expression como `Where` na query.

```csharp
public class ProductFilter : FilterBase<Guid, Product>
{
    public string? Name { get; set; }
    public RangeFilter<decimal?>? Price { get; set; }
    public PeriodFilter? CreatedAt { get; set; }

    public override void ConfigureExpressions()
    {
        if (!string.IsNullOrWhiteSpace(Name))
            AddExpression(p => p.Name.Contains(Name));

        if (Price?.MinValue is not null)
            AddExpression(p => p.Price >= Price.MinValue);
        if (Price?.MaxValue is not null)
            AddExpression(p => p.Price <= Price.MaxValue);

        if (Search is not null)
            AddSearchExpression(p => new[] { p.Name, p.Code }); // Search.Term em qualquer um dos campos

        if (IdList.Count > 0)
            AddExpression(p => (!IdList.AnyInclude() || IdList.Include.Contains(p.Id))
                            && (!IdList.AnyExclude() || !IdList.Exclude.Contains(p.Id)));
    }
}
```

## Blocos de construção

### SearchTerm / SearchTermModes
Busca textual tipada: `Term` + `Mode` + `Column` (opcional, para busca direcionada).

+ `SearchTermModes.Any` — `Contains()` / `LIKE '%termo%'`.
+ `SearchTermModes.StartsWith` — `StartsWith()` / `LIKE 'termo%'`.
+ `SearchTermModes.EndsWith` — `EndsWith()` / `LIKE '%termo'`.
+ `ToExpression<T>(entityFieldArray)` — converte o termo em `Expression<Func<T, bool>>` sobre um array de campos string da entidade (via `ExpressionHelper.SearchTermToExpression`). Campos anotados com atributo de conversão são passados por `Convert.ToString` antes da comparação.

```csharp
var search = new SearchTerm("max", SearchTermModes.StartsWith);
Expression<Func<Product, bool>> expr = search.ToExpression<Product>(p => new[] { p.Name, p.Code });
```

### KeyList&lt;TKey&gt; / SearchKey&lt;TKey&gt; / SearchKeyModes
Lista de chaves com modo `Include`/`Exclude` por item — permite "traga estes, menos aqueles" num único membro.

+ Conversões implícitas: `List<TKey>` e `TKey` viram `KeyList<TKey>` (modo Include).
+ `Include`/`Exclude` — enumeram as chaves por modo; `AnyInclude()`/`AnyExclude()`; `AddItems(items, mode)`.

```csharp
KeyList<Guid> ids = new([id1, id2], itemsToExclude: [id3]);
filter.IdList = id1; // implícito: single include
```

### RangeFilter&lt;T&gt; / PeriodFilter / DateTimeOffsetFilter
Faixas `MinValue`/`MaxValue`:

+ `RangeFilter<T>` — faixa genérica (números, datas, etc.).
+ `PeriodFilter` (`RangeFilter<DateTime?>`) — período; o ctor com `uses00h00To23h59: true` normaliza as horas para `00:00:00`–`23:59:59.9999`.
+ `PeriodFilter<TDateTypeFilter>` — período + enum `DateType` indicando **qual** data filtrar (criação, alteração...).
+ `DateTimeOffsetFilter` — idem para `DateTimeOffset`.

### ActiveTypes
Enum para filtro de ativos/inativos: `OnlyActives` (default do `FilterBase`), `OnlyInactives`, `All`.

### FilterItem&lt;T&gt;
Wrapper `{ T? Value }` para distinguir "não filtrar" (item nulo) de "filtrar por null/default" (item presente com `Value` nulo).

## Repositório

### IRepository&lt;TEntity, TFilter&gt;
Namespace `Maxsys.Core.Interfaces.Repositories` (mesmo namespace do `IRepository<TEntity>` do Core, que ele estende). Consultas a partir do filtro tipado:

+ **GET** — `GetAsync(filter)` e overloads com `includeNavigation`, `sortSelector`/`SortDirection`, projeção via mapeamento (`GetAsync<TDestination>`) ou expression; `GetSingleOrDefaultAsync` (não lança) e `GetSingleOrThrowsAsync` (lança se houver mais de um).
+ **LIST** — `ToListAsync` de `TEntity` ou `TDestination` (mapeado ou por `projection`), combinável com `ListCriteria` ou `Pagination` + `sortSelector`.
+ **QTD** — `CountAsync(filter)`, `CountAsync<TDestination>(filter, criteria)` (ColumnFilters + Search pós-projeção) e `AnyAsync(filter)`.

> :warning: Este pacote define **apenas o contrato**. A implementação EF Core (`RepositoryBase<TEntity, TFilter>`, que aplica `filters.ApplyFilter(ref query)` na query base) vive em **`Maxsys.Data.Filtering`**; para outro provedor de dados, implemente `IRepository<TEntity, TFilter>` diretamente.

## Service

### IModelService&lt;TEntity, TKey, TFilter&gt; + ModelServiceBase&lt;TEntity, TRepository, TKey, TFilter&gt;
Namespaces `Maxsys.Core.Interfaces.Services` / `Maxsys.Core.Services`. Quarta variante da família `ModelServiceBase` (aridade 4): estende `ModelServiceBase<TEntity, TRepository, TKey>` (CRUD do Core) exigindo `TRepository : IRepository<TEntity, TFilter>` e delega as consultas por filtro ao repositório, mantendo os eventos de ciclo de vida (`OnGetCompletedAsync`, `OnToListCompletedAsync`, `OnGetListCompletedAsync`).

+ **GET** — `GetAsync<TDestination>(filter)`, com `projection`, `GetSingleOrDefaultAsync`, `GetSingleOrThrowsAsync`.
+ **LIST** — `ToListAsync<TDestination>` e `GetListAsync<TDestination>` (retorna `ListDTO` com `Count` + `Items`), combináveis com `ListCriteria`/`Pagination`; `ToInfoListAsync`/`GetInfoListAsync` para `InfoDTO<TKey>` (dropdowns).
+ **QTY** — `CountAsync(filter?)` e `AnyAsync(filter?)` (filtro nulo vira `new TFilter()`).

```csharp
public class ProductService(IProductRepository repository, IUnitOfWork uow, IObjectMapper mapper)
    : ModelServiceBase<Product, IProductRepository, Guid, ProductFilter>(repository, uow, mapper);

// uso
var page = await service.GetListAsync<ProductListDTO>(filter, criteria, ct);
var combo = await service.ToInfoListAsync(filter, ct);
```

### [README](README.md)
