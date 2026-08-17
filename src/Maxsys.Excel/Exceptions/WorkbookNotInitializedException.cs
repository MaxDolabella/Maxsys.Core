using ClosedXML.Excel;
using Maxsys.Core.Exceptions;

namespace Maxsys.Excel.Exceptions;

/// <summary>
/// Representa um erro que ocorre ao tentar ler um <see cref="XLWorkbook"/> não inicializado.
/// </summary>
public sealed class WorkbookNotInitializedException : DomainException
{
    private const string DEFAULT_MESSAGE = "Not initialized Workbook. Method Initialize(Stream) must be called before use XLWorkbook property.";

    /// <summary>
    /// Representa um erro que ocorre ao tentar ler um <see cref="XLWorkbook"/> não inicializado.
    /// </summary>
    public WorkbookNotInitializedException()
      : base(DEFAULT_MESSAGE)
    { }
}