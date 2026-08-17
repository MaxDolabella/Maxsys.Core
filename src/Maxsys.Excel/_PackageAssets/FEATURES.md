# Maxsys.Excel

Biblioteca Maxsys para leitura **tipada** de arquivos Excel (`.xlsx`/`.xlsm`) via ClosedXML: mapeamento configurável de colunas para propriedades (`TableTypeBuilder<T>`), conversão automática de 14 tipos de dados e acúmulo de erros por célula em `OperationResult` (sem exceções para o chamador).

## Abstrações

### IWorkbookFacade

Contrato do serviço de leitura de workbooks (deriva de `IService` de `Maxsys.Core`).

- `Initialize(Stream file, long? maxFileSizeBytes = 52_428_800)` — carrega o workbook a partir de um stream, com validação opcional de tamanho (padrão 50 MB).
- `ReadTable<TDestination>(string? tableName = null)` — lê uma tabela nomeada do Excel (ou a primeira encontrada) para `IEnumerable<TDestination>`.
- `ReadData<TDestination>(int worksheetPosition = 1)` — lê a região de dados usada de uma worksheet (sem tabela definida) para `IEnumerable<TDestination>`.
- Todos os retornos são `OperationResult`/`OperationResult<T>` — erros viram `Notification`, não exceções.
- `TDestination` exige construtor público sem parâmetros (`where TDestination : new()`).

## Serviços

### WorkbookFacade

Implementação padrão de `IWorkbookFacade` (deriva de `ServiceBase`). Resolve a configuração de mapeamento `TableTypeConfigurationBase<TDestination>` via `IServiceProvider` e converte célula a célula conforme o `ExcelCellDataType` configurado.

- `Initialize` rejeita: workbook já inicializado (`ERROR_EXCEL_ALREADY_INITIALIZED`), arquivo acima do limite (`ERROR_EXCEL_FILE_TOO_LARGE`) e formato inválido (`ERROR_FILE_INVALID`).
- `ReadTable`/`ReadData` sem `Initialize` prévio retornam `ERROR_EXCEL_NOT_INITIALIZED`; tabela inexistente retorna `ERROR_EXCEL_TABLE_NOT_FOUND`; worksheet vazia retorna `ERROR_EXCEL_EMPTY_SPREADSHEET`.
- Erros de conversão de célula são acumulados como `Notification` (`ERROR_EXCEL_CELL_READ`) com a `ReadCellException` no `Tag`.
- Eventos: `WorkbookInitialized`, `ReadingTable`, `ReadingData`, `TableReaded`, `DataReaded` e `ItemReaded` (`OperationResultEventHandler<object>` — permite validar/abortar a leitura item a item).
- `Dispose` descarta o `XLWorkbook` interno.

```csharp
// Registro (Program.cs)
services.AddTableTypeConfigurations<Program>();          // mapeamentos do assembly
services.AddWorkbookService<IWorkbookFacade, WorkbookFacade>();

// Fluxo de leitura
public async Task<OperationResult<IEnumerable<PersonImportModel>?>> ImportAsync(Stream file)
{
    using var facade = _serviceProvider.GetRequiredService<IWorkbookFacade>();

    var initResult = facade.Initialize(file, maxFileSizeBytes: 10 * 1024 * 1024);
    if (!initResult.IsValid)
        return initResult.Cast<IEnumerable<PersonImportModel>?>();

    // lê a tabela nomeada "People" (ou passe null para a primeira tabela do workbook)
    return facade.ReadTable<PersonImportModel>("People");

    // alternativa sem tabela definida: lê a região usada da 1ª worksheet
    // return facade.ReadData<PersonImportModel>(worksheetPosition: 1);
}
```

## Infra (Mapeamento)

### TableTypeConfigurationBase&lt;T&gt;

Classe base abstrata de configuração do mapeamento coluna → propriedade para o tipo `T`. Implemente `Configure(TableTypeBuilder<T> builder)`; a configuração é resolvida via DI pelo `WorkbookFacade`.

```csharp
public enum PersonStatus { Active, Inactive }

public class PersonImportModel
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal Salary { get; set; }
    public PersonStatus? Status { get; set; }
}

public sealed class PersonImportConfiguration : TableTypeConfigurationBase<PersonImportModel>
{
    public override void Configure(TableTypeBuilder<PersonImportModel> builder)
    {
        // colunas mapeadas em sequência (1, 2, 3...); tipo inferido da propriedade
        builder.CreateMap(x => x.Name);                               // coluna 1 (Text)
        builder.CreateMap(x => x.Age);                                // coluna 2 (Integer)
        builder.CreateMap(x => x.BirthDate, ExcelCellDataType.Date, "dd/MM/yyyy"); // coluna 3
        builder.CreateMap(x => x.Salary);                             // coluna 4 (Decimal)
        builder.CreateEnumMap(x => x.Status, new Dictionary<string, PersonStatus>
        {
            ["Ativo"] = PersonStatus.Active,
            ["Inativo"] = PersonStatus.Inactive,
        });                                                           // coluna 5 (aliases case-insensitive)
    }
}
```

