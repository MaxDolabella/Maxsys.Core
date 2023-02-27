using System.IO;
using System.Text.Json.Serialization;

namespace System.Text.Json;

/// <summary>
/// Provides extension methods for JsonSerializer or string Deserialization.
/// </summary>
public static class JsonExtensions
{
    /// <summary>
    /// PropertyNameCaseInsensitive=true, ReferenceHandler.IgnoreCycles
    /// </summary>
    public static readonly JsonSerializerOptions JSON_DEFAULT_OPTIONS = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Converte uma string json em um objeto <typeparamref name="T"/>. <para/>
    /// Caso o json seja nulo ou inválido, ou ocorra um erro na conversão, retorna um valor padrão não nulo <br/>
    /// ou lança uma exception caso o não haja valor padrão (nulo).<br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="defaultValue"></param>
    /// <param name="options">options to control the behavior during parsing</param>
    /// <returns></returns>
    /// <exception cref="JsonException"></exception>
    public static T Deserialize<T>(this string? json, T? defaultValue = null, JsonSerializerOptions? options = null)
        where T : class
    {
        T? result = default;

        if (json is not null)
        {
            try
            {
                result = JsonSerializer.Deserialize<T>(json, options ?? JSON_DEFAULT_OPTIONS);
            }
            catch (Exception ex)
            {
                if (defaultValue is null)
                    throw new JsonException("Error converting from string.Deserialize() extension method", ex);
            }
        }

        return result is not null
            ? result
            : defaultValue is not null
                ? defaultValue
                : throw new JsonException("Error converting from string.Deserialize(). Default value cannot be null when deserialization result is null.");
    }

    /// <summary>
    /// Converte uma string json em um objeto <typeparamref name="T"/>. <para/>
    /// Caso o json seja nulo ou inválido, ou ocorra um erro na conversão, retorna um valor padrão não nulo.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T Deserialize<T>(this string? json, T defaultValue)
    {
        if (defaultValue is null)
            throw new ArgumentNullException(nameof(defaultValue), "Default Value cannot be null.");

        T? result = default;
        if (json is not null)
        {
            try
            {
                result = JsonSerializer.Deserialize<T>(json, options: JSON_DEFAULT_OPTIONS);
            }
            catch { }
        }

        return result ?? defaultValue;
    }

    /// <summary>
    /// Converte uma string json em um objeto <typeparamref name="T"/>. <para/>
    /// Caso o json seja nulo ou inválido, ou ocorra um erro na conversão, retorna um instância padrão de <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T Deserialize<T>(this string? json)
        where T : new()
    {
        T? result = default;
        if (json is not null)
        {
            try
            {
                result = JsonSerializer.Deserialize<T>(json, options: JSON_DEFAULT_OPTIONS);
            }
            catch { }
        }

        return result ?? new();
    }

    /// <summary>
    /// Converte uma string json em um objeto do tipo passado como parâmetro.<para/>
    /// Caso o json seja nulo ou inválido, ou ocorra um erro na conversão, retorna um instância padrão do tipo passado como parâmetro.<br/>
    /// O Tipo passado em <paramref name="returnType"/> precisa possuir um construtor vazio ou uma exception será lançada.
    /// </summary>
    /// <param name="json"></param>
    /// <param name="returnType">é o tipo do objeto desserializado. Precisa ser um tipo com construtor vazio ou uma exception será lançada.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static object Deserialize(this string? json, Type returnType)
    {
        var hasDefaultCtor = returnType.GetConstructor(Type.EmptyTypes) != null;
        if (!hasDefaultCtor)
            throw new ArgumentException($"O Tipo {returnType.Name} não possui construtor vazio.");

        object? result = default;
        if (json is not null)
        {
            try
            {
                result = JsonSerializer.Deserialize(GenerateStreamFromString(json), returnType, JSON_DEFAULT_OPTIONS);
            }
            catch { }
        }

        return result ?? Activator.CreateInstance(returnType)!;
    }

    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}