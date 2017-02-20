using System;

namespace AR.Math.Expressions.Transformations
{
    [Serializable]
	public abstract class ExpressionTransformation<TOutput>: IExpressionTransformation<TOutput>
	{
		public virtual TOutput Transform(Expression expression)
		{
			Number number = expression as Number;
			if (number != null)
				return TransformNumber(number);

			Constant constant = expression as Constant;
			if (constant != null)
				return TransformConstant(constant);

			Variable variableExpression = expression as Variable;
			if (variableExpression != null)
				return TransformVariable(variableExpression);

			FunctionExpression functionExpression = expression as FunctionExpression;
			if (functionExpression != null)
				return TransformFunction(functionExpression);

			throw new ArgumentException("Invalid expression");
		}

        protected abstract TOutput TransformNumber(Number number);
		protected abstract TOutput TransformConstant(Constant constant);
		protected abstract TOutput TransformVariable(Variable variable);
		protected abstract TOutput TransformFunction(FunctionExpression functionExpression);
	}
}