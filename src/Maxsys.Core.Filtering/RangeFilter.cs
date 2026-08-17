namespace Maxsys.Core.Filtering;

public class RangeFilter<T>
{
    #region CTOR

    public RangeFilter()
    { }

    public RangeFilter(T? minValue, T? maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    #endregion CTOR

    public T? MinValue { get; set; } = default;
    public T? MaxValue { get; set; } = default;
}