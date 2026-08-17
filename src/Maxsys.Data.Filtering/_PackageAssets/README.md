<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Data — Filtering</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Data.Filtering** é a implementação **EF Core** dos filtros tipados (*specification*) do `Maxsys.Core.Filtering`: o `RepositoryBase<TEntity, TFilter>`, que implementa `IRepository<TEntity, TFilter>` aplicando `filters.ApplyFilter(ref query)` antes de cada consulta.

É a ponta que faltava entre os contratos (`Maxsys.Core.Filtering`) e o acesso a dados (`Maxsys.Data`): estenda-o em vez de `RepositoryBase<TEntity>` quando a entidade tiver um filtro tipado.

## :gear: Uso

```csharp
// 1. Filtro tipado (Maxsys.Core.Filtering)
public class ProductFilter : FilterBase<Guid, Product>
{
    public string? Name { get; set; }

    public override void ConfigureExpressions()
    {
        if (!string.IsNullOrWhiteSpace(Name))
            AddExpression(p => p.Name.Contains(Name));
    }
}

// 2. Repositório concreto (este pacote)
public interface IProductRepository : IRepository<Product, ProductFilter>;

public class ProductRepository(AppDbContext context, IQueryProjector projector)
    : RepositoryBase<Product, ProductFilter>(context, projector), IProductRepository;

// 3. Service com filtro (Maxsys.Core.Filtering)
public class ProductService(IProductRepository repository, IUnitOfWork uow, IObjectMapper mapper)
    : ModelServiceBase<Product, IProductRepository, Guid, ProductFilter>(repository, uow, mapper);
```

As projeções (`ToListAsync<TDestination>`, `GetAsync<TDestination>`...) passam pelos mesmos *chokepoints* `ApplyProjection` do `RepositoryBase<TEntity>` — ou seja, via `IQueryProjector` (agnóstico de mapeador; use `Maxsys.Mapping.AutoMapper` ou implemente o seu).

## :dart: Target
`.NET 10`

## :link: Dependências

- `Maxsys.Core.Filtering`
- `Maxsys.Data`

## :black_nib: Autores
[@MaxDolabella](https://www.github.com/MaxDolabella)

## :old_key: Licença
Este código possui licença MIT e está liberado para uso da maneira que se desejar.
