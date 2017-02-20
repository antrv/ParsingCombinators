using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Expressions
{
	[Serializable]
	public abstract class Function
	{
		private readonly string _name;

		protected Function(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentException("Name should not be empty.", nameof(name));
			_name = name;
		}

	    public string Name => _name;
	    public string Id => _name;
    
	    public virtual void ValidateArguments(IReadOnlyList<Expression> arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Any(x => x == null))
				throw new ArgumentException("Argument list cannot contain nulls.", nameof(arguments));
		}

		public abstract double Evaluate(params double[] arguments);

		public override string ToString()
		{
			return _name + "()";
		}

		public FunctionExpression Apply(params Expression[] arguments)
		{
			return new FunctionExpression(this, arguments);
		}

		public FunctionExpression Apply(IEnumerable<Expression> arguments)
		{
			IReadOnlyList<Expression> args = arguments.ToArray();
			return new FunctionExpression(this, args);
		}

		public FunctionExpression Apply(IReadOnlyList<Expression> arguments)
		{
			return new FunctionExpression(this, arguments);
		}
	}
}