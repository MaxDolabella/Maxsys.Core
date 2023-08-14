using System.IO;
using System.Security.Cryptography;

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
}