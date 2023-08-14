using System.Linq.Expressions;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Extensions;

public static class DateTimeExtensions
{
    public static Expression<Func<DateTime, PeriodFilter, bool>> IsBetweenExpression
        => (dateTime, period)
            => (period.MinValue == null || dateTime >= period.MinValue)
            && (period.MaxValue == null || dateTime <= period.MaxValue);

    /// <summary>
    /// Checa se uma data está entre duas datas (modo inclusivo).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="initialDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public static bool IsBetween(this DateTime dateTime, DateTime initialDate, DateTime endDate)
        => IsBetween(dateTime, new PeriodFilter(initialDate, endDate));

    /// <summary>
    /// Checa se uma data está entre um perído (modo inclusivo).
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="period"></param>
    /// <returns></returns>
    public static bool IsBetween(this DateTime dateTime, PeriodFilter period)
    {
        return IsBetweenExpression.Compile().Invoke(dateTime, period);
    }
}