using ClosedXML.Excel;

namespace Maxsys.Core.Excel.Exceptions;

/// <summary>
/// Representa um erro que ocorre ao tentar ler um <see cref="XLWorkbook"/> não inicializado.
/// </summary>
public sealed class WorkbookNotInitializedException : Exception
{
    private const string DEFAULT_MESSAGE = "Not initialized Workbook. Method Initialize(Stream) must be called before use XLWorkbook property.";

    /// <summary>
    /// Representa um erro que ocorre ao tentar ler um <see cref="XLWorkbook"/> não inicializado.
    /// </summary>
    public WorkbookNotInitializedException()
      : base(DEFAULT_MESSAGE)
    { }
}