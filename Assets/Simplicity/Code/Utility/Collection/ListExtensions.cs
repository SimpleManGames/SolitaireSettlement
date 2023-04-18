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

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new NullReferenceException("List source was null when calling List.ForEach");

            if (action == null)
                throw new NullReferenceException("Action was null when calling List.ForEach");

            foreach (var item in source)
                action(item);
        }
    }
}