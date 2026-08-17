# Maxsys.Core.Filtering

:mortar_board: Cada lançamento é um novo aprendizado!!

## 17.0.0
* :tada: Versão inicial do pacote — reintroduz como **extensão opt-in** os filtros tipados (*specification*) que faziam parte do `Maxsys.Core` até a v16 (removidos do núcleo na v17 em favor do `ColumnFilter`);
* :sparkles: Contratos `IFilter`, `IKeyFilter<TKey>` e `IFilter<TEntity>` (com `Expressions` + `ApplyFilter`);
* :sparkles: Bases `FilterBase` / `FilterBase<TKey>` / `FilterBase<TKey, TEntity>` com `ConfigureExpressions()`, `AddExpression()` e `AddSearchExpression()`;
* :sparkles: Blocos de construção: `SearchTerm`/`SearchTermModes` (busca textual traduzida para `Expression` via `ExpressionHelper`), `KeyList<TKey>`/`SearchKey<TKey>`/`SearchKeyModes` (Include/Exclude de chaves), `RangeFilter<T>`, `PeriodFilter`/`PeriodFilter<TDateTypeFilter>`/`DateTimeOffsetFilter`, `FilterItem<T>` e `ActiveTypes`;
* :sparkles: `IRepository<TEntity, TFilter>` — variante de repositório com consultas por filtro tipado (`GetAsync`, `ToListAsync`, `CountAsync`, `AnyAsync`, `GetSingleOrDefaultAsync`, `GetSingleOrThrowsAsync`...);
* :sparkles: `IModelService<TEntity, TKey, TFilter>` + `ModelServiceBase<TEntity, TRepository, TKey, TFilter>` — variante de service com GET/LIST/QTY por filtro tipado (incluindo `ListDTO`/`InfoDTO`);
* :package: Depende apenas de `Maxsys.Core`. A implementação concreta do repositório fica no consumidor (ex.: estendendo `RepositoryBase` de `Maxsys.Data`).
