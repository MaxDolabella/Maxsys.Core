using System.Globalization;
using System.Reflection;
using ClosedXML.Excel;
using Maxsys.Core.Events;
using Maxsys.Excel.Abstractions;
using Maxsys.Excel.Exceptions;
using Maxsys.Excel.Infra;
using Maxsys.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Maxsys.Excel.Services;

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
    protected XLWorkbook Workbook => _workbook ?? throw new WorkbookNotInitializedException();

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

    protected virtual OperationResult<IEnumerable<TDestination>?> ReadTable<TDestination>(IXLTable table) where TDestination : new()
    {
        var result = new OperationResult<IEnumerable<TDestination>?>();
        var tableDataRange = table.DataRange;

        var configuration = _serviceProvider.GetRequiredService<TableTypeConfigurationBase<TDestination>>();

        // EVENT: ReadingTable
        ReadingTable?.Invoke(this, EventArgs.Empty);

        var items = new List<TDestination>();
        for (var rowNumber = 1; rowNumber <= tableDataRange.RowCount(); rowNumber++)
        {
            var item = new TDestination();

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
                        object? value = GetValue(xlCell, config.DataType, config.Format);

                        if (config.CustomConversion is not null)
                        {
                            var o = config.CustomConversion.Invoke(value);
                            (expression.Member as PropertyInfo)?.SetValue(item, o, null);
                        }
                        else
                            (expression.Member as PropertyInfo)?.SetValue(item, value, null);
                    }
                    catch (ReadCellException ex)
                    {
                        result.AddNotification(new Notification(Messages.ERROR_EXCEL_CELL_READ, ex.Message) { Tag = ex });
                    }
                }

                if (result.IsValid)
                {
                    // EVENT: ItemReaded
                    var itemReadedResult = ItemReaded?.Invoke(this, item!);
                    if (itemReadedResult?.IsValid == false)
                    {
                        return itemReadedResult.Cast<IEnumerable<TDestination>?>();
                    }

                    items.Add(item);
                }
            }
        }

        if (result.IsValid)
        {
            result.Data = items;

            // EVENT: TableReaded
            TableReaded?.Invoke(this, new ValueEventArgs(items));
        }

        return result;
    }

    protected virtual OperationResult<IEnumerable<TDestination>?> ReadData<TDestination>(IXLRange dataRange) where TDestination : new()
    {
        var result = new OperationResult<IEnumerable<TDestination>?>();
        var configuration = _serviceProvider.GetRequiredService<TableTypeConfigurationBase<TDestination>>();

        // event
        ReadingData?.Invoke(this, EventArgs.Empty);

        var items = new List<TDestination>();
        for (var rowNumber = 1; rowNumber <= dataRange.RowCount(); rowNumber++)
        {
            var item = new TDestination();

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
                        object? value = GetValue(xlCell, config.DataType, config.Format);

                        if (config.CustomConversion is not null)
                        {
                            var o = config.CustomConversion.Invoke(value);
                            (expression.Member as PropertyInfo)?.SetValue(item, o, null);
                        }
                        else
                            (expression.Member as PropertyInfo)?.SetValue(item, value, null);
                    }
                    catch (ReadCellException ex)
                    {
                        result.AddNotification(new Notification(Messages.ERROR_EXCEL_CELL_READ, ex.Message) { Tag = ex });
                    }
                }

                if (result.IsValid)
                    items.Add(item);
            }
        }

        if (result.IsValid)
        {
            result.Data = items;

            // event
            DataReaded?.Invoke(this, new ValueEventArgs(items));
        }

        return result;
    }

    /// <exception cref="ReadCellException"></exception>
    protected static object? GetValue(IXLCell cell, ExcelCellDataType destinationType, string? format = null)
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
                (ExcelCellDataType.Date, XLDataType.Text) => DateOnly.ParseExact(xlValue.ToString().Trim(), format ?? "dd'/'MM'/'yyyy"),
                (ExcelCellDataType.Date, XLDataType.Number) => DateOnly.FromDateTime(DateTime.FromOADate(xlValue.GetNumber())),
                (ExcelCellDataType.Date, XLDataType.DateTime) => DateOnly.FromDateTime(xlValue.GetDateTime()),
                (ExcelCellDataType.DateTime, XLDataType.Text) when format is not null => DateTime.ParseExact(xlValue.ToString().Trim(), format, CultureInfo.InvariantCulture),
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
                (ExcelCellDataType.Float, XLDataType.Text) => Convert.ToSingle(xlValue.ToString().Trim(), CultureInfo.InvariantCulture),
                (ExcelCellDataType.Float, XLDataType.Number) => Convert.ToSingle(xlValue.GetNumber()),
                (ExcelCellDataType.TimeOnly, XLDataType.Text) => TimeOnly.ParseExact(xlValue.ToString().Trim(), format ?? "HH':'mm':'ss"),
                (ExcelCellDataType.TimeOnly, XLDataType.DateTime) => TimeOnly.FromDateTime(xlValue.GetDateTime()),
                (ExcelCellDataType.TimeOnly, XLDataType.Number) => TimeOnly.FromDateTime(DateTime.FromOADate(xlValue.GetNumber())),
                (ExcelCellDataType.DateTimeOffset, XLDataType.Text) when format is not null => DateTimeOffset.ParseExact(xlValue.ToString().Trim(), format, CultureInfo.InvariantCulture),
                (ExcelCellDataType.DateTimeOffset, XLDataType.Text) => DateTimeOffset.Parse(xlValue.ToString().Trim()),
                (ExcelCellDataType.DateTimeOffset, XLDataType.DateTime) => new DateTimeOffset(xlValue.GetDateTime()),
                (ExcelCellDataType.DateTimeOffset, XLDataType.Number) => new DateTimeOffset(DateTime.FromOADate(xlValue.GetNumber())),
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

    public virtual OperationResult Initialize(Stream file, long? maxFileSizeBytes = 52_428_800)
    {
        OperationResult result = new();

        if (_workbook is not null)
        {
            result.AddNotification(new(Messages.ERROR_EXCEL_ALREADY_INITIALIZED));
            return result;
        }

        try
        {
            if (maxFileSizeBytes.HasValue && file.CanSeek && file.Length > maxFileSizeBytes.Value)
            {
                result.AddNotification(new(Messages.ERROR_EXCEL_FILE_TOO_LARGE)
                {
                    Details = $"O arquivo possui {file.Length} bytes, mas o limite máximo é {maxFileSizeBytes.Value} bytes."
                });
                return result;
            }

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

    public virtual OperationResult<IEnumerable<TDestination>?> ReadTable<TDestination>(string? tableName = null) where TDestination : new()
    {
        if (_workbook is null)
            return Result.Error<IEnumerable<TDestination>?>(Messages.ERROR_EXCEL_NOT_INITIALIZED);

        try
        {
            var table = string.IsNullOrWhiteSpace(tableName)
                ? Tables.FirstOrDefault()
                : Tables.FirstOrDefault(x => x.Name.Equals(tableName, StringComparison.CurrentCultureIgnoreCase));

            return table is null
                ? Result.Error<IEnumerable<TDestination>?>(Messages.ERROR_EXCEL_TABLE_NOT_FOUND)
                : ReadTable<TDestination>(table);
        }
        catch (Exception ex)
        {
            return Result.FromException<IEnumerable<TDestination>?>(ex, Messages.ERROR_EXCEL_READ_OBJECTS);
        }
    }

    public virtual OperationResult<IEnumerable<TDestination>?> ReadData<TDestination>(int worksheetPosition = 1) where TDestination : new()
    {
        if (_workbook is null)
            return Result.Error<IEnumerable<TDestination>?>(Messages.ERROR_EXCEL_NOT_INITIALIZED);

        try
        {
            var worksheet = _workbook.Worksheet(worksheetPosition);

            var firstCellUsed = worksheet?.FirstCellUsed();
            return firstCellUsed is null
                ? Result.Error<IEnumerable<TDestination>?>(Messages.ERROR_EXCEL_EMPTY_SPREADSHEET)
                : ReadData<TDestination>(firstCellUsed.CurrentRegion);
        }
        catch (Exception ex)
        {
            return Result.FromException<IEnumerable<TDestination>?>(ex, Messages.ERROR_EXCEL_READ_OBJECTS);
        }
    }

    #endregion Public
}