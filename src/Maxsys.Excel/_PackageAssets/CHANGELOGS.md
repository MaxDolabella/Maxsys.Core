# Maxsys.Excel

## 17.0.0
* :warning: Pacote renomeado de `Maxsys.Core.Excel` para `Maxsys.Excel` (PackageId e namespace raiz);
* :warning: Interface de entry point renomeada de `ICoreExcelEntry` para `IExcelEntry`; removida a classe obsoleta `Entry`;
* :warning: Atualização de framework (`.NET 10`);
* :warning: `IWorkbookFacade`/`WorkbookFacade`:
	* `Initialize(Stream, maxFileSizeBytes)` com validação de tamanho de arquivo (padrão 50 MB) e proteção contra dupla inicialização;
	* `ReadTable<TDestination>`/`ReadData<TDestination>` agora exigem `where TDestination : new()`;
* :sparkles: `ExcelCellDataType`: novos tipos `Float`, `TimeOnly` e `DateTimeOffset`;
* :sparkles: `TableTypeBuilder<T>`:
	* Parâmetro `format` em `CreateMap` para parse de datas/horas em células de texto;
	* `CreateEnumMap` com dicionário de aliases (case-insensitive);
	* Validação de mapeamento: coluna duplicada ou fora de sequência lança `ArgumentException`;
* :sparkles: Novas mensagens: `ERROR_EXCEL_FILE_TOO_LARGE`, `ERROR_EXCEL_ALREADY_INITIALIZED` e `ERROR_EXCEL_NOT_INITIALIZED`;
* :warning: Mensagem renomeada: `ERROR_EXCEL_EMPTY_SPREDSHEET` -> `ERROR_EXCEL_EMPTY_SPREADSHEET`;
* :triangular_flag_on_post: `AddTableTypeConfiguration<TImplementation, T>` obsoleto (erro de compilação) — utilizar `AddTableTypeConfigurations`;

---
## 16.0.0
* :package: Atualização de pacotes;
* :warning: Exception renomeada: `NotInitializedWorkbookException` -> `WorkbookNotInitializedException`;

---
## 14.0.0
* :warning: Atualização de dependências;

---
## 13.0.0
* :warning: Atualização de framework (`.NET 9`);
* :warning: Atualização de dependências;

---
## 11.0.0
* :warning: Atualização de dependências;

---
## 10.1.0
* Primeiro release.


<style>
  .warning { color: DarkGoldenRod; }
  h1 { color: Snow; }
  h2 { color: Crimson; }
  h3 { color: SteelBlue; }
  h4 { color: SeaGreen; }
</style>
