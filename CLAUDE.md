# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## O que é

`Maxsys.Core` é um conjunto de **bibliotecas (class libraries) .NET 10 publicadas como pacotes NuGet** que dão base a aplicações pessoais Maxsys em Windows Forms, WPF e ASP.NET Core (WebAPI/MVC). Não é uma aplicação executável — é infraestrutura reutilizável: Clean Architecture, Repository + Unit of Work, CQRS com mediador próprio, validação, filtragem/paginação genérica e um tipo de resultado de operação padronizado.

A solução usa o formato novo `.slnx` (`Maxsys.Core.slnx`), não `.sln`. A refatoração que originou essa estrutura (v17) está documentada em `REFACTOR.md` — consulte-o para o histórico de decisões (o que foi removido e por quê).

## Comandos

```bash
# Build de toda a solução
dotnet build Maxsys.Core.slnx

# Build de um projeto só
dotnet build src/Maxsys.Core/Maxsys.Core.csproj

# Empacotar (mesmo passo do CI) — gera .nupkg em ./out
dotnet pack src/Maxsys.Core/Maxsys.Core.csproj -c Release -o out
```

Não há suíte de testes unitários. `tests/Tests.Api` é um projeto ASP.NET Core de *sandbox* manual (referencia `Maxsys.Archive`), não um runner xUnit/NUnit — não espere `dotnet test` significativo.

## Build central (raiz)

- `Directory.Build.props` — TFM único `net10.0`, `Nullable`, `ImplicitUsings`, `LangVersion latest` para **todos** os projetos. Não declare TFM nos `.csproj`.
- `Directory.Build.targets` — metadados NuGet compartilhados sob a flag opt-in `<IsMaxsysPackage>true</IsMaxsysPackage>` (autor, ícone `logo.png`, README, LICENSE). Achata `_PackageAssets/*` na raiz do `.nupkg` via Remove+Include (não mexer sem entender o comentário no próprio arquivo).
- `Directory.Packages.props` — Central Package Management: **toda** versão de dependência é declarada aqui, nunca no `.csproj`.
- **AutoMapper está travado em 14.0.0** — última versão gratuita; versões posteriores são pagas. NÃO atualizar. Desde a v17, **só `Maxsys.Mapping.AutoMapper` referencia AutoMapper** — Core/Data usam as abstrações `IObjectMapper`/`IQueryProjector`.

## Versionamento e publicação

- **Versão é manual e por projeto**, na tag `<Version>` de cada `.csproj`. Libs principais em `17.0.0`; `Maxsys.Archive` em `2.0.0`; `Maxsys.Bootstrap` em `0.0.5`.
- **Publicação no NuGet dispara por git tag** `publish-*` (`.github/workflows/dotnet-nuget.yml`): restore/build da slnx + `dotnet pack` de cada `src/**/*.csproj` + `nuget push`. Push no master sozinho **não** publica nada.

## Arquitetura dos pacotes (camadas e dependências)

```
Maxsys.Core            ← núcleo, sem EF, ASP.NET nem AutoMapper. Contratos, DTOs, ModelServiceBase, OperationResult/Result, ColumnFilter, IObjectMapper/IQueryProjector.
  ├─ Maxsys.Data             → EF Core. RepositoryBase, JoinRepositoryBase, UnitOfWorkBase, ValueConversion.
  ├─ Maxsys.Mapping.AutoMapper → adaptador AutoMapper p/ IObjectMapper/IQueryProjector (AddMaxsysAutoMapper). Único pacote com AutoMapper.
  ├─ Maxsys.Web              → ASP.NET Core. ApiControllerBase, ApiActionResult, HealthCheck, FromJson binder.
  │    └─ Maxsys.Swagger     → filtros/extensions p/ Swashbuckle (enums, FromJson, ActionIdentifier).
  ├─ Maxsys.Excel            → ClosedXML. WorkbookFacade + mapeamento declarativo (TableTypeBuilder).
  └─ Maxsys.Messaging        → CQRS com mediador próprio (IBus, ICommand/IQuery/IEvent). SEM MediatR.
       └─ Maxsys.EventSourcing → DomainEvent, DomainEntity, StoredEvent, IEventStore (depende de Core + Messaging).
Maxsys.Drawing         ← ImageHelper (System.Drawing.Common, Windows-only). Isolado de propósito — não mover pro Core.
Maxsys.Archive         ← compressão/arquivamento (independente).
Maxsys.Bootstrap       ← componentes Bootstrap 5.3 p/ MVC (independente): TagHelpers <bs-*> (conteúdo projetado)
                         + ViewComponents <vc:bs-*> (data-driven; requer AddMaxsysBootstrap()). Customização global
                         via classes *Defaults; padrão: 1 arquivo por família (TagHelper + filhos + enums + Defaults).
```

