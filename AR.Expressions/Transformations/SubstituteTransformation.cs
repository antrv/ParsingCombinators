using System;
using System.Collections.Generic;

namespace AR.Expressions.Transformations
{
	public sealed class SubstituteTransformation: ExpressionTransformation<Expression>
	{
		private readonly Dictionary<Variable, Expression> _variables;

		public SubstituteTransformation(IReadOnlyList<Variable> variables, IReadOnlyList<Expression> expressions)
		{
			if (variables == null)
				throw new ArgumentNullException(nameof(variables));
			if (expressions == null)
				throw new ArgumentNullException(nameof(expressions));
			if (variables.Count == 0)
				throw new ArgumentException("Variable list is empty.", nameof(variables));
			if (expressions.Count == 0)
				throw new ArgumentException("Expression list is empty.", nameof(expressions));
			if (variables.Any(x => x == null))
				throw new ArgumentException("Variable list contains nulls.", nameof(variables));
			if (expressions.Any(x => x == null))
				throw new ArgumentException("Expression list contains nulls.", nameof(expressions));
			if (variables.Count != expressions.Count)
				throw new ArgumentException("Expression list length must be equal to variables array length.", nameof(expressions));

			_variables = new Dictionary<Variable, Expression>();
			for (int i = 0, count = variables.Count; i < count; i++)
				_variables.Add(variables[i], expressions[i]);
		}

		protected override Expression TransformNumber(Number number)
		{
			return number;
		}

	    protected override Expression TransformConstant(Constant constant)
	    {
	        return constant;
	    }

	    protected override Expression TransformVariable(Variable variable)
		{
			Expression expression;
			if (_variables.TryGetValue(variable, out expression))
				return expression;
			return variable;
		}

		protected override Expression TransformFunction(FunctionExpression function)
		{
			return new FunctionExpression(function.Function, function.ToArray(Transform));
		}
	}
}