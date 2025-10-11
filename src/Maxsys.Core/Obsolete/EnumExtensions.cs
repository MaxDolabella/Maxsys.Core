namespace System;

[Obsolete("Utilizar EnumExtensions de Maxsys.Core.Extensions", true)]
public static class EnumExtensions
{
    public static string? ToFriendlyName(this Enum? value, string? defaultValue = null) => throw new NotImplementedException();

    public static TEnum? ToEnum<TEnum>(this string? text) where TEnum : struct, Enum => throw new NotImplementedException();

    public static TEnum ToEnum<TEnum>(this byte? value, TEnum defaultEnum) where TEnum : Enum => throw new NotImplementedException();

    public static TTarget Convert<TTarget>(this Enum? source) where TTarget : struct, Enum => throw new NotImplementedException();

    public static TTarget? ConvertNull<TTarget>(this Enum? source) where TTarget : struct, Enum => throw new NotImplementedException();

    public static TTarget Convert<TTarget>(string? name) where TTarget : struct, Enum => throw new NotImplementedException();

    public static TTarget? ConvertNull<TTarget>(string? name) where TTarget : struct, Enum => throw new NotImplementedException();

    public static object? GetMinValue(Type enumType) => throw new NotImplementedException();

    public static TEnum GetMinValue<TEnum>() where TEnum : struct, Enum => throw new NotImplementedException();
}