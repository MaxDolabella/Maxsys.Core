using System.Text;

namespace Maxsys.Core.Helpers;


/// <summary>
/// Provides extension methods for strings
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// Retorna um texto a partir de outro texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static string? GetTextOrNullIfEmpty(this string? text)
    {
        return string.IsNullOrWhiteSpace(text) ? null : text;
    }

    /// <summary>
    /// Retorna um número decimal a partir de um texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static decimal? GetDecimalOrNullIfEmpty(this string? text)
    {
        return !string.IsNullOrWhiteSpace(text)
            ? decimal.TryParse(text!, out decimal value) ? value : default(decimal?)
            : null;
    }

    /// <summary>
    /// Retorna uma data a partir de um texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static DateTime? GetDateTimeOrNullIfEmpty(this string? text)
    {
        return !string.IsNullOrWhiteSpace(text)
            ? DateTime.TryParse(text, out DateTime dateTime) ? dateTime : default(DateTime?)
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


    /// <summary>
    /// Return the text with the first letter in low cap.<para/>
    /// "FirstLetterLowCap" will return "firstLetterLowCap"
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string FirstLetterLowCap(this string text)
    {
        return string.IsNullOrEmpty(text)
            ? string.Empty
            : text.Length == 1
                ? text.ToLower()
                : string.Concat(text[0].ToString().ToLower(), text.AsSpan(1));
    }

    /// <summary>
    /// Replaces strings based on a Dictionary
    /// </summary>
    /// <param name="contents"></param>
    /// <param name="replacementDictionary"></param>
    /// <returns></returns>
    public static string DictionaryBasedReplacement(this string contents
        , Dictionary<string, string> replacementDictionary)
    {
        var sortedDic = new SortedDictionary<string, string>(replacementDictionary);
        for (int i = sortedDic.Count - 1; i >= 0; i--)
        {
            var item = sortedDic.ElementAt(i);
            contents = contents.Replace(item.Key, item.Value);
        }

        foreach (var key in replacementDictionary.Keys)
            contents = contents.Replace(key, replacementDictionary[key]);

        return contents;
    }

}