using System.IO;
using System.Text;

namespace Maxsys.Core.Helpers;

public static class StringHelper
{
    private static readonly char[] s_InvalidFileNameChars;

    static StringHelper()
    {
        var invalidChars = new List<char>();
        invalidChars.AddRange(Path.GetInvalidFileNameChars());
        invalidChars.AddRange(new char[] { (char)37, (char)127 }); // 37=%, 127=del

        s_InvalidFileNameChars = [.. invalidChars];
    }

    public static string? RemoveInvalidFileNameChars(this string? value)
    {
        if (value is null)
            return null;

        var newValue = string.Join("_", value.Split(s_InvalidFileNameChars)).Trim();
        return newValue.All(c => c == '_') ? string.Empty : newValue;
    }

    /// <summary>
    /// Retorna um texto a partir de outro texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static string? GetTextOrNullIfEmpty(this string? text)
    {
        return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
    }

    /// <summary>
    /// Retorna um número decimal a partir de um texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static decimal? GetDecimalOrNullIfEmpty(this string? text)
    {
        return !string.IsNullOrWhiteSpace(text)
            ? decimal.TryParse(text.Trim(), out decimal value) ? value : default(decimal?)
            : null;
    }

    /// <summary>
    /// Retorna uma data a partir de um texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static DateTime? GetDateTimeOrNullIfEmpty(this string? text)
    {
        return !string.IsNullOrWhiteSpace(text)
            ? DateTime.TryParse(text.Trim(), out DateTime dateTime) ? dateTime : default(DateTime?)
            : null;
    }

    /// <summary>
    /// Converte um array de bytes em uma string Hexadecimal.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToHexString(this byte[] bytes)
    {
        var sb = new StringBuilder();

        foreach (var @byte in bytes)
            sb.Append(@byte.ToString("x2"));

        return sb.ToString();
    }
}