### TableTypeBuilder&lt;T&gt;

Builder usado dentro de `TableTypeConfigurationBase<T>.Configure` para mapear colunas.

- `CreateMap(property)` — infere o `ExcelCellDataType` a partir do tipo da propriedade (inclusive `Nullable<>`): `string`, `bool`, `byte`, `short`, `int`, `long`, `float`, `double`, `decimal`, `Guid`, `DateOnly`, `DateTime`, `DateTimeOffset`, `TimeOnly`.
- `CreateMap(property, dataType)` / `CreateMap(property, dataType, format)` — tipo explícito, com `format` opcional para parse de datas/horas em células de texto.
- `CreateMap(property, columnNumber, dataType, customConversion?, format?)` — controle total, incluindo conversão customizada (`Func<object?, dynamic?>`).
- `CreateMap(property, customConversion)` — lê como texto e aplica a conversão customizada.
- `CreateEnumMap(property)` — converte texto em enum via `ToEnum<TValue>()` de `Maxsys.Core`.
- `CreateEnumMap(property, aliases)` — idem, com dicionário de aliases (case-insensitive) e fallback para `ToEnum`.
- Validações: coluna duplicada ou fora de sequência lança `ArgumentException` (mapeie 1, 2, 3... sem lacunas).

### ExcelCellDataType

Enum (`byte`) com os tipos de destino suportados na conversão de célula:
`Boolean`, `Byte`, `Date` (→ `DateOnly`), `DateTimeOffset`, `DateTime`, `Decimal`, `Double`, `Float`, `Guid`, `Integer`, `Long`, `Short`, `Text`, `TimeOnly`.

A conversão considera o tipo real da célula (`Text`, `Number`, `DateTime`, `Boolean`, `Blank`); células em branco resultam em `null` e formatos incompatíveis geram `ReadCellException`.

## Exceptions

### ReadCellException

Erro ao converter o valor de uma célula (`IXLCell`). A mensagem inclui o endereço da célula (ex.: `Error while reading cell B7.`) e a exceção original como `InnerException`. É capturada pelo `WorkbookFacade` e convertida em `Notification`.

### WorkbookNotInitializedException

`DomainException` lançada ao acessar a propriedade protegida `Workbook` antes de `Initialize(Stream)`.

## Mensagens

### Messages

Chaves de mensagens de erro do pacote (com `[Description]` em pt-BR): `ERROR_FILE_UPLOAD`, `ERROR_FILE_INVALID`, `ERROR_EXCEL_INVALID`, `ERROR_EXCEL_READ_OBJECTS`, `ERROR_EXCEL_FILE_TYPE`, `ERROR_EXCEL_NULL`, `ERROR_EXCEL_MISSING_SHEET`, `ERROR_EXCEL_MISSING_COLUMN`, `ERROR_EXCEL_TABLE_NOT_FOUND`, `ERROR_EXCEL_EMPTY_SPREADSHEET`, `ERROR_EXCEL_CELL_READ`, `ERROR_EXCEL_FILE_TOO_LARGE`, `ERROR_EXCEL_ALREADY_INITIALIZED`, `ERROR_EXCEL_NOT_INITIALIZED`.

## Extensions

### IoCExtensions

Registro dos componentes no container de DI.

- `AddTableTypeConfigurations<TEntry>(lifetime = Scoped)` — registra todas as `TableTypeConfigurationBase<T>` do assembly de `TEntry` (ignora tipos com `DependencyInjectionIgnoreAttribute`).
- `AddTableTypeConfigurations(Assembly[] assemblies, lifetime = Scoped)` — idem, para múltiplos assemblies.
- `AddWorkbookService<TService, TImplementation>()` / `AddWorkbookService<TImplementation>()` — registra a implementação de `IWorkbookFacade` (scoped).

```csharp
services
    .AddTableTypeConfigurations<IExcelEntry>()
    .AddWorkbookService<IWorkbookFacade, WorkbookFacade>();
```

## Infraestrutura

### IExcelEntry

Interface vazia usada como referência do assembly (ex.: para registro de tipos por assembly scanning).
