# Maxsys.Core

Biblioteca base para aplicações Maxsys (.NET 10 / C# 14): resultado de operação padronizado (`OperationResult`), contratos de Repository/Unit of Work, serviços de modelo (`ModelServiceBase`), filtragem dinâmica via `ColumnFilter`, listagem/ordenação/paginação genéricas, cache, auditoria e utilitários diversos.

## Attributes

### DefaultSortAttribute
Indica a *property* que será a ordenação padrão de um tipo quando nenhum `SortFilter` for informado em `QueryableExtensions.ApplySort`.
+ `Property` (string): caminho da propriedade (aceita *dot notation*).
+ `SortDirection`: direção padrão (`Ascending` se omitida).

```csharp
[DefaultSort(nameof(Name))]
public class CityDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

// sem Sorts no ListCriteria → ordena por Name Ascending
var list = cityQueryable.ApplySort(null);
```

### SearchableAttribute
Marca uma propriedade como participante da busca global (`ListCriteria.Search`, aplicada por `QueryableExtensions.ApplySearch`).
+ Sem parâmetros: para propriedades `string` diretas.
+ Com `path`: para propriedades aninhadas (objetos complexos). Permite múltiplos atributos na mesma propriedade.

```csharp
public class PersonDTO
{
    [Searchable]
    public string Name { get; set; }

    // gera os full paths "Location.Country.Name" e "Location.City"
    [Searchable("Country.Name")]
    [Searchable("City")]
    public LocationDTO Location { get; set; }
}

var criteria = new ListCriteria { Search = "maria" };
var result = personQueryable.ApplyCriteria(criteria); // OR entre todas as props searchable
```

### DependencyInjectionIgnoreAttribute
Indica que a classe não será registrada no `ServiceProvider` pelos métodos de extensão de `ServiceCollectionExtensions` (ex.: `AddImplementations`).

### NotDefaultAttribute
`ValidationAttribute` (DataAnnotations) que especifica que um campo não pode possuir o valor `default` do seu tipo. Lembrando: `null` não é `default` — valor nulo é considerado válido (não implica *required*).

```csharp
public class CreateOrderRequest
{
    [NotDefault] // Guid.Empty é inválido
    public Guid CustomerId { get; set; }
}
```

### TextColumnAttribute / XmlColumnAttribute
Atributos de *marker* usados para indicar que uma *property* é do tipo `TEXT`/`XML` no banco, auxiliando na obtenção do *selector* de ordenação em implementações de dados.

```csharp
public class Document
{
    [TextColumn]
    public string Body { get; set; }

    [XmlColumn]
    public string Payload { get; set; }
}
```

---
## Resultado de Operação

### OperationResult / OperationResult&lt;T&gt;
Tipo de retorno padrão de operações. Carrega uma lista de `Notification` e, na versão genérica, um `Data` do tipo `T`. Serviços retornam `OperationResult` em vez de lançar exceção para erros de regra de negócio.
+ `IsValid`: `false` se houver notificação com severidade ≤ `Warning` (ou seja, `Error` ou `Warning`).
+ `ResultType`: o `ResultTypes` mais severo entre as notificações (`Success` se não houver nenhuma).
+ `AddNotification` / `AddNotifications` / `AddException` / `AddWarningNotification` / `AddErrorNotification`: adicionam notificações (ignorando duplicadas).
+ `ContainsNotification` / `ContainsAnyNotification`: verificam presença de notificações por mensagem ou predicado.
+ `Cast<TDestination>(data)`: converte um `OperationResult` em `OperationResult<TDestination>` preservando as notificações.
+ `WithData` (extension): atalho para preencher `Data` de forma fluente.
+ Os construtores com `string`/`Exception`/`ValidationResult` estão `[Obsolete]` — use a factory `Result`.

```csharp
public async Task<OperationResult<Guid>> ApproveAsync(Guid id, CancellationToken ct)
{
    var order = await _repository.GetAsync([new ColumnFilter("Id", id)], cancellationToken: ct);
    if (order is null)
        return Result.Error<Guid>(GenericMessages.ITEM_NOT_FOUND);

    if (order.Status != OrderStatus.Pending)
        return Result.Warning<Guid>("ORDER_NOT_PENDING", details: $"Status atual: {order.Status}");

    order.Approve();
    await _uow.SaveChangesAsync(ct);

    return Result.Success(order.Id);
}

// consumo
var result = await service.ApproveAsync(id, ct);
if (!result.IsValid)
{
    // result.Notifications garantidamente não-nulo aqui (MemberNotNullWhen)
    logger.LogWarning("{Errors}", string.Join("; ", result.Notifications));
}
```

### Result (factory estática)
Ponto de criação padronizado de `OperationResult`/`OperationResult<T>`. Resultados não-Success nunca carregam `Data` (a versão tipada retorna `Data = default`).
+ `Result.Success()` / `Result.Success<T>(data)`
+ `Result.Info(message, details?, tag?)` / `Result.Info<T>(...)`
+ `Result.Warning(...)` / `Result.Warning<T>(...)`
+ `Result.Error(...)` / `Result.Error<T>(...)`
+ `Result.FromException(exception, resultType?)` / `Result.FromException<T>(...)` — com *overload* para mensagem customizada.
+ `Result.FromNotifications(notifications)` / `Result.FromNotifications<T>(data?, notifications)`

```csharp
return Result.Success(dto);
return Result.Error("ITEM_DUPLICATE", details: $"Código {code} já existe");
return Result.FromException<CustomerDTO>(ex);
return Result.FromNotifications(validationResult.ConvertToNotifications());
```

### Notification
Representa uma notificação em um `OperationResult`. Implementa `IEquatable<Notification>` (duplicadas não são adicionadas duas vezes).
+ `Message`: código do erro (ex.: `ITEM_NOT_FOUND`).
+ `Details`: mensagem complementar ou detalhes da exception.
+ `ResultType`: severidade (`Error` por padrão).
+ `Tag`: objeto livre para transportar dado adicional.
+ Construtores a partir de `string` ou `Exception` (agrega `InnerException.Message` em `Details`).

```csharp
var notification = new Notification("EXTERNAL_SERVICE_UNAVAILABLE", details: responseContent, ResultTypes.Warning)
{
    Tag = payload
};
result.AddNotification(notification);
```

### ResultTypes
Enum de severidade de resultado, ordenado do mais severo para o menos: `Error = 0`, `Warning = 1`, `Info = 2`, `Success = 3`. Serializado como string em JSON (`JsonStringEnumConverter`).

### OperationResultCollection / OperationResultCollection&lt;T&gt;
Coleção de `OperationResult`(`<T>`) que também implementa `IOperationResult`: `IsValid` só é `true` se todos os itens forem válidos; `Notifications` agrega as notificações de todos os itens. Usada como retorno de operações em lote (`AddAsync`/`UpdateAsync`/`DeleteAsync` de coleções).

```csharp
OperationResultCollection<Guid?> results = await service.DeleteAsync(ids, stopOnFirstFail: false, ct);
if (!results.IsValid)
{
    var failed = results.Where(r => !r.IsValid).ToList();
}
```

### IOperationResult
Interface comum de `OperationResult` e `OperationResultCollection`: `ResultType`, `IsValid`, `Notifications`, `ContainsNotification`, `SetDataToNull()`.

### GenericMessages
Constantes com as mensagens/códigos mais comuns da aplicação, no formato `warnings.common.*`: severidades base (`SUCCESS`, `ERROR`, `WARNING`, `INFORMATION`), acesso (`UNAUTHORIZED`), CRUD (`ITEM_NOT_FOUND`, `ERROR_ADDING`, `ERROR_UPDATING`, `ERROR_DELETING`, `ERROR_SAVE`), operações (`INVALID_OPERATION`, `INVALID_OBJECT`, `INVALID_XML`, `INVALID_SCHEMA`, `SCHEMA_READING_ERROR`) e validação (`FIELD_REQUIRED`, `FIELD_INVALID`, `FIELD_UNIQUE`, `FIELD_LENGTH`, `FIELD_FORMAT`, `FIELD_RANGE`, `FIELDS_CONFLICT`, `ITEM_REQUIRED`, `ITEM_DUPLICATE`).

```csharp
return Result.Error(GenericMessages.ITEM_NOT_FOUND);
```

---
## DTOs

### IDTO
Interface *marker* para tipificar um objeto como DTO de entidade (List, Form, etc.).

### InfoDTO&lt;T&gt;
DTO mínimo de referência (id + descrição), usado nas listagens `ToInfoListAsync`/`GetInfoListAsync` de `ModelServiceBase`.
+ `Id` (required), `Description` (required), `Abbreviation`, `CustomState`.

```csharp
List<InfoDTO<Guid>> combo = await service.ToInfoListAsync(x => x.IsActive, ct);
```

### ListDTO&lt;T&gt;
Retorno de lista paginada: `Items` (a página) + `Count` (total de registros na fonte de dados).

```csharp
ListDTO<ProductListDTO> page = await service.GetListAsync<ProductListDTO>(criteria, ct);
// page.Items.Count → itens da página; page.Count → total geral
```

### MonitorableDTO / UpdateStatus
`MonitorableDTO` é um DTO abstrato com a property `UpdateStatus`, útil para sincronizar coleções cliente-servidor. `UpdateStatus` (enum byte) indica a ação a tomar com o objeto:
+ `Loaded = 0` (veio do banco), `Insert`, `Update`, `Delete`, `None`.

```csharp
public class OrderItemDTO : MonitorableDTO
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
}

foreach (var item in dto.Items.Where(i => i.UpdateStatus == UpdateStatus.Insert))
{
    // inserir...
}
```

### ObjectLink&lt;TNav, TItem&gt; / ObjectLinkItem&lt;T&gt;
Estrutura para vincular um item de navegação a uma lista de itens monitoráveis (cada `ObjectLinkItem<T>` herda de `MonitorableDTO`). Útil para manutenção de relações N:N.

```csharp
var link = new ObjectLink<CategoryDTO, ProductDTO>
{
    NavigationItem = category,
    Items = products.Select(p => new ObjectLinkItem<ProductDTO> { Item = p }).ToList()
};
```

---
## Entidades

### Entity / Entity&lt;TKey&gt;
Classes base para entidades de domínio.
+ `Entity`: base sem chave — use para entidades com chave composta (configure `HasKey(x => new { x.PropA, x.PropB })` no EF).
+ `Entity<TKey>`: entidade com chave única tipada (`Id`), implementa `IKey<TKey>`. `TKey` deve ser um tipo escalar simples (`int`, `Guid`, `string`...) — não use tuplas/records como chave.

```csharp
public class Product : Entity<Guid>
{
    public string Name { get; set; }
}
```

### IKey&lt;TKey&gt;
Interface para um objeto que contenha uma chave `Id` de tipo não nulo. Implementada por `Entity<TKey>`, `InfoDTO<T>` e exigida nos DTOs de update de `IModelService<TEntity, TKey>`.

---
## Filtragem

### ColumnFilter
Filtro dinâmico de coluna (estilo *matchModes* do PrimeNG). É o mecanismo único de filtragem da biblioteca — aplicado por `QueryableExtensions.ApplyFilters` e aceito em métodos de `IRepository<TEntity>`/`IModelService`.
+ `Field`: caminho da propriedade (aceita *dot notation* para propriedades aninhadas).
+ `Value`: valor de comparação (filtros com `Value == null` são ignorados).
+ `MatchMode`: modo de comparação (`Contains` por padrão).

```csharp
var filters = new List<ColumnFilter>
{
    new("Name", "Silva"),                                        // Contains (default)
    new("Status", OrderStatus.Active, FilterMatchModes.Equals),
    new("Customer.Address.City", "São", FilterMatchModes.StartsWith),
    new("Total", new object[] { 100, 500 }, FilterMatchModes.Between),
};

var query = orderQueryable.ApplyFilters(filters); // AND entre os filtros
var dto = await _repository.GetAsync<OrderDTO>(filters, ct);
```

### FilterMatchModes
Modos de comparação suportados por `ColumnFilter`, serializados como string em JSON:
+ Texto: `Contains`, `StartsWith`, `EndsWith`.
+ Igualdade: `Equals`, `NotEquals`.
+ Coleção: `In`, `NotIn`.
+ Numérico: `Gt`, `Gte`, `Lt`, `Lte`, `Between` (`[min, max]`).
+ Data: `DateIs`, `DateIsNot`, `DateBefore`, `DateAfter`.

### ColumnFilterExtensions
Composição de listas de `ColumnFilter` com *expressions* fortemente tipadas em vez de strings literais.
+ `AddFilter<TModel>(property, value, matchMode = Equals)`: o caminho do campo é extraído da expression (suporta propriedades aninhadas).

```csharp
var filters = new List<ColumnFilter>();
filters.AddFilter<Order>(x => x.Status, OrderStatus.Active);
filters.AddFilter<Order>(x => x.Customer.Name, "Silva", FilterMatchModes.Contains);
filters.AddFilter<Order>(x => x.Total, 1000, FilterMatchModes.Gte);
```

---
## Listagem e Ordenação

### ListCriteria
Vocabulário comum de consulta paginada, agregando paginação, ordenações, filtros de coluna e busca global.
+ `Pagination`: página/tamanho (opcional).
+ `Sorts`: lista de `SortFilter`.
+ `Filters`: lista de `ColumnFilter`.
+ `Search`: termo da busca global (aplicado nas propriedades com `[Searchable]`).

```csharp
var criteria = new ListCriteria
{
    Pagination = new Pagination(index: 0, size: 20),
    Sorts = [new SortFilter("Customer.Name", SortDirection.Ascending)],
    Filters = [new ColumnFilter("Status", OrderStatus.Active, FilterMatchModes.Equals)],
    Search = "maria"
};

ListDTO<OrderListDTO> page = await service.GetListAsync<OrderListDTO>(criteria, ct);
```

### Pagination
Configuração de paginação: `Index` (base 0) e `Size`. `IsNotEmpty()` indica se a paginação deve ser aplicada (`Size > 0`).

### SortFilter / SortDirection
`SortFilter` define a ordenação de uma coluna:
+ `Field` (string): nome/caminho da propriedade a ordenar — aceita *dot notation* (`"State.Country.Name"`). A ordenação por enum/byte foi removida na v17; `Field` é a única forma.
+ `Direction`: `SortDirection.Ascending` (1) ou `SortDirection.Descending` (2).

```csharp
var criteria = new ListCriteria
{
    Sorts =
    [
        new SortFilter("State.Country.Name", SortDirection.Ascending),
        new SortFilter("Abbreviation", SortDirection.Descending),
    ]
};

var sorted = cityQueryable.ApplySort(criteria.Sorts);
```

### QueryableExtensions
Extensões de `IQueryable<T>` que materializam `ListCriteria` (C# 14 *extension members*).
+ `ApplyCriteria(criteria)`: atalho para `ApplyFilters(...).ApplySearch(...).ApplySort(...).ApplyPagination(...)`.
+ `ApplyFilters(filters)`: aplica cada `ColumnFilter` como `Where` (AND sequencial), construindo as expressões dinamicamente via `ExpressionHelper.BuildColumnFilterExpression`.
+ `ApplySearch(search)`: busca textual global — OR entre as propriedades decoradas com `[Searchable]` (com cache de paths por tipo).
+ `ApplySort(sortFilters)`: ordena por `SortFilter.Field`; sem sorts, usa o `[DefaultSort]` do tipo, se existir. Lança `InvalidOperationException` se algum `SortFilter` não tiver `Field`.
+ `ApplyPagination(pagination)`: `Skip(Size * Index).Take(Size)` quando a paginação não é vazia.
+ `LeftOuterJoin` / `LeftOuterJoinList`: *left outer join* entre queryables, retornando `LeftOuterJoinResult<TSource, TInner>` / `LeftOuterJoinListResult<TSource, TInner>`.

```csharp
var page = await orderQueryable
    .ApplyCriteria(criteria)
    .ToListAsync(ct);

var joined = orders.LeftOuterJoin(
    customers,
    o => o.CustomerId,
    c => c.Id,
    (o, c) => new { Order = o, Customer = c });
```

---
## Serviços

### IService / ServiceBase
Contrato/base mínimos para tipificar um objeto como Service.
+ `IService`: `Guid Id` + `IDisposable`.
+ `ServiceBase`: implementa `IService` com `Id` gerado por `UIDGen.NewGuid()` e o padrão `Dispose(bool)`.

```csharp
public sealed class ReportService : ServiceBase, IReportService
{
    // ...
}
```

### IModelService&lt;TEntity&gt; / ModelServiceBase&lt;TEntity, TRepository&gt;
Serviço de leitura sobre uma entidade, delegando ao repositório e projetando para DTOs (via mapeador registrado ou *projection* explícita).
+ Consulta pontual: `GetAsync<TDestination>` (por predicate, projection ou `ColumnFilter`), `GetByIdAsync<TDestination>`, `GetSingleOrDefaultAsync`, `GetSingleOrThrowsAsync`.
+ Listagens: `ToListAsync<TDestination>` (lista simples) e `GetListAsync<TDestination>` (retorna `ListDTO<T>` com `Count`), com *overloads* por predicate/`ColumnFilter`/`ListCriteria`/paginação + `sortSelector`.
+ Utilitários: `CountAsync`, `AnyAsync`, `IdExistsAsync`.
+ Eventos async pós-consulta: `GetCompletedAsync`, `ToListCompletedAsync`, `GetListCompletedAsync` (delegate `AsyncEventHandler<ValueEventArgs>`), com *hooks* protegidos `OnAfterGetAsync`/`OnAfterToListAsync`/`OnAfterGetListAsync`.
+ Constraints: `TEntity : class`, `TRepository : IRepository<TEntity>`.

### IModelService&lt;TEntity, TKey&gt; / ModelServiceBase&lt;TEntity, TRepository, TKey&gt;
Extensão CRUD completa do serviço de modelo. Recebe `TRepository`, `IUnitOfWork` e `IObjectMapper` no construtor; exige implementar `IdSelector(TKey id)`.
+ Escrita: `AddAsync<TCreateDTO>` (item ou coleção), `UpdateAsync<TUpdateDTO>` (`TUpdateDTO : class, IKey<TKey>`; item ou coleção), `DeleteAsync(TKey)` (item ou coleção) — versões de coleção retornam `OperationResultCollection` e aceitam `stopOnFirstFail`.
+ Leitura por chave: `GetAsync<TDestination>(TKey id[, projection])`.
+ Listagens de referência: `ToInfoListAsync` / `GetInfoListAsync` retornando `InfoDTO<TKey>`.
+ Eventos async de ciclo de vida:
  + Pré-operação (podem vetar): `AddingAsync`, `UpdatingAsync`, `DeletingAsync` (`OperationResultAsyncEventHandler` — retorno inválido cancela a operação).
  + Pós-operação: `AddedAsync`, `UpdatedAsync` (com `AddedEntityEventArgs`/`UpdatedEntityEventArgs`), `DeletedAsync` (`ValueEventArgs`).
  + *Hooks* protegidos equivalentes: `OnBeforeAdd/Update/DeleteAsync`, `OnAfterAdd/Update/DeleteAsync`.

```csharp
public interface IProductService : IModelService<Product, Guid> { }

public sealed class ProductService : ModelServiceBase<Product, IProductRepository, Guid>, IProductService
{
    public ProductService(IProductRepository repository, IUnitOfWork uow, IObjectMapper mapper)
        : base(repository, uow, mapper)
    { }

    protected override Expression<Func<Product, bool>> IdSelector(Guid id) => x => x.Id == id;

    // Veto de regra de negócio antes do delete
    protected override async ValueTask<OperationResult> OnBeforeDeleteAsync(Guid id, CancellationToken ct)
    {
        return await _repository.AnyAsync(x => x.Id == id && x.HasOrders, ct)
            ? Result.Error("PRODUCT_HAS_ORDERS")
            : Result.Success();
    }
}

// uso
var created = await productService.AddAsync(createDTO, ct);       // OperationResult<ProductCreateDTO>
var dto = await productService.GetAsync<ProductDetailsDTO>(id, ct);
var page = await productService.GetListAsync<ProductListDTO>(criteria, ct);

// assinatura de evento
productService.AddedAsync += async (sender, e, ct) =>
{
    logger.LogInformation("Produto {Id} criado", ((Product)e.Entity!).Id);
};
```

### HttpClientBase
Classe base (em `Maxsys.Core.Services.Http`) para serviços que consomem APIs HTTP externas via `IHttpClientFactory`.
+ `AddAuthTokenAsync` (abstrato): retorna o `AuthenticationHeaderValue` (ou `null` se não houver autenticação).
+ `GetHttpRequestMessageAsync` (com/sem *body* JSON, com `HttpContent` custom ou sem autenticação): monta a request com auth + headers.
+ `GetHttpResponseMessageAsync`: envia e retorna `(HttpResponseMessage Message, string Content)`.
+ `GetApiResponseAsync`: envia e valida se a response é uma API no padrão esperado (prop `title` com prefixo) — senão lança `ExternalAPIException`.

```csharp
public sealed class MovieService : HttpClientBase, IMovieService
{
    private readonly IMovieTokenProvider _tokenProvider;

    public MovieService(ILogger<MovieService> logger, IHttpClientFactory factory, IMovieTokenProvider tokenProvider)
        : base(logger, factory)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async ValueTask<AuthenticationHeaderValue?> AddAuthTokenAsync(CancellationToken ct = default)
        => new("Bearer", await _tokenProvider.GetTokenAsync(ct));

    public async Task<OperationResult<MovieDTO>> GetMovieAsync(Guid id, CancellationToken ct = default)
    {
        var (message, content) = await GetHttpResponseMessageAsync(HttpMethod.Get, $"movies/{id}", requestHeaders: null, ct);

        return message.IsSuccessStatusCode
            ? Result.Success(content.FromJson<MovieDTO>())
            : Result.Error<MovieDTO>(GenericMessages.ITEM_NOT_FOUND, details: content);
    }
}
```

---
## Mapeamento (abstrações)

A partir da v17, o Core **não depende de AutoMapper**. O mapeamento é abstraído em duas interfaces
(`Maxsys.Core.Interfaces.Mapping`), implementadas por um pacote adaptador — o oficial é
`Maxsys.Mapping.AutoMapper` (registro via `AddMaxsysAutoMapper`).

### IObjectMapper
Mapeamento objeto → objeto (instâncias em memória). Usado por `ModelServiceBase<TEntity, TRepository, TKey>` no CRUD.
+ `Map<TDestination>(object source)` — nova instância de destino.
+ `Map<TDestination>(object source, Action<TDestination> afterMap)` — mapeia e executa pós-processamento no momento do map (roda **após** o pipeline do mapeador; tem implementação default na interface — adapters customizados não precisam implementar).
+ `Map<TSource, TDestination>(source, destination)` — mapeamento *in-place* (update).

```csharp
var dto = _mapper.Map<ProductDTO>(entity, dto => dto.DisplayName = $"{dto.Code} - {dto.Name}");
```

### IQueryProjector
Projeção de `IQueryable` → `IQueryable<TDestination>` por composição de *expression tree*
(traduzível pelo provedor LINQ; não materializa a query). Usado pelos repositórios de `Maxsys.Data`.
+ `Project<TDestination>(IQueryable source)`.

```csharp
// Consumidor com AutoMapper (pacote Maxsys.Mapping.AutoMapper):
services.AddMaxsysAutoMapper<IApplicationEntry>(); // scan de Profiles + IObjectMapper/IQueryProjector

// Consumidor SEM AutoMapper: implemente as interfaces e registre no DI.
public sealed class MyProjector : IQueryProjector
{
    public IQueryable<TDestination> Project<TDestination>(IQueryable source) => /* Mapster, manual... */;
}
```

---
## Repositórios (contratos)

As implementações concretas (EF Core) vivem em `Maxsys.Data`. Aqui ficam apenas os contratos.

### IRepository
Interface básica para tipificar um objeto como Repositório: `Guid Id` (identificador do repositório), `Guid ContextId` (identificador do contexto em uso) e `IDisposable`.

### IRepository&lt;TEntity&gt;
Repositório CRUD completo da entidade, com filtragem por *expression* ou `ColumnFilter` e projeção via mapeador (`IQueryProjector`) ou *projection* explícita.
+ Escrita: `AddAsync` (item/coleção), `UpdateAsync` (item/coleção), `DeleteAsync` (por chaves ou entidade), `ExecuteDeleteAsync(predicate)`.
+ Desconectado: `Update(entity, updatingData)` (update parcial com objeto anônimo), `Delete(entity)` / `Delete(entities)` — somente `Id` necessário.
+ Utilitários: `CountAsync` / `AnyAsync` (por predicate, `ColumnFilter` em entidade e/ou DTO, ou `ListCriteria`), `IdExistsAsync(object[])`, `HasChanges(entity, ...)`.
+ Listagem: dezenas de *overloads* de `ToListAsync` combinando origem do filtro (predicate ou `ColumnFilter`), projeção (`TDestination` via mapeador ou expression) e paginação/ordenação (`ListCriteria` ou `Pagination` + `sortSelector`).
+ Consulta pontual: `GetAsync` (por predicate ou `ColumnFilter`, com projeção e/ou ordenação), `GetByIdAsync` (por chaves, com/sem projeção), `GetWithIncludeAsync` (com `includeNavigation`), `GetSingleOrDefaultAsync` / `GetSingleOrThrowsAsync`.

```csharp
public interface IProductRepository : IRepository<Product> { }

// exemplos de uso do contrato
var exists = await _repository.IdExistsAsync(CompositeKeyHelper.Of(id), ct);
var dtos = await _repository.ToListAsync<ProductListDTO>(x => x.IsActive, criteria, ct);
_repository.Update(new Product { Id = id }, new { Name = "Novo nome" }); // update desconectado
```

### IUnitOfWork
Contrato de Unit of Work para transação e persistência.
+ `BeginTransactionAsync(name?)` / `CommitTransactionAsync()` / `RollbackTransactionAsync()`.
+ `SaveChangesAsync()`: retorna `OperationResult` (no EF Core, também limpa o ChangeTracker).
+ `ClearTracker()`: limpa o ChangeTracker.
+ `Id` / `ContextId`: identificadores do UoW e do contexto.

```csharp
await _uow.BeginTransactionAsync(cancellationToken: ct);

var saveResult = await _uow.SaveChangesAsync(ct);
if (!saveResult.IsValid)
{
    await _uow.RollbackTransactionAsync(ct);
    return saveResult;
}

await _uow.CommitTransactionAsync(ct);
```

---
## Cache

### ICacheManager / CacheManager
Wrapper de `IMemoryCache` que mantém uma coleção *thread-safe* das chaves cacheadas, permitindo enumeração e limpeza seletiva (coisa que `IMemoryCache` puro não oferece).
+ `Set<T>(key, value, options)`: adiciona entrada e rastreia a chave.
+ `TryGetValue<T>(key, out value)`: tenta obter (remove do rastreio chaves expiradas).
+ `Remove(key)`, `GetAllKeys()`.
+ `Clear(predicate?)`: remove todas as entradas ou apenas as com chave que satisfaça o predicado.

```csharp
public class CatalogService(ICacheManager cache, IProductRepository repository)
{
    public async Task<List<ProductListDTO>> GetCatalogAsync(CancellationToken ct)
    {
        const string key = "catalog:all";

        if (cache.TryGetValue<List<ProductListDTO>>(key, out var cached))
            return cached!;

        var items = await repository.ToListAsync<ProductListDTO>(cancellationToken: ct);
        cache.Set(key, items, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });

        return items;
    }

    public void InvalidateCatalog() => cache.Clear(k => k.StartsWith("catalog:"));
}
```

### CacheManagerDependencyInjectionExtensions
Registro do cache no container:
+ `services.AddCacheManager()`: registra `IMemoryCache` + `ICacheManager` (singleton).
+ `services.AddCacheManager<TService>()`: registra também um *keyed/typed* manager para o serviço.

```csharp
services.AddCacheManager();
```

---
## Auditoria

### AuditHelper / AuditLog / AuditLogField
Comparação de dois estados de um objeto (ou dois JSONs) gerando um log de alterações campo a campo — incluindo propriedades aninhadas (achatadas com *dot notation*).
+ `AuditHelper.GetAuditLog(object obj1, object obj2)` / `GetAuditLog(string json1, string json2)`: retorna `AuditLog`.
+ `AuditLog.Fields`: array de `AuditLogField` com `Field`, `OldValue` e `NewValue` (`null` em `NewValue` = campo removido; `null` em `OldValue` = campo inserido).

```csharp
var before = new { Name = "Maria", Address = new { City = "Santos" } };
var after  = new { Name = "Maria", Address = new { City = "São Paulo" } };

AuditLog log = AuditHelper.GetAuditLog(before, after);
// log.Fields => [ { Field = "Address.City", OldValue = "Santos", NewValue = "São Paulo" } ]
```

---
## JSON

### JsonExtensions
Serialização/desserialização padronizadas sobre `System.Text.Json`, com `JSON_DEFAULT_OPTIONS` públicas (camelCase, enums como string etc.) reutilizadas em toda a biblioteca (inclusive nos serviços HTTP).
+ `json.FromJson<T>()` / `json.FromJson<T>(defaultValue)` / `json.FromJson(Type)`.
+ `json.TryFromJson<T>(out obj, out notification, options?)`: variante que não lança — em falha, retorna uma `Notification` pronta.
+ `value.ToJson(options?)`: serializa (retorna `null` se o objeto for `null`).

```csharp
string json = """{"id":123,"name":"Giuseppe Kadura"}""";

Person person = json.FromJson<Person>();
Person safe = json.FromJson(Person.Default); // retorna default se json nulo

if (!json.TryFromJson<Person>(out var parsed, out var notification))
    return Result.FromNotifications([notification]);

string serialized = person.ToJson();
```

### DateTimeOffsetJsonConverter
`JsonConverter<DateTimeOffset>` que força a serialização em ISO 8601 com timezone UTC e sufixo `Z` (`yyyy-MM-ddTHH:mm:ss.fffZ`) em vez do offset `+00:00`. Na leitura, string nula/vazia vira `DateTimeOffset.MinValue`.

### UnixTimestampJsonConverter
`JsonConverter<DateTime>` que lê valores numéricos como Unix Timestamp em milissegundos (convertidos para UTC) e escreve datas como string ISO 8601 (formato `"o"`).

```csharp
public class EventDTO
{
    [JsonConverter(typeof(UnixTimestampJsonConverter))]
    public DateTime OccurredAt { get; set; }

    [JsonConverter(typeof(DateTimeOffsetJsonConverter))]
    public DateTimeOffset CreatedAt { get; set; }
}
```

---
## Exceções

### DomainException
Representa um erro de domínio. Base das demais exceções de domínio da biblioteca.

### ExternalAPIException
Erro ao chamar uma API externa. Carrega o `HttpStatusCode` da chamada e compõe a mensagem com o status + *reason phrase*.

### InvalidEnumArgumentException&lt;TEnum&gt;
Versão genérica de `InvalidEnumArgumentException` — captura automaticamente o nome do argumento via `CallerArgumentExpression`.

### InvalidServiceProviderException
Erro ao validar um ServiceProvider; recebe a lista de erros encontrados.

### NotAllowedOperationException
Erro ao tentar realizar uma operação não permitida.

### NotAuthenticatedUserException
Erro quando o usuário não está autenticado.

```csharp
if (invalidValue is not OrderStatus status)
    throw new InvalidEnumArgumentException<OrderStatus>((OrderStatus)invalidValue);

if (user is null)
    throw new NotAuthenticatedUserException();

throw new ExternalAPIException(response.StatusCode, responseContent);
```

---
## Eventos

### AsyncEventHandler&lt;TEventArgs&gt; / OperationResultAsyncEventHandler&lt;TEventArgs&gt;
Delegates de evento assíncronos (retornam `ValueTask` / `ValueTask<OperationResult>`) com `CancellationToken`. São a base dos eventos de `ModelServiceBase` — a variante `OperationResult` permite que o *handler* vete a operação.

```csharp
service.AddingAsync += async (sender, entity, ct) =>
{
    return entity.Price < 0
        ? Result.Error("INVALID_PRICE")
        : Result.Success();
};
```

### ValueEventArgs
`EventArgs` genérico com um `Value` (`object?`) e auxiliares `GetValueAs<T>()` / `IsValue<T>()`.

### AddedEntityEventArgs / UpdatedEntityEventArgs / ModifiedEntityEventArgs
`EventArgs` dos eventos pós-escrita de `ModelServiceBase`: carregam a `Entity` persistida e o `DTO` de origem.

```csharp
service.UpdatedAsync += async (sender, e, ct) =>
{
    var entity = e.Entity; // TEntity
    var dto = e.DTO;       // DTO usado no update
};
```

---
## HTTP

### HttpServiceBase
Classe base (em `Maxsys.Core.Http`) para consumir **APIs Maxsys** (que respondem no envelope `ApiResult`), convertendo a resposta diretamente em `OperationResult`. Diferente de `HttpClientBase` (genérico para qualquer API), valida a resposta como uma API Maxsys — identificador `MaxsysAPI` no JSON (com compatibilidade retroativa via `apiPrefix` para APIs antigas).
+ Atalhos por verbo: `GetResultAsync[<T>]`, `GetPostResultAsync[<T>]`, `GetPutResultAsync[<T>]`, `GetDeleteResultAsync[<T>]` — todos retornam `OperationResult`/`OperationResult<T>`.
+ Núcleo: `GetMaxsysApiAsync[<T>]` (valida resposta → converte `ApiResult` em `OperationResult`); `SendAsync` (envio cru com eventos).
+ Helpers: `CreateHttpRequestMessage`, `CreateJsonContent<T>` (usa `JSON_DEFAULT_OPTIONS`), `AddHeaders`, `AddContent`.
+ Eventos assíncronos: `Sending`, `Sent`, `MaxsysApiResponseInvalid`, `MaxsysApiResponseValid`.

```csharp
public sealed class BillingApiService : HttpServiceBase, IBillingApiService
{
    public BillingApiService(IHttpClientFactory factory) : base(factory)
    {
        Sending += async (s, e, ct) =>
            e.HttpRequestMessage.Headers.Authorization = new("Bearer", await GetTokenAsync(ct));
    }

    public Task<OperationResult<InvoiceDTO>> GetInvoiceAsync(Guid id, CancellationToken ct = default)
        => GetResultAsync<InvoiceDTO>($"invoices/{id}", requestHeaders: null, ct);

    public Task<OperationResult> CancelInvoiceAsync(Guid id, CancellationToken ct = default)
        => GetPostResultAsync($"invoices/{id}/cancel", null, CreateJsonContent(new { Reason = "user" }), ct);
}
```

### MaxsysApiValidationResult
Resultado da validação de uma resposta de API Maxsys em `HttpServiceBase`.
+ Factories: `CreateValidResult()` / `CreateInvalidResult(statusCode, errorMessage, content?, exception?)`.
+ `ToOperationResult()` / `ToOperationResult<T>()`: converte a falha em `OperationResult` com notificação detalhada.

### Delegates e EventArgs de HTTP
`SendingEventHandler`/`SendingEventArgs`, `SentEventHandler`/`SentEventArgs`, `MaxsysApiResponseInvalidEventHandler`/`MaxsysApiResponseInvalidEventArgs`, `MaxsysApiResponseValidEventHandler`/`MaxsysApiResponseValidEventArgs` — os tipos usados pelos eventos de `HttpServiceBase` (todos `record`s simples com a mensagem/resultado correspondente).

---
## Helpers

### CompositeKeyHelper
Açúcar sintático para montar chaves compostas (`object[]`) de forma legível.

```csharp
var keys = CompositeKeyHelper.Of(orderId, productId);
await _repository.DeleteAsync(keys, ct);
```

### DateTimeHelper
Conversões e limites de data:
+ `FromUnixTimestamp(long)` / `ToUnixTimestamp(DateTime)` (milissegundos).
+ `StartDate(date)` / `EndDate(date)`: início (00:00:00) e fim (23:59:59.9999999) do dia — *overloads* para `DateTime` e `DateTimeOffset`.

```csharp
var start = DateTimeHelper.StartDate(DateTime.Today);
var end = DateTimeHelper.EndDate(DateTime.Today);
```

### EncryptHelper
Criptografia simétrica AES com *salt*: `AESEncrypt(plainText, salt)` / `AESDecrypt(cipherText, salt)`.

```csharp
var cipher = EncryptHelper.AESEncrypt("segredo", salt);
var plain = EncryptHelper.AESDecrypt(cipher, salt);
```

### ExpressionHelper
Construção dinâmica de *expressions*:
+ `GetMemberAccessExpression<T>(propertyName)`: cria `x => x.Prop` a partir de string (aceita *dot notation*) — usado pelo `ApplySort`.
+ `GetMemberPath<T>(expression)`: caminho ("A.B.C") a partir de uma expression — usado por `ColumnFilterExtensions.AddFilter`.
+ `BuildColumnFilterExpression<T>(filter)`: converte um `ColumnFilter` no predicado `Expression<Func<T, bool>>` correspondente — o coração do `ApplyFilters`.

```csharp
var selector = ExpressionHelper.GetMemberAccessExpression<City>("State.Name");
var predicate = ExpressionHelper.BuildColumnFilterExpression<City>(new("Name", "São", FilterMatchModes.StartsWith));
```

### HashHelper
Hashes de bytes/strings/objetos:
+ `ToSHA512` / `ToSHA512HashString` (extensões de `byte[]`/`MemoryStream`).
+ `GetHexHash(value, hashType = HashTypes.MD5)`: hash hexadecimal de `object`/`string`/`byte[]` — enum `HashTypes` define o algoritmo.

```csharp
var md5 = HashHelper.GetHexHash("conteudo");
var sha = fileBytes.ToSHA512HashString();
```

### IOHelper
Operações de arquivos com retorno `OperationResult` (assíncronas) e saneamento de nomes:
+ `MoveFileAsync` / `MoveOrOverwriteFileAsync` / `CopyFileAsync` / `DeleteFileAsync` — todas `ValueTask<OperationResult>`, com opção `setAsReadOnly`.
+ `InsertReadOnlyAttribute` / `RemoveReadOnlyAttribute` e manipulação de `FileAttributes`.
+ `RemoveInvalidFileNameChars` / `ReplaceInvalidFileNameChars` / `ReplaceAndRemoveInvalidFileNameChars` (idem para *directory*).

```csharp
var result = await IOHelper.MoveOrOverwriteFileAsync(sourcePath, destPath, setAsReadOnly: false, ct);
if (!result.IsValid) { /* tratar */ }
```

### ReflectionHelper
Descoberta de tipos por reflexão (base do `AddImplementations`):
+ `GetImplementationDictionary<TInterface>(assemblies, suffix?, predicate?)`: dicionário interface → implementação.
+ `GetInterfaces<TInterface>` / `GetImplementation<TInterface>`: listas de tipos filtradas por sufixo/predicado.

### RegexHelper
Padrões de regex prontos via enum `Pattern` + `GetPattern(pattern)`, e constantes como `PATTERN_FOR_VALID_FILE_NAME` e `PATTERN_FOR_VALID_FILE_PATH`.

### StringHelper
Utilitários de string (a maioria como métodos de extensão): `RemoveInvalidFileNameChars`, `GetTextOrNullIfEmpty`, `GetDecimalOrNullIfEmpty`, `GetDateTimeOrNullIfEmpty`, `ToHexString`, `GetOnlyNumbers`, `RemoveDiacritics`, `FirstCharToUpper`, `ToCamelCase`, `ToPascalCase`, `SplitLines`, `SplitTextIntoChunks`, `NormalizeText`, `GetLoremIpsumPhrase` (+ constante `LOREM_IPSUM`).

```csharp
var digits = "(11) 98888-7766".GetOnlyNumbers();  // "11988887766"
var ascii = "coração".RemoveDiacritics();          // "coracao"
var chunks = longText.SplitTextIntoChunks(4000);
```

### UIDGen
Geração de identificadores únicos:
+ `NewGuid(sequentialGuidOption?, dateTimeOffset?)`: Guid sequencial — padrão `SequentialAsVersion7` (UUID v7).
+ `GenerateUID(UIDBits bits | int bytes, UIDGenerationOptions options)`: UID string com tamanho configurável.
+ Enums: `SequentialGuidOptions`, `UIDBits`, `UIDGenerationOptions`.

```csharp
var id = UIDGen.NewGuid();                       // UUID v7 (sequencial)
var uid = UIDGen.GenerateUID(UIDBits.Bits64);
```

### XMLHelper
Serialização/validação XML:
+ `Serialize<T>(item)` / `Deserialize<T>(xml, defaultNamespace?)` / `Read(xml)` (→ `XElement`).
+ `ToXmlString<TXml>(root, encoding?, settings?)`: retorna `OperationResult<string?>`.
+ `ValidateSchema<TXml>(root, schemaResourceName | XmlSchema, ...)`: valida contra XSD retornando `OperationResult`.
+ `ReplaceElement(other, me, xName)` e `DEFAULT_XML_SETTINGS`.

```csharp
var xmlResult = XMLHelper.ToXmlString(invoice, Encoding.UTF8);
var validation = XMLHelper.ValidateSchema(invoice, schema);
```

---
## Extensions

Todas reescritas com *extension members* do C# 14 (`extension(...)` blocks).

### ClaimsPrincipalExtensions
+ `user.GetIdentifier(identifier = ClaimTypes.NameIdentifier)`: valor do claim (lança `NotAuthenticatedUserException` se ausente).
+ `user.GetIdentifierAsGuid(...)`: idem, convertido para `Guid`.

```csharp
Guid userId = User.GetIdentifierAsGuid();
```

### DateTimeExtensions
+ `dateTime.IsBetween(initialDate, endDate)`: checa se a data está entre duas datas (inclusivo).

```csharp
var isChristmas2023 = new DateTime(2023, 12, 25).IsBetween(new(2023, 1, 1), new(2023, 12, 31)); // true
```

### EnumExtensions
+ `value.ToFriendlyName(defaultValue?)`: nome amigável do literal (usa `DescriptionAttribute` quando presente; literal sem correspondência vira o valor numérico como string).
+ `text.ToEnum<TEnum>()`: converte string (nome ou *friendly name*) em enum — `null` se não casar.
+ `value.Convert<TTarget>()` / `ConvertNull<TTarget>()`: conversão entre enums.
+ `byteValue.ToByteEnum<TEnum>(defaultEnum)`.
+ Estáticos: `Min<T>`/`Max<T>`/`GetMinValue`.

```csharp
public enum SampleEnum : byte
{
    [Description("Este é o tipo A")]
    TipoA = 1,
    TipoC = 99
}

SampleEnum.TipoA.ToFriendlyName();       // "Este é o tipo A"
SampleEnum.TipoC.ToFriendlyName();       // "TipoC"
"Este é o tipo A".ToEnum<SampleEnum>();  // SampleEnum.TipoA
"TipoD".ToEnum<SampleEnum>();            // null
```

### FluentValidationExtensions
Integração FluentValidation ↔ `Notification`:
+ `rule.WithNotification(message[, details[, tag]], resultType = Warning)`: anexa metadados de `Notification` à regra.
+ `validationResult.ConvertToNotifications()`: `ValidationResult` → `List<Notification>`.
+ `validationFailure.ConvertToNotification()`.

```csharp
public class CreateProductValidator : AbstractValidator<ProductCreateDTO>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithNotification(GenericMessages.FIELD_REQUIRED, "Nome é obrigatório", ResultTypes.Error);
    }
}

var validation = await validator.ValidateAsync(dto, ct);
if (!validation.IsValid)
    return Result.FromNotifications(validation.ConvertToNotifications());
```

### NotificationExtensions
+ `notifications.ToResultType(@default = Success)`: o `ResultTypes` mais severo de uma coleção de notificações.

### ServiceCollectionExtensions
Registro em massa por convenção (interface × implementação descobertas por reflexão):
+ `AddImplementations<TInterface>(interfaceAssemblies, implementationAssemblies, suffix?, predicate?)` e variantes com *entry types* (`<TInterface, TInterfaceEntry, TImplementationEntry>` / `<TInterface, TEntry>`). Classes com `[DependencyInjectionIgnore]` são puladas.
+ `Add<TService, TImplementation>(lifetime)` / `Add<TService>(lifetime)`: registro com `ServiceLifetime` parametrizado.
+ `RegisterImplementationDictionary(keyValues, lifetime = Scoped)`.
+ `ReplaceServiceImplementation<TService, TReplaceImplementation>(lifetime = Scoped)`: substitui a implementação registrada de um serviço.

```csharp
// registra todas as interfaces *Service do assembly de ICoreEntry
// com suas implementações no assembly de IDataEntry
services.AddImplementations<IService, ICoreEntry, IDataEntry>("Service");

services.ReplaceServiceImplementation<IEmailSender, FakeEmailSender>();
```

### TypeExtensions
+ `type.IsAssignableToGenericType(genericType)`: checa atribuição a tipo genérico aberto (ex.: `typeof(IRepository<>)`).
+ `type.TryGetAttribute<T>(out value)`: obtém atributo do tipo, se existir.

```csharp
if (type.IsAssignableToGenericType(typeof(IModelService<,>))) { /* ... */ }
```

### JsonExtensions / QueryableExtensions / ColumnFilterExtensions
Documentadas nas seções [JSON](#json), [Listagem e Ordenação](#listagem-e-ordenação) e [Filtragem](#filtragem), respectivamente.

---
## Utils

### RandomUtils
Geração aleatória baseada em `Random.Shared`:
+ Primitivos: `NextBool`, `NextChance(probability)`, `NextInt`, `NextLong`, `NextFloat`, `NextDouble`, `NextDecimal`, `NextBytes`.
+ Strings: `NextString(length[, chars])`, `NextHexString`, `NextDigits`.
+ Datas: `NextDateTime`, `NextDateTimeOffset`, `NextTimeSpan`.
+ Coleções: `GetRandomEnum<TEnum>(except?)`, `GetRandomItem`, `GetRandomItems`, `Shuffled`.

```csharp
var otp = RandomUtils.NextDigits(6);
var status = RandomUtils.GetRandomEnum<OrderStatus>(except: [OrderStatus.None]);
var sample = RandomUtils.GetRandomItems(products, 5);
```

### ResourcesUtils
Acesso a recursos embutidos (*embedded resources*):
+ `GetEmbeddedResource<TAssemblyReference>(resourceName)`: `Stream?` do recurso.
+ `ListResourcesInAssembly<TAssemblyReference>()`.
+ `GetXmlSchema<TAssemblyReference>(resourceName)`: `OperationResult<XmlSchema?>` a partir de um XSD embutido.

```csharp
var schemaResult = ResourcesUtils.GetXmlSchema<ICoreEntry>("Maxsys.Core.Schemas.invoice.xsd");
```

### StringWriterWithEncoding
`StringWriter` com `Encoding` configurável (o padrão do .NET é fixo em UTF-16) — usado, por exemplo, na serialização XML com UTF-8.

### IgnoreNamespaceXmlTextReader
`XmlTextReader` que ignora namespaces ao desserializar XML (`NamespaceURI` sempre vazio).

```csharp
using var reader = new IgnoreNamespaceXmlTextReader(new StringReader(xml));
var obj = (Invoice?)new XmlSerializer(typeof(Invoice)).Deserialize(reader);
```

---
## Web / ApiResult

### ApiResultBase
Base do envelope de resposta das APIs Maxsys. Propriedades ordenadas no JSON:
+ `MaxsysAPI`: identificador fixo (`"MaxsysAPI"`) que marca a resposta como API Maxsys — usado na validação de `HttpServiceBase`.
+ `Title`, `StatusCode`, `ResultType`, `Tag`.

### ApiResult / ApiResult&lt;T&gt;
Envelope de resposta com `Notifications` (e `Data` na versão genérica). Constrói-se diretamente de um `OperationResult`(`<T>`), herdando notificações, `ResultType` e `Data`.

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> Get(Guid id, CancellationToken ct)
{
    var result = await _service.GetOperationAsync(id, ct); // OperationResult<ProductDTO>

    var apiResult = new ApiResult<ProductDTO>("PRODUCTS-GET", StatusCodes.Status200OK, result);
    return Ok(apiResult);
}
```

### ApiMultipleResults&lt;T&gt;
Envelope para operações em lote: converte uma `OperationResultCollection<T>` em uma lista de `ResultItem<T?>` (um resultado por item processado).

### ResultItem&lt;T&gt;
Item individual de `ApiMultipleResults`: `Data`, `Notifications` e `ResultType` (o mais severo das notificações).

### ApiResultExtensions
Conversões `ApiResult` → `OperationResult` (o caminho inverso do envelope, usado ao consumir APIs Maxsys):
+ `apiResult.ToOperationResult()` / `apiResult.ToOperationResult<T>()`: copia `Data`/notificações; `StatusCode == 404` adiciona `ITEM_NOT_FOUND`.

```csharp
var apiResult = responseContent.FromJson<ApiResult<MovieDTO>>();
OperationResult<MovieDTO> result = apiResult.ToOperationResult();
```

---
## Entrada do assembly

### ICoreEntry
Interface *marker* para referenciar o assembly `Maxsys.Core` (ex.: em `AddImplementations<TInterface, TEntry>`).

### [README](README.md)
