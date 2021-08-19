using System;
using System.Collections.Generic;

namespace Maxsys.Core.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random _randomGenerator = new Random();

        public static int GetInt(int min, int max)
        {
            lock (_randomGenerator) // synchronize
            {
                return _randomGenerator.Next(min, max);
            }
        }

        public static double GetDouble(double min, double max)
        {
            lock (_randomGenerator) // synchronize
            {
                return _randomGenerator.NextDouble() * (max - min) + min;
            }
        }

        public static double GetDouble()
        {
            lock (_randomGenerator) // synchronize
            {
                return _randomGenerator.NextDouble();
            }
        }

        public static double GetMargin(double margin)
        {
            lock (_randomGenerator) // synchronize
            {
                // same as: NextDouble() * (margin - (-margin)) + (-margin);
                return _randomGenerator.NextDouble() * (margin + margin) - margin;
            }
        }

        public static double GetMarginFromNumber(double number, double margin)
        {
            return number + GetMargin(margin);
        }

        public static T GetRandomItem<T>(this IList<T> list)
        {
            lock (_randomGenerator) // synchronize
            {
                return list[_randomGenerator.Next(0, list.Count)];
            }
        }
    }
}