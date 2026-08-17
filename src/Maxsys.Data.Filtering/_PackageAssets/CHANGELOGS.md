# Maxsys.Data.Filtering

:mortar_board: Cada lançamento é um novo aprendizado!!

## 17.0.0
* :tada: Versão inicial do pacote — implementação EF Core dos filtros tipados de `Maxsys.Core.Filtering`;
* :sparkles: `RepositoryBase<TEntity, TFilter>` implementando `IRepository<TEntity, TFilter>`: aplica `ApplyFilter` do filtro na query base (`GetQueryable(filters)`) e delega projeções aos *chokepoints* `ApplyProjection` herdados de `RepositoryBase<TEntity>` (via `IQueryProjector` — sem dependência de AutoMapper);
* :package: Depende de `Maxsys.Core.Filtering` (contratos) e `Maxsys.Data` (EF Core, `RepositoryBase`).
