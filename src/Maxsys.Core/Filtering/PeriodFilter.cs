using Maxsys.Core.Helpers;

namespace Maxsys.Core.Filtering;

public class PeriodFilter : RangeFilter<DateTime?>
{
    #region CTOR

    public PeriodFilter()
    { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="uses00h00To23h59">Caso <see langword="true"/>, horas de <paramref name="minValue"/> e <paramref name="maxValue"/> serão 00:00:00 e 23:59:59.9999 respectivamente.</param>
    public PeriodFilter(DateTime? minValue, DateTime? maxValue, bool uses00h00To23h59 = false) : base(
        uses00h00To23h59 && minValue.HasValue ? DateTimeHelper.StartDate(minValue.Value) : minValue,
        uses00h00To23h59 && maxValue.HasValue ? DateTimeHelper.EndDate(maxValue.Value) : maxValue
        )
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