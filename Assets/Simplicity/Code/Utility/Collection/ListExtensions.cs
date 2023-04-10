using System;
using System.Collections.Generic;

namespace Simplicity.Utility.Collections
{
    public static class ListExtensions
    {
        private static readonly Random rng = new Random();

        public static void FisherYatesShuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}