using System.ComponentModel;

namespace Maxsys.Excel;

/// <summary>
/// Fornece mensagens de erro para o projeto atual.
/// </summary>
public static class Messages
{
    ///<summary>Ocorreu um erro ao importar arquivo.</summary>
    [Description("Ocorreu um erro ao importar arquivo.")]
    public static string ERROR_FILE_UPLOAD => nameof(ERROR_FILE_UPLOAD);

    ///<summary>Arquivo é inválido.</summary>
    [Description("Arquivo é inválido.")]
    public static string ERROR_FILE_INVALID => nameof(ERROR_FILE_INVALID);

    ///<summary>Arquivo excel inválido.</summary>
    [Description("Arquivo excel inválido.")]
    public static string ERROR_EXCEL_INVALID => nameof(ERROR_EXCEL_INVALID);

    ///<summary>Erro a ler linhas da planilha.</summary>
    [Description("Erro a ler linhas da planilha.")]
    public static string ERROR_EXCEL_READ_OBJECTS => nameof(ERROR_EXCEL_READ_OBJECTS);

    ///<summary>Arquivo selecionado precisa ser Excel 2007 ou mais novo (.xlsx, .xlsm).</summary>
    [Description("Arquivo selecionado precisa ser Excel 2007 ou mais novo (.xlsx, .xlsm).")]
    public static string ERROR_EXCEL_FILE_TYPE => nameof(ERROR_EXCEL_FILE_TYPE);

    ///<summary>Nenhum Arquivo Selecionado</summary>
    [Description("Nenhum Arquivo Selecionado.")]
    public static string ERROR_EXCEL_NULL => nameof(ERROR_EXCEL_NULL);

    ///<summary>Planilha não encontrada.</summary>
    [Description("Planilha não encontrada.")]
    public static string ERROR_EXCEL_MISSING_SHEET => nameof(ERROR_EXCEL_MISSING_SHEET);

    ///<summary>Coluna na planilha não encontrada.</summary>
    [Description("Coluna na planilha não encontrada.")]
    public static string ERROR_EXCEL_MISSING_COLUMN => nameof(ERROR_EXCEL_MISSING_COLUMN);

    ///<summary>Tabela não encontrada.</summary>
    [Description("Tabela não encontrada.")]
    public static string ERROR_EXCEL_TABLE_NOT_FOUND => nameof(ERROR_EXCEL_TABLE_NOT_FOUND);

    ///<summary>Planilha está vazia.</summary>
    [Description("Planilha está vazia.")]
    public static string ERROR_EXCEL_EMPTY_SPREADSHEET => nameof(ERROR_EXCEL_EMPTY_SPREADSHEET);

    ///<summary>Erro ao ler célula.</summary>
    [Description("Erro ao ler célula.")]
    public static string ERROR_EXCEL_CELL_READ => nameof(ERROR_EXCEL_CELL_READ);

    ///<summary>O arquivo excede o tamanho máximo permitido.</summary>
    [Description("O arquivo excede o tamanho máximo permitido.")]
    public static string ERROR_EXCEL_FILE_TOO_LARGE => nameof(ERROR_EXCEL_FILE_TOO_LARGE);

    ///<summary>O workbook já foi inicializado.</summary>
    [Description("O workbook já foi inicializado.")]
    public static string ERROR_EXCEL_ALREADY_INITIALIZED => nameof(ERROR_EXCEL_ALREADY_INITIALIZED);

    ///<summary>O workbook não foi inicializado.</summary>
    [Description("O workbook não foi inicializado.")]
    public static string ERROR_EXCEL_NOT_INITIALIZED => nameof(ERROR_EXCEL_NOT_INITIALIZED);
}