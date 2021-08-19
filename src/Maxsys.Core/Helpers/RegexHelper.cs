namespace Maxsys.Core.Helpers
{
    public static class RegexHelper
    {
        // accents: àáéíóúãñõâêôäëïöüçÀÁÉÍÓÚÃÑÕÂÊÔÄËÏÖÜÇ

        #region REGEX

        /// <summary>
        ///Only numbers (0-9).
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
}