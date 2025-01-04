using System.ComponentModel;
using System.Reflection;

namespace Maxsys.Core.Web.Swagger.Helpers;

internal static class Helper
{
    /// <summary>
    /// Obtém uma lista de descriptions do enum.
    /// Cada item do enum, terá uma description no seguinte formato:
    /// <para/>
    /// {valor} - {literal} ({atributo_description quando tiver})
    /// <para/>
    /// <example>
    /// Exemplo:
    /// <code>
    /// public enum SampleEnum : byte
    /// {
    ///     [Description("Este é o tipo A.")]
    ///     TipoA = 1,
    ///
    ///     [Description("Este é o tipo B.")]
    ///     TipoB,
    ///
    ///     // Sem description
    ///     TipoC = 99
    /// }
    ///
    /// var descriptions = GetEnumValuesList(typeof(SampleEnum));
    ///
    /// /*
    ///     descriptions:
    ///     [
    ///         "Valores possíveis:",
    ///         "1 - TipoA (Este é o tipo A.)",
    ///         "2 - TipoB (Este é o tipo B.)",
    ///         "99 - TipoC"
    ///     ]
    ///
    /// */
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="enumType"></param>
    /// <exception cref="ArgumentException"></exception>
    public static List<string> GetEnumDescriptionsList(Type enumType)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException($"Type {enumType.Name} não é enum.", nameof(enumType));

        var underlaying = Enum.GetUnderlyingType(enumType);
        var array = Enum.GetValues(enumType);
        var contents = new List<string>
        {
            "Valores possíveis:"
        };

        foreach (Enum item in array)
        {
            var value = Convert.ChangeType(item, underlaying);
            var desc = enumType!.GetField(item.ToString())!.GetCustomAttribute<DescriptionAttribute>()?.Description;
            var name = Enum.GetName(enumType, item);

            contents.Add($"{value} - {name}{(desc is null ? string.Empty : $" ({desc})")}");
        }

        return contents;
    }
}