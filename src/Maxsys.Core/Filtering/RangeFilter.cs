namespace Maxsys.Core.Filtering;

public class RangeFilter<T>
{
    public RangeFilter()
    { }

    public RangeFilter(T? minValue, T? maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public T? MinValue { get; set; }
    public T? MaxValue { get; set; }
}