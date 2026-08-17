namespace Maxsys.Core.Extensions;

public static class DateTimeExtensions
{
    extension(DateTime dateTime)
    {
        /// <summary>
        /// Checa se uma data está entre duas datas (modo inclusivo).
        /// </summary>
        public bool IsBetween(DateTime initialDate, DateTime endDate)
            => dateTime >= initialDate && dateTime <= endDate;
    }
}
