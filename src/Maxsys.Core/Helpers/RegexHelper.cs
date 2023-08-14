using System.ComponentModel;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Provides Regex Patterns.
/// </summary>
public static class RegexHelper
{
    // accents: àáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ

    /// <summary>
    /// Flag para representar um ou mais padrões Regex.
    /// <list type="bullet">
    /// <item>
    ///     <term>1. <see cref="Numbers"/></term>
    ///     <description>Números.</description>
    /// </item>
    /// <item>
    ///     <term>2. <see cref="Letters"/></term>
    ///     <description>Letras.</description>
    /// </item>
    /// <item>
    ///     <term>4. <see cref="Spaces"/></term>
    ///     <description>Espaço.</description>
    /// </item>
    /// <item>
    ///     <term>8. <see cref="Hyphen"/></term>
    ///     <description>Hífen.</description>
    /// </item>
    /// <item>
    ///     <term>16.<see cref="Dots"/></term>
    ///     <description>Pontuação.</description>
    /// </item>
    /// <item>
    ///     <term>32.<see cref="Commas"/></term>
    ///     <description>Vírgula.</description>
    /// </item>
    /// <item>
    ///     <term>64.<see cref="Parentesis"/></term>
    ///     <description>Parêntesis.</description>
    /// </item>
    /// </list>
    /// </summary>
    [Flags]
    public enum Pattern
    {
        /// <summary>
        /// Adiciona 0-9 ao pattern.
        /// </summary>
        [Description("Números")]
        Numbers = 1,

        /// <summary>
        /// Adiciona "a-zA-ZàáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ" ao pattern.
        /// </summary>
        [Description("Letras")]
        Letters = 2,

        /// <summary>
        /// Adiciona "\s" ao pattern.
        /// </summary>
        [Description("Espaço")]
        Spaces = 4,

        /// <summary>
        /// Adiciona "\-" ao pattern.
        /// </summary>
        [Description("Hífen")]
        Hyphen = 8,

        /// <summary>
        /// Adiciona "\." ao pattern.
        /// </summary>
        [Description("Ponto")]
        Dots = 16,

        /// <summary>
        /// Adiciona "\," ao pattern.
        /// </summary>
        [Description("Vírgula")]
        Commas = 32,

        /// <summary>
        /// Adiciona "\(\)" ao pattern.
        /// </summary>
        [Description("Parêntesis")]
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