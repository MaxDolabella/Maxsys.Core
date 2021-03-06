using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class StringHelper
    {
        public static string StringJoin(this IEnumerable<string> stringCollection, string separator = "\n")
            => string.Join(separator, stringCollection);

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
                    : text[0].ToString().ToLower() + text.Substring(1);
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
}