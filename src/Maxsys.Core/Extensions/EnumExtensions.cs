using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace System;

/// <summary>
/// Provides extension methods to Enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Converts the value of this instance to its friendly name string representation
    /// provided by a <see cref="DescriptionAttribute"/>.
    /// <para/>
    /// <example>
    /// Example:
    /// <code>
    /// public enum SampleEnum : byte
    /// {
    ///     [Description("Este é o tipo A")]
    ///     TipoA = 1,
    ///
    ///     [Description("Este é o tipo B")]
    ///     TipoB,
    ///
    ///     // Sem description
    ///     TipoC = 99
    /// }
    ///
    /// var enumA = SampleEnum.TipoA;
    /// var enumC = SampleEnum.TipoC;
    /// var enumNull = default(SampleEnum?);
    /// var enumError = (SampleEnum)77;
    ///
    /// Console.WriteLine(enumA.ToFriendlyName());                          // "Este é o tipo A"
	/// Console.WriteLine(enumC.ToFriendlyName());                          // "TipoC"
	/// Console.WriteLine(enumNull.ToFriendlyName());                       // null
	/// Console.WriteLine(enumNull.ToFriendlyName(""));                     // ""
	/// Console.WriteLine(enumNull.ToFriendlyName("Nenhum"));               // "Nenhum"
	/// Console.WriteLine(enumError.ToFriendlyName());                      // null
	/// Console.WriteLine(enumError.ToFriendlyName("Indefinido"));          // "Indefinido"
	/// Console.WriteLine(enumError.ToFriendlyName(enumError.ToString()));  // "77"
    /// </code>
    ///
    /// </example>
    /// </summary>
    /// <param name="value">An enum value.</param>
    /// <param name="defaultValue"></param>
    /// <returns>The friendly name string representation of the value of this instance.</returns>
    public static string? ToFriendlyName(this Enum? value, string? defaultValue = null)
    {
        if (value is null)
            return defaultValue;

        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo is null)
            return defaultValue;

        var attDescription = fieldInfo.GetCustomAttribute<EnumMemberAttribute>()?.Value 
            ?? fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return attDescription ?? value.ToString();
    }

    /// <summary>
    /// Obtém um <typeparamref name="TEnum"/> a partir de um texto.
    /// <para/>
    /// Se um item do enum ou seu nome amigável (<see cref="DescriptionAttribute"/> / <see cref="EnumMemberAttribute"/>)
    /// tiver o mesmo valor que <paramref name="text"/>, então o enum é
    /// retornado com seu valor correspondente.
    /// <br/>
    /// Caso não haja correspondência ou haja mais de uma correspondência, valor retornado será <see langword="null"/>
    /// </summary>
    /// <typeparam name="TEnum">é o tipo do enum.</typeparam>
    /// <param name="text">é o valor a ser comparado.</param>
    /// <returns>O enum correspondente ao <paramref name="text"/> ou <see langword="null"/>.</returns>
    public static TEnum? ToEnum<TEnum>(this string? text) where TEnum : struct, Enum
    {
        TEnum? result = null;

        try
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                result = Enum.GetValues<TEnum>()
                    .Single(x 
                        => x.ToFriendlyName(defaultValue: x.ToString())!.Equals(text, StringComparison.CurrentCultureIgnoreCase)
                        || x.ToString().Equals(text, StringComparison.CurrentCultureIgnoreCase));
            }
        }
        catch (Exception)
        { }

        return result;
    }

    /// <summary>
    /// Obtém um item do enum a partir de um valor byte.
    /// Se nenhum item do enum corresponder ao byte, então o primeiro item do enum será retornado.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="defaultEnum">é o enum caso a conversão falhe.</param>
    /// <returns></returns>
    public static TEnum ToByteEnum<TEnum>(this byte? value, TEnum defaultEnum) where TEnum : Enum
    {
        value ??= 0;
        object sort = Enum.IsDefined(typeof(TEnum), value!)
            ? value
            : defaultEnum;

        return (TEnum)sort;
    }

    /// <summary>
    /// Obtém SortablePropertyAttribute.Name ou o nome do literal de um Enum.
    /// <br/>
    /// Um replace é aplicado antes do valor final ser retornado substituindo
    /// <c>"__"</c> por <c>"."</c>.
    /// <br/>
    /// Logo um literal <c>SomeEnum.Foo__Bar__Xpto_Cuca</c> retornará <c>"Foo.Bar.Xpto_Cuca"</c>.
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string ToSortablePropertyName(this Enum enumValue)
    {
        var literal = enumValue.ToString();

        var attDescription = enumValue
            .GetType()
            .GetField(literal)?
            .GetCustomAttribute<SortablePropertyAttribute>()?
            .Name;

        return (attDescription ?? literal).Replace("__", ".");
    }

    public static T Min<T>(Type type)
    {
        return (T)Enum.ToObject(type, Enum.GetValues(type).Cast<T>().Min()!);
    }

    public static T Max<T>(Type type)
    {
        return (T)Enum.ToObject(type, Enum.GetValues(type).Cast<T>().Max()!);
    }
}