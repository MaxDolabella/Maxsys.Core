# Maxsys.Data

Biblioteca Maxsys de acesso a dados com Entity Framework Core: implementações de Repository Pattern e Unit of Work para os contratos definidos em `Maxsys.Core` (`IRepository`, `IUnitOfWork`), com consultas dinâmicas via `ColumnFilter`/`ListCriteria` e projeções via `IQueryProjector` (adaptador AutoMapper no pacote `Maxsys.Mapping.AutoMapper`).

## Repositórios

### RepositoryBase

Classe base (não genérica) de todos os repositórios. Implementa `IRepository` e `IDisposable`.

- `Context` (`DbContext` protegido) — o contexto compartilhado.
- `Id` / `ContextId` — identificadores do repositório e da instância do contexto.
- `Dispose(bool)` é virtual e, por padrão, **não** descarta o `DbContext` (o ciclo de vida do contexto é do UnitOfWork/DI).

### RepositoryBase&lt;TEntity&gt;

Implementação concreta de `IRepository<TEntity>`. Cobre CRUD, consultas por `Expression`, consultas dinâmicas por `ColumnFilter`/`ListCriteria`, projeções (via `IQueryProjector` ou expressão manual), paginação e ordenação.

- `GetQueryable(predicate, @readonly, ct)` — ponto central (virtual) de obtenção da query. A flag `@readonly` alterna `AsNoTracking()`/`AsTracking()`. Sobrescreva para aplicar filtro global, `Include` etc.
- `ApplyProjection<TDestination>(source)` — *chokepoint* único de projeção via `IQueryProjector`. Sobrescrevível para injetar políticas de leitura (ex.: Field-Level Security).
- `ApplyProjection<TDestination>(source, projection)` — *chokepoint* equivalente para projeções manuais (`Expression<Func<TEntity, TDestination>>`).
- Escrita: `AddAsync`, `UpdateAsync`, `DeleteAsync` (por entidade ou por `object[] keys`), `ExecuteDeleteAsync(predicate)`.
- Cenário desconectado: `Update(entity, updatingData)` e `Delete(entity/entities)` via `Attach`.
- Utilitários: `CountAsync`, `AnyAsync`, `IdExistsAsync(object[] ids)`, `HasChanges(entity)`.
- Leitura: sobrecargas de `ToListAsync`/`GetAsync`/`GetByIdAsync`/`GetSingleOrDefaultAsync`/`GetSingleOrThrowsAsync` aceitando `Expression`, `ICollection<ColumnFilter>`, `ListCriteria`, `Pagination` e *sort selectors*; `GetWithIncludeAsync` para navegações.
- Chaves compostas são suportadas por `object[] keys` — a expressão de identidade é montada a partir da primary key do modelo EF (`GetIdExpression`).

```csharp
public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context, IQueryProjector projector)
        : base(context, projector)
    { }

    // Filtro global + include aplicados a TODAS as consultas do repositório
    protected override async ValueTask<IQueryable<Product>> GetQueryable(
        Expression<Func<Product, bool>>? predicate = null,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = (await base.GetQueryable(predicate, @readonly, cancellationToken))
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted);

        return query;
    }

    // Opcional: interceptar toda projeção do mapeador (ex.: mascarar campos sensíveis)
    protected override IQueryable<TDestination> ApplyProjection<TDestination>(IQueryable<Product> source)
        => base.ApplyProjection<TDestination>(source);
}

// Uso
var dto = await repository.GetByIdAsync<ProductDto>([productId], cancellationToken);

var page = await repository.ToListAsync<ProductListDto>(
    filters: [new ColumnFilter { Column = "CategoryId", Value = categoryId }],
    criteria: new ListCriteria { Pagination = new Pagination(1, 20) },
    cancellationToken);
```

### JoinRepositoryBase&lt;TEntity, TJoin&gt;

Repositório abstrato para consultas em que a entidade `TEntity` é convertida em um objeto de join não natural `TJoin` antes da projeção final. Utiliza `ColumnFilter` para filtragem dinâmica.

- `EntityToJoinQueryableConvert(query, filters)` — método abstrato onde a subclasse define o join (ex.: `LeftOuterJoin` de `Maxsys.Core`).
- `GetQueryable(filters, @readonly, ct)` / `GetJoinQueryable(...)` — obtenção das queries base e de join, com ordenação opcional sobre a entidade.
- Sobrecargas de `CountAsync`, `AnyAsync`, `ToListAsync`, `GetAsync`, `GetSingleOrDefaultAsync`, `GetSingleOrThrowsAsync` e `GetByIdAsync` operando sobre `TJoin` — projeções partem de `TJoin` (mapeie `TJoin -> TDestination`).

