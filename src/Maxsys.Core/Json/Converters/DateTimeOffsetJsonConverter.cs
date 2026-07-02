using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maxsys.Core.Json.Converters;

/// <summary>
/// Conversor JSON personalizado para o tipo <see cref="DateTimeOffset"/>.
/// Garante que todas as datas sejam serializadas no formato ISO 8601 com timezone UTC (sufixo 'Z').
/// </summary>
public sealed class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    /// <summary>
    /// Desserializa uma string JSON em um objeto <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="reader">O leitor de JSON que fornece acesso ao token atual.</param>
    /// <param name="typeToConvert">O tipo de destino da conversão.</param>
    /// <param name="options">As opções de serialização configuradas.</param>
    /// <returns>
    /// Um objeto <see cref="DateTimeOffset"/> representando a data/hora lida.
    /// Retorna <see cref="DateTimeOffset.MinValue"/> se a string for nula ou vazia.
    /// </returns>
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        if (string.IsNullOrEmpty(dateString))
        {
            return DateTimeOffset.MinValue;
        }
        return DateTimeOffset.Parse(dateString, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Serializa um objeto <see cref="DateTimeOffset"/> em uma string JSON.
    /// </summary>
    /// <param name="writer">O escritor de JSON usado para gravar o valor.</param>
    /// <param name="value">O <see cref="DateTimeOffset"/> a ser serializado.</param>
    /// <param name="options">As opções de serialização configuradas.</param>
    /// <remarks>
    /// Este método força o formato ISO 8601 com sufixo 'Z' (yyyy-MM-ddTHH:mm:ss.fffZ)
    /// ao invés do formato padrão com offset '+00:00'.
    /// Todos os valores são convertidos para UTC antes da serialização.
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        // Força o formato ISO 8601 com 'Z' ao invés de '+00:00'
        writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
    }
}