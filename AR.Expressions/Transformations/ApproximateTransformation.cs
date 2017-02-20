using System;

namespace AR.Expressions.Transformations
{
	public sealed class ApproximateTransformation: ExpressionTransformation<double>
	{
		protected override double TransformNumber(Number number)
		{
			throw new NotImplementedException();
			//return constant.ApproximatedValue;
		}

	    protected override double TransformConstant(Constant constant)
	    {
			throw new NotImplementedException();
	    }

	    protected override double TransformVariable(Variable variable)
		{
			throw new NotSupportedException("Approximated expression should not contain variables.");
		}

		protected override double TransformFunction(FunctionExpression function)
		{
			return function.Function.Evaluate(function.ToArray(Transform));
		}
	}
}