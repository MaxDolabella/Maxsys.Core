using System;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Provides Regex Patterns.
/// </summary>
public static class RegexHelper
{
    // accents: àáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ

    [Flags]
    public enum Pattern
    {
        // 0-9
        Numbers = 1,

        Letters = 2,
        Spaces = 4,
        Hyphen = 8,
        Dots = 16,
        Commas = 32,
        Parentesis = 64
    }

    /// <summary>
    /// Get a regex pattern from pattern options.
    /// </summary>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static string GetPattern(Pattern pattern)
    {
        var regexPattern = string.Empty;

        if (pattern.HasFlag(Pattern.Numbers))
            regexPattern += "0-9";

        if (pattern.HasFlag(Pattern.Letters))
            regexPattern += "a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ"; //\s\-\(\)\,\.

        if (pattern.HasFlag(Pattern.Spaces))
            regexPattern += "\\s";

        if (pattern.HasFlag(Pattern.Hyphen))
            regexPattern += "\\-";

        if (pattern.HasFlag(Pattern.Dots))
            regexPattern += "\\.";

        if (pattern.HasFlag(Pattern.Commas))
            regexPattern += "\\,";

        if (pattern.HasFlag(Pattern.Parentesis))
            regexPattern += "\\(\\)";

        return $@"^[{regexPattern}]+$";
    }

    #region REGEX

    /// <summary>
    /// Only numbers (0-9).
    /// </summary>
    public const string PATTERN_NUMBERS = @"^[0-9]+$";

    /// <summary>
    /// Only letters (a-z + accents, ignoring caps)
    /// </summary>
    public const string PATTERN_LETTERS = @"^[a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ]+$";

    /// <summary>
    /// Only letters (a-z + accents, ignoring caps) and numbers
    /// </summary>
    public const string PATTERN_LETTERS_NUMBERS = @"^[0-9a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ]+$";

    /// <summary>
    /// Only letters (a-z + accents, ignoring caps), numbers and spaces
    /// </summary>
    public const string PATTERN_LETTERS_NUMBERS_SPACES = @"^[0-9a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ\s]+$";

    /// <summary>
    /// Only letters (a-z + accents, ignoring caps), numbers and spaces
    /// </summary>
    public const string PATTERN_LETTERS_SPACES = @"^[0-9a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ\s]+$";

    /// <summary>
    /// Only letters (ignore caps), numbers, space and hyphen
    /// </summary>
    public const string PATTERN_LETTERS_NUMBERS_SPACES_HYPHENS = @"^[0-9a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ\s\-]+$";

    /// <summary>
    /// Only letters (ignore caps), numbers, parenthesis, comma, dot, parenspace and hyphen
    /// </summary>
    public const string PATTERN_LETTERS_NUMBERS_PARENTHESIS_COMMA_DOT_SPACES_HYPHENS = @"^[0-9a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ\s\-\(\)\,\.]+$";

    /// <summary>
    /// Does work? IDK
    /// </summary>
    public const string PATTERN_FOR_VALID_FILE_NAME = "\\A(?!(?:COM[0-9]|CON|LPT[0-9]|NUL|PRN|AUX|com[0-9]|con|lpt[0-9]|nul|prn|aux)|[\\s\\.])[^\\\\/:*\"?<>|]{1,254}\\z";

    /// <summary>
    /// Does work? IDK<para/>
    /// Regex Pattern for valid file path.
    /// Source from Stack Overflow <see href="https://stackoverflow.com/a/6416209/4121969">here</see>.
    /// </summary>
    public const string PATTERN_FOR_VALID_FILE_PATH = @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$";

    #endregion REGEX
}