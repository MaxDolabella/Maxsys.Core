<div align="center">
<img src="logo.png" alt="drawing" width="128" />
<h1>Maxsys Core — Filtering</h1>
</div>

[![License](https://img.shields.io/github/license/maxdolabella/maxsys.core)](LICENSE)

**Maxsys.Core.Filtering** é a extensão do `Maxsys.Core` para **filtros tipados** (*specification*): classes de filtro fortemente tipadas (`FilterBase`/`IFilter`) que se traduzem em `Expression`s aplicadas à query, além das variantes de `IRepository` e `IModelService`/`ModelServiceBase` que consultam a partir de um `TFilter`.

Complementa (não substitui) a filtragem dinâmica via `ColumnFilter` do `Maxsys.Core`: use `ColumnFilter`/`ListCriteria` para grids dinâmicos estilo PrimeNG, e um `TFilter` tipado quando o filtro é um contrato de negócio explícito da API.

## :gear: Uso

```csharp
// 1. Defina o filtro da entidade
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

        if (Search is not null)
            AddSearchExpression(p => new[] { p.Name, p.Code });
    }
}

// 2. Repositório e service com filtro
public interface IProductRepository : IRepository<Product, ProductFilter>;

public class ProductService : ModelServiceBase<Product, IProductRepository, Guid, ProductFilter>
{
    // GetAsync(filter), ToListAsync(filter), GetListAsync(filter, criteria),
    // ToInfoListAsync(filter), CountAsync(filter), AnyAsync(filter)...
}
```

Inclui ainda blocos de construção para filtros: `SearchTerm` (busca textual com `SearchTermModes`), `KeyList<TKey>` (Include/Exclude de chaves), `RangeFilter<T>`/`PeriodFilter` (faixas e períodos) e `ActiveTypes`.

> :bulb: Este pacote define os **contratos e bases**. A implementação EF Core do repositório (`RepositoryBase<TEntity, TFilter>`) vive em **`Maxsys.Data.Filtering`** — ou implemente `IRepository<TEntity, TFilter>` você mesmo para outro provedor de dados.

## :dart: Target
`.NET 10`

## :link: Dependências

- `Maxsys.Core`

## :black_nib: Autores
[@MaxDolabella](https://www.github.com/MaxDolabella)

## :old_key: Licença
Este código possui licença MIT e está liberado para uso da maneira que se desejar.
