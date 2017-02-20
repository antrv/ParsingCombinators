using AR.Expressions.Transformations;

namespace AR.Expressions
{
	public static class Extensions
	{
		
		public static FunctionExpression Power(this Expression expression, Expression power)
		{
			return Operators.Power.Apply(expression, power);
		}

		
		public static FunctionExpression Differentiate(this Expression expression, Expression direction)
		{
			return Operators.Diff.Apply(expression, direction);
		}

		public static Expression Simplify(this Expression expression)
		{
            return SimplificationTransformation.Instance.Transform(expression);
		}
	}
}