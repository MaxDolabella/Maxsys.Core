using System.Text.Json;
using System.Text.Json.Serialization;
using Maxsys.Core.Helpers;

namespace Maxsys.Core.Json.Converters;

/// <summary>
/// Converte valores JSON representando timestamps Unix em instâncias de <see cref="DateTime"/> e vice-versa.
/// </summary>
/// <remarks>
/// Esta classe permite a serialização e desserialização de valores de tempo no formato Unix Timestamp.
/// <para/>
/// Durante a leitura (<see cref="Read"/>), o valor numérico é interpretado como um timestamp Unix em milissegundos
/// e convertido em um <see cref="DateTime"/> UTC equivalente utilizando
/// <c>DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime</c>.
/// <para/>
/// Durante a escrita (<see cref="Write"/>), a data é gravada como uma string no formato ISO 8601 ("o").
/// </remarks>
public sealed class UnixTimestampJsonConverter : JsonConverter<DateTime>
{
    /// <summary>
    /// Lê um valor JSON e o converte em uma instância de <see cref="DateTime"/>.
    /// </summary>
    /// <param name="reader">O leitor JSON contendo o valor numérico representando um Unix Timestamp (em milissegundos).</param>
    /// <param name="typeToConvert">O tipo de destino (espera-se <see cref="DateTime"/>).</param>
    /// <param name="options">As opções de serialização JSON aplicáveis.</param>
    /// <returns>
    /// Um <see cref="DateTime"/> correspondente ao timestamp Unix fornecido, em UTC.
    /// </returns>
    /// <exception cref="JsonException">Lançada se o valor lido não for um número válido.</exception>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt64();
        var date = DateTimeHelper.FromUnixTimestamp(value);

        return date;
    }

    /// <summary>
    /// Escreve um valor <see cref="DateTime"/> como string no formato ISO 8601.
    /// </summary>
    /// <param name="writer">O gravador JSON que receberá o valor convertido.</param>
    /// <param name="value">A data/hora a ser escrita.</param>
    /// <param name="options">As opções de serialização JSON aplicáveis.</param>
    /// <remarks>
    /// O valor é formatado com <c>value.ToString("o")</c>, garantindo compatibilidade com o padrão ISO 8601.
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("o"));
    }
}
