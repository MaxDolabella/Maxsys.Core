# Refatoração maxsys-core → arquitetura modular (net10)

> Documento vivo. Captura as decisões e o plano da refatoração grande iniciada em 2026-06-29.
> Objetivo: alinhar a solution ao padrão modular de referência (multi-pacote NuGet), mantendo
> a identidade Maxsys (nomes, ícones, URLs).
>
> **Status: ✅ CONCLUÍDA em 2026-06-30.** Solution inteira compila (0 erros) e empacota
> (10 nupkgs). Versões: libs principais **17.0.0**, `Maxsys.Archive` **2.0.0**,
> `Maxsys.Bootstrap` **0.0.5**. Este documento permanece como registro histórico das decisões.

## Princípios

- **Target `net10.0` puro.** netstandard2.1 foi **descartado** (custo de polyfills/`#if`/duplicação
  de extensões C#14 não compensou). Código idiomático net10: `extension` members, `required`,
  collection expressions `[]`, `DateOnly`/`TimeOnly`, file-scoped namespaces — tudo liberado.
- **Sem dependências externas de mensageria.** Mediador próprio (substitui MediatR).
- **Nada de referência a outras empresas** no código, docs ou metadados. Só Maxsys.
- **Ícones preservados:** `logo.ico` / `logo.png` (já existentes em cada `_PackageAssets/`).
- **Pastas/arquivos podem mudar de nome e estrutura.** Conteúdo `Obsolete/`, `Old/`, `_ignore/`
  é ignorado (não migra). `AppCore/` e `Models/` do Core estão vazios → descartados.

## Orquestração (raiz)

- `Directory.Build.props` — TFM net10, nullable, implicit usings, LangVersion latest.
- `Directory.Build.targets` — metadados de pacote sob flag `IsMaxsysPackage`; achata
  `_PackageAssets/*` (logo.png, README.md, LICENSE) na raiz do `.nupkg`.
- `Directory.Packages.props` — Central Package Management (todas as versões aqui).
- Publicação NuGet: tag `publish-*` via `.github/workflows/dotnet-nuget.yml` — **modernizado**:
  actions atuais (`checkout@v5`, `setup-dotnet@v4`), SDK `10.0.x`, restore/build da `.slnx` +
  `pack --no-build` por projeto de `src/` (globstar explícito), `permissions: contents: read`.

## Mapa de pacotes

| Antes | Depois | Namespace / PackageId | Depende de |
|---|---|---|---|
| Maxsys.Core | **Maxsys.Core** | `Maxsys.Core` | — |
| Maxsys.Core.Data | **Maxsys.Data** | `Maxsys.Data` | Core |
| Maxsys.Core.Excel | **Maxsys.Excel** | `Maxsys.Excel` | Core |
| Maxsys.Core.Web | **Maxsys.Web** | `Maxsys.Web` | Core (FrameworkRef AspNetCore.App) |
| Maxsys.Core.Web.Swagger | **Maxsys.Swagger** | `Maxsys.Swagger` | Web |
| *(novo)* | **Maxsys.Messaging** | `Maxsys.Messaging` | Core |
| *(novo)* | **Maxsys.EventSourcing** | `Maxsys.EventSourcing` | Core + Messaging |
| *(novo)* | **Maxsys.Drawing** | `Maxsys.Drawing` | Core (System.Drawing.Common, Windows-only) |
| *(novo)* | **Maxsys.Mapping.AutoMapper** | `Maxsys.Mapping` (PackageId `Maxsys.Mapping.AutoMapper`) | Core + AutoMapper (único pacote com AutoMapper) |
| Maxsys.Archive | *(intacto, retarget net10)* | — | — |
| Maxsys.Bootstrap | *(intacto, retarget net10)* | — | — |

**Ordem de execução (dependências):**
```
1º  Core
2º  Data · Messaging · Excel · Web · Drawing   (só dependem de Core)
3º  EventSourcing (Core+Messaging) · Swagger (Web)
```

## Decisões de design

- **Service base entity-centric → renomeado para `ModelService`** (nem sempre há Entity 1:1; o
  serviço expõe Model/DTO ao mundo externo). Família:
  - `ServiceBase` : `IService` — base descartável (`Guid Id`, Dispose). Inalterado.
  - `ModelServiceBase<TModel, TRepository>` — leitura (Get/List/Count/Any) + eventos async.
  - `ModelServiceBase<TModel, TRepository, TKey>` — + CRUD (UoW + AutoMapper).
  - (Não há variante `TValidator` — validação fica nos handlers/pipeline. A variante de 4 parâmetros do template era a `TFilter`, removida junto com o `IFilter`.)
  - Interfaces correspondentes: `IModelService<...>`.
- **`Result` factory estática** adotada sobre o `OperationResult` existente (`Result.Success()`,
  `Result.Error()`, `Result.Warning<T>()`, etc.). `OperationResult`/`OperationResult<T>` continuam
  sendo os tipos de retorno.
- **Mensageria:** mediador próprio (`Maxsys.Messaging`) — `IBus`, `ICommand`/`IQuery`/`IEvent`,
  handlers, `ValidationBehavior`, DI por reflexão. Bases enxutas: `CommandBase`, `QueryBase`,
  `QueryHandlerBase`.

## Relatório de gap — features exclusivas do maxsys (resolvido)

| Feature | Decisão |
|---|---|
| `IOHelper` | **Mantido** no Core. |
| `ImageHelper` (+ `System.Drawing.Common`) | **Mantido, isolado** em `Maxsys.Drawing` (dep Windows-only fora do Core). |
| Extensões `ToObservableCollection`/`ToReadOnlyObservableCollection` | **Removidas** (CommunityToolkit.Mvvm cobre no lado do app). |
| Templates CRUD (`CreateCommandBase`/`Update`/`Delete`, `GetByIdQueryBase`/`GetListQueryBase`/`ToListQueryBase`) | **Removidos.** |
| `RepositoryBase<TKey1, TKey2, TEntity>` (chave composta) | **Correção posterior:** essa variante não existia no v16 (mapeamento inicial impreciso). Chaves compostas são suportadas via `object[] keys` + `GetIdExpression`; a variante realmente removida foi a `RepositoryBase<TEntity, TFilter>` (IFilter). |
| `ServiceBase<R1..R8>` (1–8 repositórios) | **Substituído** pela família `ModelServiceBase`. |
| `ApiMultipleResults<T>` / `ResultItem<T>` | **Resolvido:** o padrão novo cobre via `ApiOperationResult(OperationResultCollection)` + `ResultItem<T>` (portado para `Maxsys.Core.Web`). `ApiMultipleResults<T>` não migrou. |
| Filtragem por `IFilter`/`FilterBase` (specification pattern) | **Removida** (decisão explícita). Saíram: `IFilter`, `FilterBase`, `SearchTerm(+Modes)`, `SearchKey(+Modes)`, `KeyList`, `FilterItem`, `ActiveTypes` e as variantes `IRepository<TEntity,TFilter>`, `IModelService<…,TFilter>`, `ModelServiceBase<…,TFilter>`. Filtragem única: `ColumnFilter`. |
| `PeriodFilter` / `RangeFilter` / `DateTimeOffsetFilter` | **Removidos** (decisão explícita). `DateTimeExtensions.IsBetween` reescrito sem dependência deles. |

## Convenções de migração

- Trocar todo identificador/namespace/string da empresa de referência por `Maxsys`.
- Documentação dos `_PackageAssets/` (`README.md`, `CHANGELOGS.md`, `FEATURES.md`) adaptada por lib.
- XML docs públicos em pt-BR.
- **Filtragem:** só `ColumnFilter` (PrimeNG-style). Qualquer implementação/overload baseado em `IFilter`/`FilterBase` (variantes `*<…, TFilter>`) foi removido — só os caminhos `ColumnFilter` e `predicate`. `PeriodFilter`/`RangeFilter` também removidos.
- **Sort:** só por `Field` (string). O enum-sort (byte) foi removido do Core (`ApplySort` lança se receber `SortFilter` por byte).
- **Global using `Maxsys.Core`:** ao dropar "Core" do nome (Data/Web/Excel), o namespace deixa de ser filho de `Maxsys.Core` e perde a visibilidade implícita dos tipos-raiz (`OperationResult`, `ListCriteria`, `Pagination`...). Cada lib que dropa "Core" precisa de `<Using Include="Maxsys.Core" />` no csproj.
- Avisos benignos esperados: **CS0618** (uso de ctor `[Obsolete]` do `OperationResult`, herdado do template) e **CS0114** (hiding no `JoinRepositoryBase`, design do template).
- Versão: bump da solution para a próxima major (**17.0.0**, aplicado) por ser refatoração de
  ruptura. `Maxsys.Archive` → **2.0.0** (major pelo retarget net10); `Maxsys.Bootstrap` → **0.0.5**.
- **Encoding:** todo `.cs` em UTF-8. Os fontes do template eram mistura de Latin-1 e UTF-8-com-BOM;
  a porção foi feita com detecção de encoding **por arquivo** (script node) para não corromper acentos.

## Ajustes pós-migração (revisão final, 2026-06-30)

- **Entry markers renomeados** (breaking): `ICoreDataEntry`→`IDataEntry`, `ICoreExcelEntry`→`IExcelEntry`,
  `ICoreWebEntry`→`IWebEntry`, `ICoreSwaggerEntry`→`ISwaggerEntry`. `ICoreEntry` mantido (o pacote é o Core).
  Classe legada `Entry` (`[Obsolete]`) removida de todas as libs.
- **Sintaxe de corpo vazio:** 13 tipos vazios (`{ }`) convertidos para a sintaxe de `;` do C# 12
  (ex.: `public interface ICoreEntry;`) — markers, `IDTO`, `OperationResultCollection`, contratos CQRS
  (`IEvent`, `ICommand*`, `IQuery`, `CommandBase*`).
- **`Maxsys.Web`:** `UseHealthCheck()` promovido de `internal` para `public` (era inacessível a consumidores);
  constantes `Headers` rebatizadas com prefixo `mx-` (antes carregavam iniciais da empresa de referência).
- **Documentação completa:** `README.md` (raiz + por lib), `FEATURES.md` e `CHANGELOGS.md` de todas as 8 libs
  reescritos a partir do código v17 (formato H2-área/H3-tipo com exemplos, legível para humano e RAG);
  históricos de changelog preservados; entries 17.0.0/2.0.0/0.0.5 adicionadas.
- **`CLAUDE.md`** reescrito para a estrutura v17; URLs de pacote (Archive/Bootstrap) corrigidas para o layout `src/`.
- **AutoMapper desacoplado (2026-07-02, breaking):** Core/Data não referenciam mais AutoMapper.
  Abstrações em `Maxsys.Core.Interfaces.Mapping` — `IObjectMapper` (instâncias; `ModelServiceBase<,,TKey>`
  agora o recebe no ctor) e `IQueryProjector` (projeção de `IQueryable`; `RepositoryBase`/`JoinRepositoryBase`
  o recebem no ctor). `JoinRepositoryBase` ganhou o chokepoint `ApplyJoinProjection` (antes tinha 13 chamadas
  `ProjectTo` diretas que ignoravam o `ApplyProjection` — bug latente corrigido). Novo pacote
  **`Maxsys.Mapping.AutoMapper`** (namespace `Maxsys.Mapping`) com `AutoMapperAdapter` + `AddMaxsysAutoMapper<TEntry>()`
  (auto-scan de Profiles). Consumidores sem AutoMapper implementam as duas interfaces e registram no DI.

> Migração concluída: build da solution com 0 erros e `dotnet pack` gerando os 10 pacotes.
> Warnings remanescentes são os benignos catalogados acima (CS0618/CS0114).
