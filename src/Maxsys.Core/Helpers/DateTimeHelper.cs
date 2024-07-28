namespace Maxsys.Core.Helpers;

/// <summary>
/// Fornece métodos de ajuda para <see cref="DateTime"/>.
/// </summary>
public static class DateTimeHelper
{
    /// <summary>
    /// Converte uma data em formato UNIX (long) para DateTime (UTC)
    /// </summary>
    /// <param name="timestamp">data em formato UNIX (milisegundos a partir de 1970-01-01)</param>
    /// <returns></returns>
    public static DateTime FromUnixTimestamp(long timestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);

        return dateTimeOffset.UtcDateTime;
    }

    /// <summary>
    /// Converte um DateTime para uma data em formato UNIX.
    /// </summary>
    /// <param name="dateTime">data a ser convertida.</param>
    /// <returns></returns>
    public static long ToUnixTimestamp(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// Obtém uma nova instância da mesma data com horário 00:00:00
    /// </summary>
    /// <param name="date"></param>
    public static DateTime StartDate(DateTime date) => date.Date;

    /// <summary>
    /// Obtém uma nova instância da mesma data com horário 23:59:59.9999...
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime EndDate(DateTime date) => date.Date.AddDays(1).AddTicks(-1);
}