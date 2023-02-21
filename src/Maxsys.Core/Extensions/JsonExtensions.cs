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
}