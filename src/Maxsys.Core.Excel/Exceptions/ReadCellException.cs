using ClosedXML.Excel;

namespace Maxsys.Core.Excel.Exceptions;

/// <summary>
/// Representa um erro que ocorre ao tentar ler um <see cref="IXLCell"/> não inicializado.
/// </summary>
public sealed class ReadCellException : Exception
{
    private const string DEFAULT_MESSAGE = "Error while reading cell {0}.";

    /// <summary>
    /// Representa um erro que ocorre ao tentar ler um <see cref="IXLCell"/> não inicializado.
    /// </summary>
    public ReadCellException(IXLCell cell)
      : base(string.Format(DEFAULT_MESSAGE, cell.Address.ToString()), null)
    { }

    /// <summary>
    /// Representa um erro que ocorre ao tentar ler um <see cref="IXLCell"/> não inicializado.
    /// </summary>
    public ReadCellException(IXLCell cell, Exception innerException)
      : base(string.Format(DEFAULT_MESSAGE, cell.Address.ToString()), innerException)
    { }
}