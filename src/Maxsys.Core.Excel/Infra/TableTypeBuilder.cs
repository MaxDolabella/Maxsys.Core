using System.Linq.Expressions;
using Maxsys.Core.Extensions;

namespace Maxsys.Core.Excel.Infra;

public sealed class TableTypeBuilder<T>
{
    private List<ExcelTableCellConfig<T>> _configurations = [];

    internal List<ExcelTableCellConfig<T>> Build() => _configurations;

    private void AddConfig(ExcelTableCellConfig<T> config)
    {
        _configurations.Add(config);
    }

    public void CreateMap<TValue>(Expression<Func<T, TValue>> property, int columnNumber, ExcelCellDataType dataType, Func<object?, dynamic?>? customConversion = null)
    {
        AddConfig(ExcelTableCellConfig<T>.Create(property, columnNumber, dataType, customConversion));
    }

    public void CreateMap<TValue>(Expression<Func<T, TValue>> property, ExcelCellDataType dataType)
    {
        CreateMap(property, GetNextColumnNumber(), dataType);
    }

    public void CreateMap<TValue>(Expression<Func<T, TValue>> property)
    {
        var type = typeof(TValue);
        var typeName = type.FullName!;
        var isNullable = typeName.StartsWith("System.Nullable`1");
        if (isNullable)
        {
            typeName = type.GenericTypeArguments[0].FullName;
        }

        var dataType = typeName switch
        {
            "System.Byte" => ExcelCellDataType.Byte,
            "System.Boolean" => ExcelCellDataType.Boolean,
            "System.DateOnly" => ExcelCellDataType.Date,
            "System.DateTime" => ExcelCellDataType.DateTime,
            "System.Double" => ExcelCellDataType.Double,
            "System.Guid" => ExcelCellDataType.Guid,
            "System.Int16" => ExcelCellDataType.Short,
            "System.Int32" => ExcelCellDataType.Integer,
            "System.Int64" => ExcelCellDataType.Long,
            "System.String" => ExcelCellDataType.Text,
            "System.Decimal" => ExcelCellDataType.Decimal,
            _ => throw new ArgumentException($"Property of type {typeName} has no mapping.")
        };

        CreateMap(property, dataType);
    }

    public void CreateMap<TValue>(Expression<Func<T, TValue>> property, Func<object?, dynamic?>? customConversion)
    {
        AddConfig(ExcelTableCellConfig<T>.Create(property, GetNextColumnNumber(), ExcelCellDataType.Text, customConversion));
    }

    public void CreateEnumMap<TValue>(Expression<Func<T, TValue?>> property) where TValue : struct, Enum
    {
        Type type = typeof(TValue);
        var isNullable = type.FullName?.StartsWith("System.Nullable`1") == true;
        if (isNullable)
        {
            type = type.GenericTypeArguments[0];
        }

        Func<object?, dynamic?>? customConversion = value => value?.ToString()?.ToEnum<TValue>();
        AddConfig(ExcelTableCellConfig<T>.Create(property, GetNextColumnNumber(), ExcelCellDataType.Text, customConversion));
    }

    private int GetNextColumnNumber() => _configurations.Count == 0 ? 1 : _configurations.Max(x => x.ColumnNumber) + 1;
}