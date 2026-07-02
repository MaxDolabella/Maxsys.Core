using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace System;

/// <summary>
/// Provides extension methods to Enums.
/// </summary>
public static class EnumExtensions
{
    extension(Enum? value)
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
        /// <param name="defaultValue"></param>
        /// <returns>The friendly name string representation of the value of this instance.</returns>
        public string? ToFriendlyName(string? defaultValue = null)
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
        /// Converte um enum em outro a partir do literal.
        /// <para/>
        /// Caso <paramref name="source"/> seja nulo ou os enums não tenham correspondência,
        /// uma exception será lançada.
        /// </summary>
        /// <typeparam name="TTarget">Enum de destino (não nulo)</typeparam>
        /// <returns>Retorna um <typeparamref name="TTarget"/> não nulo.</returns>
        /// <exception cref="ArgumentException"></exception>
        public TTarget Convert<TTarget>()
            where TTarget : struct, Enum
            => Convert<TTarget>(value?.ToString());

        /// <summary>
        /// Converte um enum em outro a partir do literal.
        /// <para/>
        /// Caso <paramref name="source"/> seja nulo, será retornado <see langword="null"/>.<br/>
        /// Caso os enums não tenham correspondência, uma exception será lançada.
        /// </summary>
        /// <typeparam name="TTarget">Enum de destino (nulável)</typeparam>
        /// <returns>Retorna um <typeparamref name="TTarget"/> nulável.</returns>
        /// <exception cref="ArgumentException"></exception>
        public TTarget? ConvertNull<TTarget>() where TTarget : struct, Enum
            => ConvertNull<TTarget>(value?.ToString());
    }

    extension(string? text)
    {
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
        /// <returns>O enum correspondente ao texto ou <see langword="null"/>.</returns>
        public TEnum? ToEnum<TEnum>() where TEnum : struct, Enum
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
    }

    extension(byte? value)
    {
        /// <summary>
        /// Obtém um item do enum a partir de um valor byte.
        /// Se nenhum item do enum corresponder ao byte, então o primeiro item do enum será retornado.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="defaultEnum">é o enum caso a conversão falhe.</param>
        /// <returns></returns>
        public TEnum ToByteEnum<TEnum>(TEnum defaultEnum) where TEnum : Enum
        {
            var val = value ?? 0;
            object sort = Enum.IsDefined(typeof(TEnum), val)
                ? val
                : defaultEnum;

            return (TEnum)sort;
        }
    }

    public static T Min<T>(Type type)
    {
        return (T)Enum.ToObject(type, Enum.GetValues(type).Cast<T>().Min()!);
    }

    public static T Max<T>(Type type)
    {
        return (T)Enum.ToObject(type, Enum.GetValues(type).Cast<T>().Max()!);
    }

    public static TTarget Convert<TTarget>(string? name) where TTarget : struct, Enum
    {
        return Enum.TryParse<TTarget>(name, ignoreCase: true, out var result)
            ? result
            : throw new ArgumentException($"Valor '{name}' não encontrado no enum {typeof(TTarget).Name}");
    }

    public static TTarget? ConvertNull<TTarget>(string? name) where TTarget : struct, Enum
    {
        return string.IsNullOrEmpty(name)
            ? default :
            (TTarget?)Convert<TTarget>(name);
    }

    /// <summary>
    /// Obtém o literal do enum com o menor valor numérico
    /// </summary>
    /// <param name="enumType">O tipo do enum</param>
    /// <returns>O literal do enum com menor valor, ou null se o tipo não for um enum ou estiver vazio</returns>
    /// <exception cref="ArgumentNullException">Lançada quando enumType é null</exception>
    /// <exception cref="ArgumentException">Lançada quando o tipo fornecido não é um enum</exception>
    /// <remarks>
    /// Este método:
    /// - Verifica se o tipo fornecido é realmente um enum
    /// - Obtém todos os valores do enum
    /// - Converte os valores para long para comparação numérica
    /// - Retorna o literal com o menor valor numérico
    /// - Funciona com enums de qualquer tipo base (byte, int, long, etc.)
    /// </remarks>
    /// <example>
    /// <code>
    /// public enum Priority { Low = 1, Medium = 2, High = 3 }
    ///
    /// var minValue = EnumHelper.GetMinValue(typeof(Priority));
    /// // minValue será Priority.Low (valor 1)
    ///
    /// public enum Status { Active = 10, Inactive = 5, Pending = 20 }
    /// var minStatus = EnumHelper.GetMinValue(typeof(Status));
    /// // minStatus será Status.Inactive (valor 5)
    /// </code>
    /// </example>
    public static object? GetMinValue(Type enumType)
    {
        if (enumType == null)
            throw new ArgumentNullException(nameof(enumType));

        if (!enumType.IsEnum)
            throw new ArgumentException($"O tipo '{enumType.Name}' não é um enum.", nameof(enumType));

        var values = Enum.GetValues(enumType);

        if (values.Length == 0)
            return null;

        // Converte todos os valores para long para comparação
        var enumValues = values.Cast<object>()
            .Select(value => new { Value = value, NumericValue = System.Convert.ToInt64(value) })
            .OrderBy(x => x.NumericValue)
            .FirstOrDefault();

        return enumValues?.Value;
    }

    /// <summary>
    /// Obtém o literal do enum com o menor valor numérico (versão genérica)
    /// </summary>
    /// <typeparam name="TEnum">O tipo do enum</typeparam>
    /// <returns>O literal do enum com menor valor</returns>
    /// <exception cref="ArgumentException">Lançada quando TEnum não é um enum</exception>
    /// <remarks>
    /// Versão genérica do método GetMinValue que oferece type safety em tempo de compilação
    /// </remarks>
    /// <example>
    /// <code>
    /// public enum Priority { Low = 1, Medium = 2, High = 3 }
    ///
    /// var minValue = EnumHelper.GetMinValue&lt;Priority&gt;();
    /// // minValue será Priority.Low (valor 1) com tipo Priority
    /// </code>
    /// </example>
    public static TEnum GetMinValue<TEnum>() where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();

        return values
            .OrderBy(value => System.Convert.ToInt64(value))
            .FirstOrDefault();
    }
}