using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Math.Expressions.Transformations
{
	public sealed class SubstituteTransformation: ExpressionTransformation<Expression>
	{
		private readonly Dictionary<Variable, Expression> _variables;

        public SubstituteTransformation(params VariableValuePair[] values)
            : this((IReadOnlyList<VariableValuePair>)values)
        {
        }

        public SubstituteTransformation(IReadOnlyList<VariableValuePair> values)
        {
            if (values == null)
				throw new ArgumentNullException(nameof(values));
			if (values.Count == 0)
				throw new ArgumentException("Variable list cannot be empty", nameof(values));
			if (values.Any(x => x.Variable == null || x.Value == null))
				throw new ArgumentException("Variable list contains nulls", nameof(values));

			_variables = values.ToDictionary(p => p.Variable, p => p.Value);
		}

        protected override Expression TransformNumber(Number number) => number;
        protected override Expression TransformConstant(Constant constant) => constant;

        protected override Expression TransformVariable(Variable variable)
		{
            if (_variables.TryGetValue(variable, out Expression expression))
                return expression;
            return variable;
		}

        protected override Expression TransformFunction(FunctionExpression function) => new FunctionExpression(function.Function, function.Select(Transform).ToArray());
    }
}