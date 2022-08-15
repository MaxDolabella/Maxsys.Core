using System.ComponentModel;
using System.Linq;

namespace System;

/// <summary>
/// Provides extension methods to Enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Converts the value of this instance to its friendly name string representation
    /// provided by a <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <param name="value">An  <typeparamref name="T"/> value.</param>
    /// <returns>The friendly name string representation of the value of this instance.</returns>
    public static string ToFriendlyName<T>(this T value) where T : Enum
    {
        var fieldInfo = value?.GetType().GetField(value.ToString());

        if (fieldInfo is null)
            return string.Empty;

        var attributes = (DescriptionAttribute[])fieldInfo
            .GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes is not null && attributes.Length > 0
            ? attributes[0].Description
            : value!.ToString();
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

        if (string.IsNullOrWhiteSpace(text)) return null;

        try
        {
            result = Enum.GetValues<TEnum>()
                .SingleOrDefault(x
                    => x.ToString().ToLower() == text.ToLower()
                    || x.ToFriendlyName().ToLower() == text.ToLower());
        }
        catch (Exception)
        {
            result = null;
        }

        return result;
    }
}