using System.Security.Cryptography;

namespace Maxsys.Core.Helpers;

public static class UIDHelper
{
    /// <summary>
    /// Randomly creates a hexadecimal string representation of a n bits UID.<br/>
    /// The result string is in lowecase.
    /// </summary>
    /// <returns>A string of hexadecimal 32 bits UID representation, for example: "7f2c4a00".</returns>
    public static string GenerateUID(UIDBits bits, UIDGenerationOptions options = UIDGenerationOptions.None)
        => GenerateUID((int)bits, options);

    /// <summary>
    /// Randomly creates a hexadecimal string representation of a n bytes UID (bits=n*8).<br/>
    /// The result string is in lowecase.
    /// </summary>
    /// <returns>A string of hexadecimal 32 bits (4 bytes) UID representation, for example: "7f2c4a00".</returns>
    public static string GenerateUID(int bytes, UIDGenerationOptions options = UIDGenerationOptions.None)
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(bytes);

        var result = BitConverter.ToString(randomBytes);

        #region Options

        if (options.HasFlag(UIDGenerationOptions.LowerCase))
        {
            result = result.ToLower();
        }
        if (!options.HasFlag(UIDGenerationOptions.KeepDots))
        {
            result = result.Replace("-", string.Empty);
        }

        #endregion Options

        return result;
    }
}

[Flags]
public enum UIDGenerationOptions : byte
{
    None = 0,
    LowerCase = 1,
    KeepDots = 2
}

public enum UIDBits
{
    B8 = 1,
    B16 = 2,
    B24 = 3,
    B32 = 4,
    B64 = 8,
    B128 = 16,
    B192 = 24,
    B256 = 32,
    B512 = 64,
    B1024 = 128
}