using System.Diagnostics.CodeAnalysis;
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

    public static string? RemoveInvalidFileNameChars(this string? input)
    {
        if (input is null)
            return null;

        var newValue = string.Join("_", input.Split(s_InvalidFileNameChars)).Trim();
        return newValue.All(c => c == '_') ? string.Empty : newValue;
    }

    /// <summary>
    /// Retorna um texto a partir de outro texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static string? GetTextOrNullIfEmpty(this string? input)
    {
        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }

    /// <summary>
    /// Retorna um número decimal a partir de um texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static decimal? GetDecimalOrNullIfEmpty(this string? input)
    {
        return !string.IsNullOrWhiteSpace(input)
            ? decimal.TryParse(input.Trim(), out decimal value) ? value : default(decimal?)
            : null;
    }

    /// <summary>
    /// Retorna uma data a partir de um texto, ou nulo se o texto testado for vazio/nulo
    /// </summary>
    public static DateTime? GetDateTimeOrNullIfEmpty(this string? input)
    {
        return !string.IsNullOrWhiteSpace(input)
            ? DateTime.TryParse(input.Trim(), out DateTime dateTime) ? dateTime : default(DateTime?)
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
    /// <param name="input"></param>
    public static string? GetOnlyNumbers(this string? input)
    {
        return input is not null
            ? new string([.. input.Where(char.IsDigit)])
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

    /// <summary>
    /// Divide a string inicial em quebras de linhas.
    /// </summary>
    /// <param name="input">string inicial</param>
    /// <returns>um array</returns>
    public static string[] SplitLines(this string? input)
    {
        return input is null
            ? []
            : [.. input.Split([Environment.NewLine, "\n", "\r"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
    }

    /// <summary>
    /// Breaks a given text into up to two parts, each with a maximum of 35 characters.
    /// </summary>
    /// <param name="input">The input text to split. Can be <c>null</c>.</param>
    /// <param name="maxLength"></param>
    /// <returns>
    /// A list containing up to two strings, each at most 35 characters long, or <c>null</c> if the input is <c>null</c>.
    /// If the input is empty or whitespace, returns an empty list.
    /// </returns>
    /// <remarks>
    /// Words are split at spaces when possible. If a word exceeds 35 characters, it is chunked into pieces of at most 35 characters.
    /// The method stops after collecting two parts.
    /// </remarks>
    public static List<string>? SplitTextIntoChunks([NotNullIfNotNull(nameof(input))] this string? input, int maxLength)
    {
        if (input is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            return [];
        }

        input = input.Trim();
        if (input.Length <= maxLength)
        {
            return [input];
        }

        var result = new List<string>(2);

        foreach (var word in input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            foreach (var part in word.Chunk(maxLength).Select(x => new string(x)))
            {
                result.Add(part);
                if (result.Count == 2)
                    return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Normaliza uma string para comparação segura, tratando quebras de linha, espaços e Unicode.
    /// </summary>
    /// <param name="input">A string original que será normalizada.</param>
    /// <returns>
    /// Uma nova string normalizada:
    /// <list type="bullet">
    /// <item>Todos os caracteres Unicode compostos são normalizados (FormC).</item>
    /// <item>Quebras de linha convertidas para '\n'.</item>
    /// <item>Espaços no início e fim removidos.</item>
    /// </list>
    /// Retorna <c>null</c> se a string original for <c>null</c>.
    /// </returns>
    public static string? NormalizeText(string? input)
    {
        if (input is null)
            return null;

        // Normaliza Unicode
        input = input.Normalize(NormalizationForm.FormC);

        // Converte todas quebras de linha para \n
        input = input.Replace("\r\n", "\n")
             .Replace('\r', '\n'); // caso haja \r isolado

        // Remove espaços extras nas extremidades
        input = input.Trim();

        return input;
    }
}