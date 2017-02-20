using System;
using System.Collections.Generic;

namespace AR.Math.Parsing
{
	public sealed class EnumerableInput<T>: IParserInput<T>
	{
		private readonly bool _eof;
		private readonly int _position;
		private readonly T _current;
		private IParserInput<T> _next;
		private readonly IEnumerator<T> _enumerator;

		public EnumerableInput(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			_enumerator = collection.GetEnumerator();
			if (_enumerator.MoveNext())
				_current = _enumerator.Current;
			else
			{
				_eof = true;
				_enumerator.Dispose();
			}
		}

		private EnumerableInput(IEnumerator<T> enumerator, int position)
		{
			_enumerator = enumerator;
			_position = position;

			if (_enumerator.MoveNext())
				_current = _enumerator.Current;
			else
			{
				_eof = true;
				_enumerator.Dispose();
			}
		}

		public int Position => _position;
	    public T Current => _current;
	    public bool Eof => _eof;

	    public IParserInput<T> Next
		{
			get
			{
			    if (_eof)
					return this;
			    return _next ?? (_next = new EnumerableInput<T>(_enumerator, _position + 1));
			}
		}
	}
}