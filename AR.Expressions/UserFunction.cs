using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Expressions
{
	[Serializable]
	public sealed class UserFunction: Function
	{
		private readonly IReadOnlyList<Variable> _arguments;
		private readonly Expression _expression;

		public UserFunction(string name, Func<Variable, Expression> function)
			: base(name)
		{
			Variable x = new Variable("x");
			_arguments = new ReadOnlyList<Variable>(x);
			_expression = function(x);
		}

		public UserFunction(string name, Func<Variable, Variable, Expression> function)
			: base(name)
		{
			Variable x = new Variable("x");
			Variable y = new Variable("y");
			_arguments = new ReadOnlyList<Variable>(x, y);
			_expression = function(x, y);
		}

		public UserFunction(string name, Variable argument, Expression expression)
			: base(name)
		{
			if (argument == null)
				throw new ArgumentNullException(nameof(argument));
			if (expression == null)
				throw new ArgumentNullException(nameof(expression));
			_arguments = new ReadOnlyList<Variable>(argument);
			_expression = expression;
		}

		public UserFunction(string name, Variable argument1, Variable argument2,
			Expression expression)
			: base(name)
		{
			if (argument1 == null)
				throw new ArgumentNullException(nameof(argument1));
			if (argument2 == null)
				throw new ArgumentNullException(nameof(argument2));
			if (expression == null)
				throw new ArgumentNullException(nameof(expression));
			_arguments = new ReadOnlyList<Variable>(argument1, argument2);
			_expression = expression;
		}

		public UserFunction(string name, Variable argument1, Variable argument2,
			Variable argument3, Expression expression)
			: base(name)
		{
			if (argument1 == null)
				throw new ArgumentNullException(nameof(argument1));
			if (argument2 == null)
				throw new ArgumentNullException(nameof(argument2));
			if (argument3 == null)
				throw new ArgumentNullException(nameof(argument3));
			if (expression == null)
				throw new ArgumentNullException(nameof(expression));
			_arguments = new ReadOnlyList<Variable>(argument1, argument2, argument3);
			_expression = expression;
		}

		public UserFunction(string name, Expression expression, params Variable[] arguments)
			: base(name)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Length == 0)
				throw new ArgumentException("Function should have arguments.", nameof(arguments));
			if (arguments.Any(x => x == null))
				throw new ArgumentException("Arguments should not contain null.", nameof(arguments));
			if (expression == null)
				throw new ArgumentNullException(nameof(expression));
			_expression = expression;
			_arguments = new ReadOnlyList<Variable>(arguments);
		}

		public override void ValidateArguments(IReadOnlyList<Expression> arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Count != _arguments.Count)
				throw new ArgumentException("Invalid arguments count.", nameof(arguments));
			if (arguments.Any(x => x == null))
				throw new ArgumentException("Argument list cannot contain nulls.", nameof(arguments));
		}

		public Expression Expression => _expression;
		public IReadOnlyList<Variable> Arguments => _arguments;

	    public override double Evaluate(params double[] arguments)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return Name + "(" + string.Join(", ", _arguments) + ")";
		}
	}
}