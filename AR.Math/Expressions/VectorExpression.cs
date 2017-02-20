using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AR.Math.Expressions
{
	[Serializable]
	public sealed class VectorExpression: Expression
	{
		private readonly IReadOnlyList<Expression> _elements;

		public VectorExpression(params Expression[] elements): 
			this(new ReadOnlyList<Expression>(elements))
		{
		}

		public VectorExpression(IReadOnlyList<Expression> elements)
		{
			if (elements == null)
				throw new ArgumentNullException(nameof(elements));
			if (elements.Any(x => x == null))
				throw new ArgumentException("Elements array contains nulls.", nameof(elements));
			_elements = elements;
		}

	    public IReadOnlyList<Expression> Elements => _elements;

	    public override void ToString(StringBuilder stringBuilder)
		{
			stringBuilder.Append("[");
			int count = _elements.Count;
			if (count > 0)
			{
				_elements[0].ToString(stringBuilder);
				for (int i = 1; i < count; i++)
				{
					stringBuilder.Append(", ");
					_elements[i].ToString(stringBuilder);
				}
			}
			stringBuilder.Append("]");
		}
	}
}