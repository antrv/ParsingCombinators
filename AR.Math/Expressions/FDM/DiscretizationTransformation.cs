using System;
using AR.Math.Expressions.Transformations;

namespace AR.Math.Expressions.FDM
{
	public sealed class DiscretizationTransformation: ExpressionTransformation<Expression>
	{
		private readonly Function _function;
		private readonly Variable _variable;

		private readonly Variable _index;
		private readonly Variable _discreteFunction;
		private readonly Variable _discreteVariable;

		public DiscretizationTransformation(Function function, Variable variable)
		{
			if (function == null)
				throw new ArgumentNullException(nameof(function));
			if (variable == null)
				throw new ArgumentNullException(nameof(variable));
			_function = function;
			_variable = variable;

			_index = new Variable("i");
			_discreteVariable = new Variable(variable.Name, _index);
			//_discreteFunction = new Variable();
		}

		protected override Expression TransformConstant(Constant constant)
		{
			return constant;
		}

		protected override Expression TransformNumber(Number number)
		{
			return number;
		}

		protected override Expression TransformVariable(Variable variable)
		{
			if (variable == _variable)
			{
				//return new VariableExpression(new Variable());
			}
			throw new NotImplementedException();
		}

		protected override Expression TransformFunction(FunctionExpression function)
		{
			throw new NotImplementedException();
		}
	}
}
