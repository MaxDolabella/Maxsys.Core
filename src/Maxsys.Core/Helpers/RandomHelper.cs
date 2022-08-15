using System;
using System.Collections.Generic;

namespace Maxsys.Core.Helpers;

public static class RandomHelper
{
    public static int GetInt(int min, int max)
        => Random.Shared.Next(min, max);

    public static double GetDouble(double min, double max)
        => Random.Shared.NextDouble() * (max - min) + min;

    public static double GetDouble()
       => Random.Shared.NextDouble();

    public static double GetMargin(double margin)
            // same as: NextDouble() * (margin - (-margin)) + (-margin);
            => Random.Shared.NextDouble() * (margin + margin) - margin;

    public static double GetMarginFromNumber(double number, double margin)
        => number + GetMargin(margin);

    public static T GetRandomItem<T>(this IList<T> list)
            => list[Random.Shared.Next(0, list.Count)];
}