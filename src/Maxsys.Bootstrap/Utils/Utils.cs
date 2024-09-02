using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Maxsys.Bootstrap;

internal static class Utils
{
    public static string GenerateRandomId() => BitConverter.ToString(RandomNumberGenerator.GetBytes(8));

    public static string? ToFriendlyName(this Enum? value, string? defaultValue = null)
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
}