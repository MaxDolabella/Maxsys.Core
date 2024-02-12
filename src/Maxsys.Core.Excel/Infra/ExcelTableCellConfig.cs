using System.Linq.Expressions;

namespace Maxsys.Core.Excel.Infra;

internal sealed class ExcelTableCellConfig<T>
{
    public static ExcelTableCellConfig<T> Create<TValue>(Expression<Func<T, TValue>> propertySelector, int columnNumber, ExcelCellDataType dataType, Func<object?, dynamic?>? customConversion = null)
    {
        return propertySelector.Body is MemberExpression memberExpression
            ? new ExcelTableCellConfig<T>
            {
                ColumnNumber = columnNumber,
                Property = memberExpression,
                DataType = dataType,
                CustomConversion = customConversion
            }
            : throw new ArgumentException("Expression inválida");
    }

    public required int ColumnNumber { get; set; }
    public required MemberExpression Property { get; set; }
    public required ExcelCellDataType DataType { get; set; }
    public Func<object?, dynamic?>? CustomConversion { get; set; }
}