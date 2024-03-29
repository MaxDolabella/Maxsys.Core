using System.Reflection;
using ClosedXML.Excel;
using Maxsys.Core.Events;
using Maxsys.Core.Excel.Abstractions;
using Maxsys.Core.Excel.Exceptions;
using Maxsys.Core.Excel.Infra;
using Maxsys.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Maxsys.Core.Excel.Services;

/// <inheritdoc cref="IWorkbookFacade"/>
public class WorkbookFacade : ServiceBase, IWorkbookFacade
{
    #region Fields

    protected readonly ILogger _logger;
    protected readonly IServiceProvider _serviceProvider;
    private XLWorkbook? _workbook;

    #endregion Fields

    public WorkbookFacade(ILogger<WorkbookFacade> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    #region Properties

    protected IReadOnlyCollection<IXLTable> Tables => _workbook?.Worksheets.SelectMany(x => x.Tables).ToList() ?? [];
    protected XLWorkbook Workbook => _workbook ?? throw new NotInitializedWorkbookException();

    #endregion Properties

    #region Events

    public event EventHandler? WorkbookInitialized;

    public event EventHandler? ReadingTable;

    public event EventHandler? ReadingData;

    public event EventHandler<ValueEventArgs>? TableReaded;

    public event EventHandler<ValueEventArgs>? DataReaded;

    public event OperationResultEventHandler<object>? ItemReaded;

    #endregion Events

    #region Protected Methods

    protected virtual OperationResult<IEnumerable<TDestination>?> ReadTable<TDestination>(IXLTable table)
    {
        var tableDataRange = table.DataRange;

        var configuration = _serviceProvider.GetRequiredService<TableTypeConfigurationBase<TDestination>>();

        // EVENT: ReadingTable
        ReadingTable?.Invoke(this, EventArgs.Empty);

        var items = new List<TDestination>();
        for (var rowNumber = 1; rowNumber <= tableDataRange.RowCount(); rowNumber++)
        {
            var item = Activator.CreateInstance<TDestination>();

            var row = tableDataRange.Row(rowNumber);
            if (!row.IsEmpty())
            {
                foreach (var config in configuration.Configs)
                {
                    var expression = config.Property;
                    var xlCell = row.Cell(config.ColumnNumber);
                    var xlValue = xlCell.CachedValue;

                    try
                    {
                        object? value = GetValue(xlCell, config.DataType);

                        if (config.CustomConversion is not null)
                        {
                            var o = config.CustomConversion.Invoke(value);
                            (expression.Member as PropertyInfo)?.SetValue(item, o, null);
                        }
                        else
                            (expression.Member as PropertyInfo)?.SetValue(item, value, null);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                // EVENT: ItemReaded
                var itemReadedResult = ItemReaded?.Invoke(this, item!);
                if (itemReadedResult?.IsValid == false)
                {
                    return itemReadedResult.Cast<IEnumerable<TDestination>?>();
                }

                items.Add(item);
            }
        }

        // EVENT: TableReaded
        TableReaded?.Invoke(this, new ValueEventArgs(items));

        return new(items);
    }

    protected virtual OperationResult<IEnumerable<TDestination>?> ReadData<TDestination>(IXLRange dataRange)
    {
        var configuration = _serviceProvider.GetRequiredService<TableTypeConfigurationBase<TDestination>>();

        // event
        ReadingData?.Invoke(this, EventArgs.Empty);

        var items = new List<TDestination>();
        for (var rowNumber = 1; rowNumber <= dataRange.RowCount(); rowNumber++)
        {
            var item = Activator.CreateInstance<TDestination>();

            var row = dataRange.Row(rowNumber);
            if (!row.IsEmpty())
            {
                foreach (var config in configuration.Configs)
                {
                    var expression = config.Property;
                    var xlCell = row.Cell(config.ColumnNumber);
                    var xlValue = xlCell.CachedValue;

                    try
                    {
                        object? value = GetValue(xlCell, config.DataType);

                        //(expression.Member as PropertyInfo)?.SetValue(item, config.CustomConversion?.Invoke(value) ?? value, null);

                        if (config.CustomConversion is not null)
                        {
                            var o = config.CustomConversion.Invoke(value);
                            (expression.Member as PropertyInfo)?.SetValue(item, o, null);
                        }
                        else
                            (expression.Member as PropertyInfo)?.SetValue(item, value, null);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                items.Add(item);
            }
        }

        // event
        DataReaded?.Invoke(this, new ValueEventArgs(items));

        return new(items);
    }

    /// <exception cref="ReadCellException"></exception>
    protected static object? GetValue(IXLCell cell, ExcelCellDataType destinationType)
    {
        var xlValue = cell.CachedValue;

        object? value;

        try
        {
            value = (destinationType, cell.DataType) switch
            {
                (_, XLDataType.Blank) => null,
                (ExcelCellDataType.Text, _) => xlValue.ToString().Trim(),
                (ExcelCellDataType.Integer, XLDataType.Text) => Convert.ToInt32(xlValue.ToString().Trim()),
                (ExcelCellDataType.Integer, XLDataType.Number) => Convert.ToInt32(xlValue.GetNumber()),
                (ExcelCellDataType.Double, XLDataType.Text) => Convert.ToDouble(xlValue.ToString().Trim()),
                (ExcelCellDataType.Double, XLDataType.Number) => xlValue.GetNumber(),
                (ExcelCellDataType.Date, XLDataType.Text) => DateOnly.ParseExact(xlValue.ToString().Trim(), "dd'/'MM'/'yyyy"),
                (ExcelCellDataType.Date, XLDataType.Number) => DateOnly.FromDateTime(DateTime.FromOADate(xlValue.GetNumber())),
                (ExcelCellDataType.Date, XLDataType.DateTime) => DateOnly.FromDateTime(xlValue.GetDateTime()),
                (ExcelCellDataType.DateTime, XLDataType.Text) => DateTime.Parse(xlValue.ToString().Trim()),
                (ExcelCellDataType.DateTime, XLDataType.Number) => DateTime.FromOADate(xlValue.GetNumber()),
                (ExcelCellDataType.DateTime, XLDataType.DateTime) => xlValue.GetDateTime(),
                (ExcelCellDataType.Guid, XLDataType.Text) => Guid.Parse(xlValue.ToString().Trim()),
                (ExcelCellDataType.Decimal, XLDataType.Text) => Convert.ToDecimal(xlValue.ToString().Trim()),
                (ExcelCellDataType.Decimal, XLDataType.Number) => Convert.ToDecimal(xlValue.GetNumber()),
                (ExcelCellDataType.Long, XLDataType.Text) => Convert.ToInt64(xlValue.ToString().Trim()),
                (ExcelCellDataType.Long, XLDataType.Number) => Convert.ToInt64(xlValue.GetNumber()),
                (ExcelCellDataType.Short, XLDataType.Text) => Convert.ToInt16(xlValue.ToString().Trim()),
                (ExcelCellDataType.Short, XLDataType.Number) => Convert.ToInt16(xlValue.GetNumber()),
                (ExcelCellDataType.Byte, XLDataType.Text) => Convert.ToByte(xlValue.ToString().Trim()),
                (ExcelCellDataType.Byte, XLDataType.Number) => Convert.ToByte(xlValue.GetNumber()),
                (ExcelCellDataType.Boolean, XLDataType.Boolean) => xlValue.GetBoolean(),
                _ => throw new InvalidCastException($"Formato inválido."),
            };
        }
        catch (Exception ex)
        {
            throw new ReadCellException(cell, ex);
        }

        return value;
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        if (disposing)
        {
            // NOTE: dispose managed state (managed objects).
            _workbook?.Dispose();
        }
        // NOTE: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // NOTE: set large fields to null.
        _disposed = true;
    }

    #endregion Protected Methods

    #region Public

    public virtual OperationResult Initialize(Stream file)
    {
        OperationResult result = new();
        try
        {
            using (file)
                _workbook = new XLWorkbook(file);

            WorkbookInitialized?.Invoke(this, EventArgs.Empty);
        }
        catch (FileFormatException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);

            result.AddNotification(new(ex, Messages.ERROR_FILE_INVALID) { Details = ex.Message });
        }
        catch (DocumentFormat.OpenXml.Packaging.OpenXmlPackageException ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);

            result.AddNotification(new(ex, Messages.ERROR_FILE_INVALID) { Details = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{message}", ex.Message);

            result.AddNotification(new(ex, Messages.ERROR_FILE_INVALID) { Details = ex.Message });
        }

        return result;
    }

    public virtual OperationResult<IEnumerable<TDestination>?> ReadTable<TDestination>(string? tableName = null)
    {
        try
        {
            var table = string.IsNullOrWhiteSpace(tableName)
                ? Tables.FirstOrDefault()
                : Tables.FirstOrDefault(x => x.Name.Equals(tableName, StringComparison.CurrentCultureIgnoreCase));

            return table is null
                ? new(Messages.ERROR_EXCEL_TABLE_NOT_FOUND)
                : ReadTable<TDestination>(table);
        }
        catch (Exception ex)
        {
            return new(new Notification(ex, Messages.ERROR_EXCEL_READ_OBJECTS));
        }
    }

    public virtual OperationResult<IEnumerable<TDestination>?> ReadData<TDestination>(int worksheetPosition = 1)
    {
        try
        {
            var worksheet = Workbook.Worksheet(worksheetPosition);

            var firstCellUsed = worksheet?.FirstCellUsed();
            return firstCellUsed is null
                ? new(Messages.ERROR_EXCEL_EMPTY_SPREDSHEET)
                : ReadData<TDestination>(firstCellUsed.CurrentRegion);
        }
        catch (ReadCellException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return new(new Notification(ex, Messages.ERROR_EXCEL_READ_OBJECTS));
        }
    }

    #endregion Public
}