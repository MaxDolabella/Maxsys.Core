using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Maxsys.Core.Helpers;

public static class StringHelper
{
    private static readonly char[] s_InvalidFileNameChars;
    public const string LOREM_IPSUM = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec tempus scelerisque egestas. Sed at sagittis velit. Vestibulum sed velit a metus faucibus mollis. Pellentesque porta mi neque, vitae tempor ante laoreet a. Praesent non elementum massa, vel lacinia lacus. Integer semper in enim eget condimentum. Donec a convallis ligula, eget sodales odio. Nulla odio quam, hendrerit sit amet eros at, mattis vestibulum orci. Duis id felis felis. Aenean congue a ex ultricies vehicula. Mauris eget tortor nibh. Sed et tincidunt magna, sit amet tempor urna. Aliquam erat volutpat. Fusce in sapien augue. Integer et urna ipsum. Sed vel felis vel mauris sagittis laoreet id ut magna. Ut faucibus feugiat ex ut volutpat. Ut vehicula, nulla ac vehicula lobortis, risus libero ultrices nulla, vel varius diam tortor a lacus. Sed dictum vitae libero ac lacinia. Cras porttitor semper dolor vel ornare. Donec sodales auctor orci, sed hendrerit lacus hendrerit nec. Sed tincidunt gravida leo, at facilisis ante ac.";

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

    /// <summary>
    /// Retorna uma string contendo apenas números.
    /// </summary>
    /// <param name="text"></param>
    public static string? GetOnlyNumbers(this string? text)
    {
        return text is not null
            ? new string(text.Where(char.IsDigit).ToArray())
            : null;
    }

    /// <summary>
    /// Remove acentos
    /// </summary>
    /// <remarks>Fonte: <see href="https://stackoverflow.com/a/13155469">stackoverflow</see></remarks>
    /// <param name="input"></param>
    public static string? RemoveDiacritics(this string? input)
    {
        if (input is null)
            return null;

        var chars = input
            .Normalize(NormalizationForm.FormD)
            .ToCharArray()
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Obtém uma frase aleatória de Lorem Ipsum.
    /// </summary>
    /// <returns></returns>
    public static string GetLoremIpsumPhrase()
    {
        var phrases = LOREM_IPSUM.Split('.', StringSplitOptions.TrimEntries).ToList();

        return phrases[Random.Shared.Next(0, phrases.Count)];
    }

    /// <summary>
    /// Converte a primeira letra da string em Maiúscula.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string FirstCharToUpper(this string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
    }

    /// <summary>
    /// Transform text to camelCase  (first char to lower).
    /// </summary>
    /// <param name="input"></param>
    /// <param name="removeSpaces"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string input, bool removeSpaces = true)
        => ToCamelCaseInternal(input, removeSpaces, false);

    /// <summary>
    /// Transform text to PascalCase (first char to upper).
    /// </summary>
    /// <param name="input"></param>
    /// <param name="removeSpaces"></param>
    /// <returns></returns>
    public static string ToPascalCase(this string input, bool removeSpaces = true)
        => ToCamelCaseInternal(input, removeSpaces, true);

    private static string ToCamelCaseInternal(string input, bool removeSpaces = true, bool firstCharToUpper = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        var sb = new StringBuilder(JsonNamingPolicy.CamelCase.ConvertName(input));

        if (removeSpaces)
        {
            sb.Replace(" ", string.Empty);
        }
        if (firstCharToUpper)
        {
            sb[0] = char.ToUpper(sb[0]);
        }

        return sb.ToString();
    }
}