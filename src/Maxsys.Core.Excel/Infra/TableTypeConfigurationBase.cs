namespace Maxsys.Core.Excel.Infra;

public abstract class TableTypeConfigurationBase<T>
{
    protected TableTypeConfigurationBase()
    {
        var builder = new TableTypeBuilder<T>();

        Configure(builder);

        Configs = builder.Build();
    }

    internal List<ExcelTableCellConfig<T>> Configs { get; set; } = [];

    public abstract void Configure(TableTypeBuilder<T> builder);
}