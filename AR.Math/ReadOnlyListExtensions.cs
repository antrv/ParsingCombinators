using System.Collections.Generic;
using System.Linq;

namespace AR.Math
{
    public static class ReadOnlyListExtensions
    {
        public static IReadOnlyList<T> Add<T>(this IReadOnlyList<T> list, T item)
        {
            T[] items = new T[list.Count + 1];
            list.CopyTo(items, 0);
            items[list.Count] = item;
            return new ReadOnlyList<T>(items);
        }

        public static IReadOnlyList<T> AddRange<T>(this IReadOnlyList<T> list, IEnumerable<T> collection)
        {
            return new ReadOnlyList<T>(list.Concat(collection).ToArray());
        }

        public static void CopyTo<T>(this IReadOnlyList<T> list, T[] array, int startIndex)
        {
            for (int i = 0, count = list.Count; i < count; i++)
                array[startIndex + i] = list[i];
        }

        public static IReadOnlyList<T> SetItem<T>(this IReadOnlyList<T> list, int index, T value)
        {
             T[] items = new T[list.Count];
            list.CopyTo(items, 0);
            items[index] = value;
            return new ReadOnlyList<T>(items);
       }
    }
}