namespace Maxsys.Core.Filtering;

public class PeriodFilter : RangeFilter<DateTime?>
{
    #region CTOR

    public PeriodFilter()
    { }

    public PeriodFilter(DateTime? minValue, DateTime? maxValue)
        : base(minValue, maxValue)
    { }

    #endregion CTOR
}

public class PeriodFilter<TDateTypeFilter> : PeriodFilter
    where TDateTypeFilter : Enum
{
    #region CTOR

    public PeriodFilter()
        : base()
    { }

    public PeriodFilter(DateTime? minValue, DateTime? maxValue, TDateTypeFilter dateType)
        : base(minValue, maxValue)
    {
        DateType = dateType;
    }

    #endregion CTOR

    public required TDateTypeFilter DateType { get; set; }
}