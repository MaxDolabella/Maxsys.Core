using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Fornece métodos de extensão para manipulação e obtenção de hash.
/// </summary>
public static class HashHelper
{
    /// <summary>
    /// Obtém um array de bytes que representa um hash SHA512 a partir de um array de bytes.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static byte[] ToSHA512(this byte[] bytes)
    {
        return SHA512.HashData(bytes);
    }

    /// <summary>
    /// Obtém um array de bytes que representa um hash SHA512 a partir de um MemoryStream.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static byte[] ToSHA512(this MemoryStream stream)
    {
        var bytes = stream.ToArray();

        return ToSHA512(bytes);
    }

    /// <summary>
    /// Obtém uma string que representa um hash SHA512 a partir de um array de bytes.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToSHA512HashString(this byte[] bytes)
    {
        var sha512 = ToSHA512(bytes);

        return sha512.ToHexString();
    }

    /// <summary>
    /// Obtém uma string que representa um hash SHA512 a partir de um MemoryStream.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string ToSHA512HashString(this MemoryStream stream)
    {
        var sha512 = ToSHA512(stream);

        return sha512.ToHexString();
    }

    /// <summary>
    /// Obtém uma string que representa um hash a partir de um object.
    /// </summary>
    /// <param name="value">Objeto que se deseja gerar a hash.</param>
    /// <param name="hashType">(Opcional). Valor padrão é <see cref="HashTypes.MD5"/>.</param>
    public static string GetHexHash(object value, HashTypes hashType = HashTypes.MD5)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        return GetHexHash(JsonSerializer.Serialize(value), hashType);
    }

    /// <summary>
    /// Obtém uma string que representa um hash a partir de um texto.
    /// </summary>
    /// <param name="value">Texto para o qual se deseja gerar a hash.</param>
    /// <param name="hashType">(Opcional). Valor padrão é <see cref="HashTypes.MD5"/>.</param>
    public static string GetHexHash(string value, HashTypes hashType = HashTypes.MD5)
        => GetHexHash(Encoding.UTF8.GetBytes(value), hashType);

    /// <summary>
    /// Obtém uma string que representa um hash a partir de um array de bytes.
    /// </summary>
    /// <param name="value">Array de bytes para o qual se deseja gerar a hash.</param>
    /// <param name="hashType">(Opcional). Valor padrão é <see cref="HashTypes.MD5"/>.</param>
    public static string GetHexHash(byte[] value, HashTypes hashType = HashTypes.MD5)
    {
        byte[] hash = hashType switch
        {
            HashTypes.MD5 => MD5.HashData(value),
            HashTypes.SHA256 => SHA256.HashData(value),
            HashTypes.SHA384 => SHA384.HashData(value),
            HashTypes.SHA512 => SHA512.HashData(value),
            _ => SHA512.HashData(value),
        };

        return hash.ToHexString();
    }
}

public enum HashTypes
{
    MD5,
    SHA256,
    SHA384,
    SHA512
}