```csharp
public sealed class LocationJoin
{
    public required Location Location { get; init; }
    public Country? Country { get; init; }
}

public class LocationRepository : JoinRepositoryBase<Location, LocationJoin>
{
    public LocationRepository(AppDbContext context, IQueryProjector projector)
        : base(context, projector)
    { }

    protected override IQueryable<LocationJoin> EntityToJoinQueryableConvert(
        IQueryable<Location> query, ICollection<ColumnFilter>? filters)
    {
        return query.LeftOuterJoin(Context.Set<Country>(),
                location => location.CountryId,
                country => country.Id,
                join => new { Location = join.Outer, Country = join.Inner })
            .Select(a => new LocationJoin
            {
                Location = a.Location,
                Country = a.Country
            });
    }
}

// Uso: filtros dinâmicos aplicados na entidade, projeção a partir do join
var list = await repository.ToListAsync<LocationDetailsDto>(filters, criteria, cancellationToken);
```

## Unit of Work

### UnitOfWorkBase&lt;TContext&gt;

Implementação abstrata de `IUnitOfWork` sobre um `DbContext`, com transações nomeadas/aninhadas controladas por semáforo e persistência retornando `OperationResult`.

- `BeginTransactionAsync(name?, ct)` — inicia uma transação (nome opcional, apenas para log). Chamadas aninhadas incrementam o semáforo em vez de abrir nova transação.
- `CommitTransactionAsync(ct)` — decrementa o semáforo; o commit real só ocorre quando o semáforo zera (transação mais externa).
- `RollbackTransactionAsync(ct)` — desfaz a transação corrente e zera o semáforo.
- `SaveChangesAsync(ct)` — retorna `OperationResult`; em caso de exceção, reverte o `ChangeTracker` para o estado original (auto-rollback dos entries) e adiciona a exceção como notificação. Fora de transação, limpa o tracker após salvar.
- `ClearTracker()` — limpa o `ChangeTracker` do contexto.

```csharp
public class AppUnitOfWork : UnitOfWorkBase<AppDbContext>
{
    public AppUnitOfWork(ILogger<AppUnitOfWork> logger, AppDbContext context)
        : base(logger, context)
    { }
}

// Uso em um serviço: operações atômicas (aninhamento é seguro)
await _uow.BeginTransactionAsync("CREATE_ORDER", cancellationToken);

await _orderRepository.AddAsync(order, cancellationToken);
var result = await _uow.SaveChangesAsync(cancellationToken);

if (!result.IsValid)
{
    await _uow.RollbackTransactionAsync(cancellationToken);
    return result;
}

await _uow.CommitTransactionAsync(cancellationToken);
```

## Extensions

### ConfigurationExtensions

Extension members para `IConfiguration`.

- `GetConnectionString<TContext>()` — obtém a connection string cujo nome é o nome do `DbContext` (atalho para `GetSection("ConnectionStrings")[typeof(TContext).Name]`).

```csharp
var conn = configuration.GetConnectionString<AppDbContext>();
services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conn));
```

### ConventionsExtensions

Convenções para `ModelConfigurationBuilder`.

- `StringToVarcharConvention(maxLength = -1)` — mapeia todas as propriedades `string` como não-unicode (`varchar`) com tamanho máximo opcional.

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.StringToVarcharConvention(100);

    base.ConfigureConventions(configurationBuilder);
}
```

### EntityFrameworkExtensions

Extension members para `IEntityType` (metadados do EF Core).

- `GetIdExpression<T>(object[] ids)` — monta dinamicamente a `Expression<Func<T, bool>>` de comparação com a primary key do modelo (suporta chave composta; valida quantidade de chaves × ids).

```csharp
var predicate = DbSet.EntityType.GetIdExpression<User>([userId, workspaceId]);
var user = await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
```

### IoCExtensions

Registro dos componentes de dados no container de DI.

- `AddContext<TContext>()` — registra o `DbContext` via DI nativa.
- `AddUnitOfWork<TUnitOfWork>()` — registra a implementação como `IUnitOfWork` (scoped).
- `AddGenericRepositories<TInterfaceEntry, TImplementationEntry>(lifetime)` — registra `IRepository<>` → `RepositoryBase<>` genérico no lifetime informado.

```csharp
services.AddContext<AppDbContext>()
    .AddUnitOfWork<AppUnitOfWork>()
    .AddGenericRepositories<IDataEntry, IDataEntry>();
```

## Conversão de Valores

### ObjectToJsonValueConverter&lt;TModel&gt;

`ValueConverter<TModel?, string?>` que serializa/desserializa a propriedade como JSON (via `System.Text.Json`) para persistência em coluna texto. Aceita `JsonSerializerOptions` opcional no construtor.

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Order>()
        .Property(o => o.Metadata)
        .HasConversion(new ObjectToJsonValueConverter<OrderMetadata>());
}
```

## Infraestrutura

### IDataEntry

Interface vazia usada como referência do assembly (ex.: para registro de tipos por assembly scanning).