Regra de ouro: **`Maxsys.Core` não conhece EF Core nem ASP.NET**. Abstrações (`IRepository`, `IUnitOfWork`) vivem em `Maxsys.Core/Interfaces`; implementações concretas vivem em `Maxsys.Data`.

**Namespaces vs nomes de pacote:** `Maxsys.Data`/`Maxsys.Web`/`Maxsys.Excel` **não** são namespaces-filho de `Maxsys.Core`, então não enxergam os tipos-raiz (`OperationResult`, `ListCriteria`, `Pagination`) implicitamente — cada um tem `<Using Include="Maxsys.Core" />` no `.csproj` (Web também tem `Maxsys.Core.Web`). Ao criar projeto novo que consuma o Core, replique isso.

## Padrões centrais (entender antes de mexer)

- **`OperationResult` / `OperationResult<T>`** (`src/Maxsys.Core/Common/`) é o retorno padrão de operações. Carrega `Notification`s com `ResultTypes`; `IsValid` é falso com severidade ≤ Warning. **Criação nova deve usar a factory estática `Result`** (`Result.Success()`, `Result.Error(msg)`, `Result.FromException(ex)`...). Os construtores com mensagem/exception estão `[Obsolete]` em `OperationResult.Ctors.cs` — ainda usados internamente (warnings **CS0618** no build são esperados e benignos).

- **Família `ModelServiceBase`** (`Services/`): serviços entity-centric em 2 níveis — `ModelServiceBase<TEntity, TRepository>` (leitura + eventos async de consulta) e `ModelServiceBase<TEntity, TRepository, TKey>` (CRUD com UoW + `IObjectMapper`, requer `IdSelector`). Interfaces `IModelService*` em `Interfaces/Services/`. Eventos de ciclo de vida (`AddingAsync`, `AddedAsync`, ...) para cross-cutting. Validação (FluentValidation) é aplicada nos handlers/pipeline, não numa variante do service.

- **Mapeamento é abstraído** (`Interfaces/Mapping/`): `IObjectMapper` (instâncias; usado pelo `ModelServiceBase`) e `IQueryProjector` (projeção de `IQueryable`; usado por `RepositoryBase`/`JoinRepositoryBase` via chokepoints `ApplyProjection`/`ApplyJoinProjection`). A implementação AutoMapper vive em `Maxsys.Mapping.AutoMapper` (`AddMaxsysAutoMapper<TEntry>()`, com scan de Profiles). NÃO referenciar AutoMapper em Core/Data.

- **Filtragem é SÓ via `ColumnFilter`** (`Filtering/ColumnFilter.cs`, modos PrimeNG-style). O specification pattern antigo (`IFilter`/`FilterBase`) foi **removido** na v17 — não reintroduzir. `[Searchable]` em props de DTO habilita busca textual global via `ApplySearch`.

- **Ordenação é SÓ por `Field` string** (`SortFilter`). Sort por enum/byte foi removido; `ApplySort` lança exceção se receber `SortFilter` sem `Field`. Default sort via `[DefaultSort]` na classe.

- **Listagem** (`Listing/`): `ListCriteria` (paginação + sorts + filtros + search) e `Pagination`. `ListDTO<T>` (Count + Items) e `InfoDTO<TKey>` (Id + Description, para dropdowns).

- **Mensageria** (`Maxsys.Messaging`): mediador próprio — `IBus` → `MaxsysBus` → `MaxsysMediator` (reflexão com cache). Registro via `AddMessaging<TEntry>()`. `ValidationBehavior` aplica FluentValidation no pipeline. **Não adicionar MediatR.**

- **C# moderno em uso**: o código usa `extension` members (C# 14), `required`, `init`, collection expressions `[]`, file-scoped namespaces. Siga o estilo — não converta extension blocks para static classes clássicas.

- **Famílias de genéricos numeradas por aridade**: `ModelServiceBase_2.cs`, `IRepository'1.cs`, etc. O sufixo = nº de parâmetros de tipo. Ao alterar assinatura de uma família, propague em todas as variantes.

- Warnings benignos conhecidos no build: **CS0618** (ctors obsoletos do OperationResult) e **CS0114** (hiding intencional no `JoinRepositoryBase`). Zero erros é o esperado; esses warnings não são regressão.

## Convenções

- Nullable e ImplicitUsings habilitados globalmente (via `Directory.Build.props`).
- XML docs ativados — APIs públicas têm `<summary>` em **pt-BR**; mantenha idioma e estilo.
- Arquivos `.cs` em **UTF-8** (sem Latin-1 — houve migração de encoding na v17; não salvar em ANSI).
- `Maxsys.Core.csproj` faz `<Using Include="Maxsys.Core" />` global.
- Formatação regida por `.editorconfig` na raiz.
- **Proibido** mencionar nomes de outras empresas em código, docs ou metadados. Os ícones são `logo.ico`/`logo.png` de cada `_PackageAssets/`.
