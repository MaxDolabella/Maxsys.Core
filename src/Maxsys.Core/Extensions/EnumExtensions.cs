using System.ComponentModel;
using System.Reflection;

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
    /// Console.WriteLine(enumA.ToFriendlyName());              // "Este é o tipo A"
	/// Console.WriteLine(enumC.ToFriendlyName());              // "TipoC"
	/// Console.WriteLine(enumNull.ToFriendlyName());           // null
	/// Console.WriteLine(enumNull.ToFriendlyName(""));         // ""
	/// Console.WriteLine(enumNull.ToFriendlyName("Nenhum"));   // "Nenhum"
	/// Console.WriteLine(enumError.ToFriendlyName());          // "77"
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
            return value.ToString();

        var attDescription = fieldInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;

        return attDescription ?? value.ToString();
    }

    /// <summary>
    /// Obtém um <typeparamref name="TEnum"/> a partir de um texto.
    /// <para/>
    /// Se um item do enum ou sua descrição (<see cref="DescriptionAttribute"/>)
    /// tiver o mesmo valor que <paramref name="text"/>, então o enum é
    /// retornado com seu valor correspondente.
    /// <br/>
    /// Caso não haja correspondência ou haja mais de uma correspondência, valor retornado será <see langword="null"/>
    /// </summary>
    /// <typeparam name="TEnum">é o tipo do enum.</typeparam>
    /// <param name="text">é o valor a ser comparado.</param>
    /// <returns>O enum correspondente ao <paramref name="text"/> ou <see langword="null"/>.</returns>
    public static TEnum? ToEnum<TEnum>(this string text) where TEnum : struct, Enum
    {
        TEnum? result;

        if (string.IsNullOrWhiteSpace(text))
            return null;

        try
        {
            result = Enum.GetValues<TEnum>()
                .SingleOrDefault(x
                    => x.ToString().ToLower() == text.ToLower()
                    || x.ToFriendlyName(string.Empty)!.ToLower() == text.ToLower());
        }
        catch (Exception)
        {
            result = null;
        }

        return result;
    }

    /// <summary>
    /// Obtém um item do enum a partir de um valor byte.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TEnum ToByteEnum<TEnum>(this byte? value) where TEnum : Enum
    {
        var value2 = value ?? 0;
        object sort = Enum.IsDefined(typeof(TEnum), value2)
            ? value2
            : Enum.GetValues(typeof(TEnum)).Cast<byte>().Min();

        return (TEnum)sort;
    }
}