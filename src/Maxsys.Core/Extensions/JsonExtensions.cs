using System.Collections;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maxsys.Core.Extensions;

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

    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// Converte uma string json em um objeto <typeparamref name="T"/>. <para/>
    /// Caso o json seja nulo ou inválido, ou ocorra um erro na conversão, retorna um valor padrão não nulo <br/>
    /// ou lança uma exception caso o não haja valor padrão (nulo).<br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T FromJson<T>(this string? json, T defaultValue)
    {
        if (defaultValue is null)
            throw new ArgumentNullException(nameof(defaultValue), "Valor default não pode ser nulo.");

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
    /// Converte uma string json em um objeto <typeparamref name="T"/>.<para/>
    /// Caso o json seja nulo ou inválido, ou ocorra um erro na conversão, retorna um array vazio,
    /// uma lista vazia ou o objeto inicializado com um CTOR vazio.<br/>
    /// Caso o objeto não seja um array/lista e não tenha um CTOR vazio, uma exception é lançada.<br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static T FromJson<T>(this string? json)
    {
        object? result = default;
        var returnType = typeof(T);

        // Array
        if (returnType.IsArray)
        {
            var elementType = returnType.GetElementType()!;
            var arrayType = elementType.MakeArrayType();

            if (json is not null)
            {
                try
                {
                    result = JsonSerializer.Deserialize<T>(GenerateStreamFromString(json), JSON_DEFAULT_OPTIONS);
                }
                catch { }
            }

            return (T)(result ?? Array.CreateInstance(arrayType, 0));
        }

        // IEnumerable
        if (returnType.IsAssignableTo(typeof(IEnumerable)))
        {
            var genericType = returnType.GenericTypeArguments.First();
            var listType = typeof(List<>).MakeGenericType(new[] { genericType });

            if (json is not null)
            {
                try
                {
                    result = JsonSerializer.Deserialize(GenerateStreamFromString(json), listType, JSON_DEFAULT_OPTIONS);
                }
                catch { }
            }

            return (T)(IList)(result ?? Activator.CreateInstance(listType)!);
        }

        // Objeto comum
        var hasDefaultCtor = returnType.GetConstructor(Type.EmptyTypes) != null;
        if (!hasDefaultCtor)
            throw new ArgumentException($"O Tipo {returnType.Name} não é um array, nem uma lista e nem possui construtor vazio.");

        if (json is not null)
        {
            try
            {
                result = JsonSerializer.Deserialize(GenerateStreamFromString(json), returnType, JSON_DEFAULT_OPTIONS);
            }
            catch { }
        }

        return (T)(result ?? Activator.CreateInstance(returnType)!);
    }

    /// <summary>
    /// Converte uma string json em um objeto de acordo com o <paramref name="returnType"/> passado.<para/>
    /// Caso o json seja nulo ou inválido, ou ocorra um erro na conversão, retorna um array vazio,
    /// uma lista vazia ou o objeto inicializado com um CTOR vazio.<br/>
    /// Caso o objeto não seja um array/lista e não tenha um CTOR vazio, uma exception é lançada.<br/>
    /// </summary>
    /// <param name="json"></param>
    /// <param name="returnType">o tipo para qual se deseja converter o json</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static object FromJson(this string? json, Type returnType)
    {
        object? result = default;

        // Array
        if (returnType.IsArray)
        {
            var elementType = returnType.GetElementType()!;
            var arrayType = elementType.MakeArrayType();

            if (json is not null)
            {
                try
                {
                    result = JsonSerializer.Deserialize(GenerateStreamFromString(json), arrayType, JSON_DEFAULT_OPTIONS);
                }
                catch { }
            }

            return result ?? Array.CreateInstance(arrayType, 0);
        }

        // IEnumerable
        if (returnType.IsAssignableTo(typeof(IEnumerable)))
        {
            var genericType = returnType.GenericTypeArguments.First();
            var listType = typeof(List<>).MakeGenericType(new[] { genericType });

            if (json is not null)
            {
                try
                {
                    result = JsonSerializer.Deserialize(GenerateStreamFromString(json), listType, JSON_DEFAULT_OPTIONS);
                }
                catch { }
            }

            return result ?? (IList)Activator.CreateInstance(listType)!;
        }

        // Objeto comum
        var hasDefaultCtor = returnType.GetConstructor(Type.EmptyTypes) != null;
        if (!hasDefaultCtor)
            throw new ArgumentException($"O Tipo {returnType.Name} não possui construtor vazio.");

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

    /// <summary>
    /// Converte um objeto do tipo <typeparamref name="T"/> em uma string JSON.<para/>
    /// Shortcut para <c>JsonSerializer.Serialize(value, options);</c>
    /// </summary>
    /// <param name="value">o valor a ser convertido</param>
    /// <param name="options">options para controlar comportamento da serialização.</param>
    /// <exception cref="NotSupportedException"/>
    public static string ToJson<T>(this T? value, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(value, options ?? JSON_DEFAULT_OPTIONS);
    }
}