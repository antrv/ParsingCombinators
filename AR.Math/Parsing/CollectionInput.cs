using System;
using System.Collections.Generic;

namespace AR.Math.Parsing
{
	public sealed class CollectionInput<T>: IParserInput<T>
	{
		private readonly int _position;
		private readonly IReadOnlyList<T> _collection;
	    private IParserInput<T> _next;

		public CollectionInput(IReadOnlyList<T> collection)
		{
		    if (collection == null)
                throw new ArgumentNullException(nameof(collection));
		    _collection = collection;
		}

		public CollectionInput(IReadOnlyList<T> collection, int position)
		{
		    if (collection == null)
                throw new ArgumentNullException(nameof(collection));
		    _position = position;
			_collection = collection;
		}

		public int Position => _position;
	    public T Current => _collection[_position];
	    public bool Eof => _position >= _collection.Count;
	    public IParserInput<T> Next => _next ?? (_next = new CollectionInput<T>(_collection, _position + 1));
	}
}