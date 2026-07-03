# Maxsys.Data

## 17.0.0
* :warning: Pacote renomeado de `Maxsys.Core.Data` para `Maxsys.Data` (PackageId e namespace raiz);
* :warning: Interface de entry point renomeada de `ICoreDataEntry` para `IDataEntry`; removida a classe obsoleta `Entry`;
* :warning: Repositórios desacoplados do AutoMapper: `RepositoryBase`/`JoinRepositoryBase` agora recebem `IQueryProjector` (antes `IMapper`); novo *chokepoint* `ApplyJoinProjection` no `JoinRepositoryBase`. Use o pacote `Maxsys.Mapping.AutoMapper` (`AddMaxsysAutoMapper`) para manter o comportamento anterior;
* :warning: Atualização de framework (`.NET 10`) e `Entity Framework Core 10`;
* :warning: `RepositoryBase<TEntity>` reescrito:
	* Agora é classe concreta (registrável como `IRepository<>` genérico via `AddGenericRepositories`);
	* Consultas dinâmicas via `ColumnFilter`/`ListCriteria` (substituindo as variantes baseadas em `IFilter`/`FilterBase`);
	* Projeções centralizadas no *chokepoint* sobrescrevível `ApplyProjection` (para políticas de leitura, ex.: Field-Level Security);
	* Flag `@readonly` (`AsNoTracking`/`AsTracking`) mantida em todas as consultas;
* :sparkles: Adicionado `JoinRepositoryBase<TEntity, TJoin>` para consultas com joins não naturais e filtragem via `ColumnFilter`;
* :triangular_flag_on_post: Removida a variante `RepositoryBase<TEntity, TFilter>` (baseada em `IFilter<TEntity>`/`FilterBase`);
* :recycle: Pasta/namespace `ValueConverters` renomeado para `ValueConversion`;
* :recycle: `IConfigurationExtensions` renomeada para `ConfigurationExtensions` (agora usando *extension members*);

---
## 16.0.0
* :package: Atualização de pacotes;
* :sparkles: Adicionado método para limprar Tracker em UnitfWork;
* Repositórios:
	* :warning: Método `FilterBase.SetExpressions()` subtituído por `ApplyFilter()`;
	* :recycle: RepositoryBase agora é classe concreta;
	* :recycle: Implementados métodos em Repository incluindo suporte à execução desconectada;
* :sparkles: Adicionado método para registrar Repositórios genéricos;
* :sparkles: Adicionadas classes de extensão `IConfigurationExtensions` e `ConventionsExtensions`;
* :triangular_flag_on_post: Adicionadas classes de extensão `IConfigurationExtensions` e `ConventionsExtensions`;

---
## 15.1.0
* :warning: Atualização de dependências;
* Adicionada sobrecarga para `RepositoryBase.UpdateAsync`;

---
## 15.0.0
* :warning: Atualização de dependências;
* :warning: Método `FilterBase.SetExpressions()` subtituído por `.ApplyFilter()`;

---
## 14.0.0
* :warning: Atualização de dependências;

---
## 13.0.0
* :warning: Atualização de framework (`.NET 9`);
* :warning: Atualização de dependências;

---
## 12.0.0
* :warning: Atualização de dependências;
* `RepositoryBase`: Métodos com `object id` em parametros removidos. Somente `object[] keys` mantidos.

---
## 11.0.0
* :warning: Atualização de dependências;
* :warning: Refatoração de implementações de `IRepository` incluindo adição de métodos que aceitam `Expression` para *projection* e remoção de *constraints*;
* :warning: Removido `BusExtensions`. Usar *Interceptors*;

---
## 10.1.0
* :warning: Projeto totalmente refatorado;

---
## 7.1.0
* Atualização de pacotes NUGET (`Maxsys.Core` e `Microsoft.EntityFrameworkCore`).
* Alteração de namespace em `IoCExtensions`.
* `RepositoryBase` refatorado (adequação às alterações em `Maxsys.Core`).
* Adicionado log em `UnitOfWorkBase`.

---
## 7.0.0
* Commit Inicial com implementações de `IUnitOfWork` e `IRepository`


<style>
  .warning { color: DarkGoldenRod; }
  h1 { color: Snow; }
  h2 { color: Crimson; }
  h3 { color: SteelBlue; }
  h4 { color: SeaGreen; }
</style>
