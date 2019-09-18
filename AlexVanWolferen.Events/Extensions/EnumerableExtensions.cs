namespace AlexVanWolferen.Events.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }
    }
}