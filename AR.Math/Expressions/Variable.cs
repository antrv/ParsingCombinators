using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AR.Math.Expressions
{
	[Serializable]
	public sealed class Variable: Expression
	{
		private readonly string _id;
		private readonly string _name;
		private readonly IReadOnlyList<Expression> _indexes;

		public Variable(string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (name.Length == 0)
				throw new ArgumentException("Name should not be empty", nameof(name));
			_name = name;
			_id = name;
			_indexes = ReadOnlyList<Expression>.Empty;
		}

		public Variable(string name, params Expression[] indexes)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (name.Length == 0)
				throw new ArgumentException("Name should not be empty", nameof(name));
			_name = name;

			if (indexes == null || indexes.Length == 0)
			{
				_id = name;
				_indexes = ReadOnlyList<Expression>.Empty;
			}
			else
			{
				if (indexes.Any(x => x == null))
					throw new ArgumentException("Index expression cannot be null.", nameof(indexes));
				_indexes = new ReadOnlyList<Expression>(indexes);
				_id = GenerateId();
			}
		}

		public Variable(string name, IReadOnlyList<Expression> indexes)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (name.Length == 0)
				throw new ArgumentException("Name should not be empty", nameof(name));
			_name = name;

			if (indexes == null || indexes.Count == 0)
			{
				_id = name;
				_indexes = ReadOnlyList<Expression>.Empty;
			}
			else
			{
				if (indexes.Any(x => x == null))
					throw new ArgumentException("Index expression cannot be null.", nameof(indexes));
				_indexes = indexes;
				_id = GenerateId();
			}
		}

		private string GenerateId()
		{
			StringBuilder sb = new StringBuilder(_name);
			sb.Append("[");
			_indexes[0].ToString(sb);
			for (int i = 1, count = _indexes.Count; i < count; i++)
			{
				sb.Append(", ");
				_indexes[i].ToString(sb);
			}
			sb.Append("]");
			return sb.ToString();
		}

		public string Id => _id;
	    public string Name => _name;
	    public IReadOnlyList<Expression> Indexes => _indexes;

	    public override void ToString(StringBuilder sb)
		{
			sb.Append(_id);
		}

		public override string ToString()
		{
			return _id;
		}
	}
}