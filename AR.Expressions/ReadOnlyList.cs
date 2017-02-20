using System.Collections;
using System.Collections.Generic;

namespace AR.Expressions
{
    public sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        public static readonly ReadOnlyList<T> Empty = new ReadOnlyList<T>(ArrayHelper<T>.Empty);

        private readonly T[] _items;

        public ReadOnlyList(params T[] items)
        {
            _items = items ?? ArrayHelper<T>.Empty;
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => _items.Length;
        public T this[int index] => _items[index];
    }
}