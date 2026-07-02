namespace Maxsys.Core.Utils;

/// <summary>
/// Métodos utilitários de geração aleatória baseados em <see cref="Random.Shared"/>.
/// </summary>
public static class RandomUtils
{
    private const string AlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string HexChars = "0123456789ABCDEF";
    private const string DigitChars = "0123456789";

    // ── Valores primitivos ────────────────────────────────────────────────

    public static bool NextBool()
        => Random.Shared.Next(2) == 1;

    public static bool NextChance(double probability)
        => Random.Shared.NextDouble() < probability;

    public static int NextInt(int minValue, int maxValue)
        => Random.Shared.Next(minValue, maxValue);

    public static long NextLong(long minValue, long maxValue)
        => Random.Shared.NextInt64(minValue, maxValue);

    public static float NextFloat(float minValue, float maxValue)
        => Random.Shared.NextSingle() * (maxValue - minValue) + minValue;

    public static double NextDouble(double minValue, double maxValue)
        => Random.Shared.NextDouble() * (maxValue - minValue) + minValue;

    public static decimal NextDecimal(decimal minValue, decimal maxValue)
        => (decimal)Random.Shared.NextDouble() * (maxValue - minValue) + minValue;

    // ── Bytes ─────────────────────────────────────────────────────────────

    public static byte[] NextBytes(int count)
    {
        var buffer = new byte[count];
        Random.Shared.NextBytes(buffer);

        return buffer;
    }

    // ── Strings ───────────────────────────────────────────────────────────

    public static string NextString(int length)
        => NextString(length, AlphanumericChars);

    public static string NextHexString(int length)
        => NextString(length, HexChars);

    public static string NextDigits(int length)
        => NextString(length, DigitChars);

    public static string NextString(int length, string chars)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        return string.Create(length, chars, static (span, c) =>
        {
            for (var i = 0; i < span.Length; i++)
                span[i] = c[Random.Shared.Next(c.Length)];
        });
    }

    // ── DateTime / TimeSpan ───────────────────────────────────────────────

    public static DateTime NextDateTime(DateTime minValue, DateTime maxValue)
    {
        var range = (maxValue - minValue).Ticks;
        var randomTicks = (long)(Random.Shared.NextDouble() * range);

        return minValue.AddTicks(randomTicks);
    }

    public static DateTimeOffset NextDateTimeOffset(DateTimeOffset minValue, DateTimeOffset maxValue)
    {
        var range = (maxValue - minValue).Ticks;
        var randomTicks = (long)(Random.Shared.NextDouble() * range);

        return minValue.AddTicks(randomTicks);
    }

    public static TimeSpan NextTimeSpan(TimeSpan minValue, TimeSpan maxValue)
    {
        var range = (maxValue - minValue).Ticks;
        var randomTicks = (long)(Random.Shared.NextDouble() * range);

        return minValue + TimeSpan.FromTicks(randomTicks);
    }

    // ── Seleção de coleções ───────────────────────────────────────────────

    public static TEnum GetRandomEnum<TEnum>(IEnumerable<TEnum>? except = null) where TEnum : struct, Enum
    {
        except ??= [];
        var values = Enum.GetValues<TEnum>().Except(except).ToArray();

        return Random.Shared.GetItems(values, 1)[0];
    }

    public static T GetRandomItem<T>(params T[] items)
        => Random.Shared.GetItems(items, 1)[0];

    public static T GetRandomItem<T>(IReadOnlyList<T> items)
        => items[Random.Shared.Next(items.Count)];

    public static T[] GetRandomItems<T>(T[] items, int count)
        => Random.Shared.GetItems(items, count);

    public static List<T> Shuffled<T>(IEnumerable<T> items)
    {
        var list = items.ToList();
        Random.Shared.Shuffle(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list));

        return list;
    }
}