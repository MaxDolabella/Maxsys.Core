using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Obtido de <see href="https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/"/>
/// </summary>
public static class EncryptHelper
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="salt">The 'salt' param must have 32 chars</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string AESEncrypt(string plainText, string salt)
    {
        if (salt?.Length != 32)
        {
            throw new ArgumentException("The 'salt' param must have 32 chars");
        }

        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(salt);
            aes.Key = bytes;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new())
            {
                using (CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        var result = Convert.ToBase64String(array);

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="salt">The 'salt' param must have 32 chars</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string AESDecrypt(string cipherText, string salt)
    {
        if (salt?.Length != 32)
        {
            throw new ArgumentException("The 'salt' param must have 32 chars");
        }

        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(salt);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var memoryStream = new MemoryStream(buffer))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
                return streamReader.ReadToEnd();
        }
    }
}