namespace AR.Math.Expressions.Transformations
{
	public interface IExpressionTransformation<out TOutput>
	{
		TOutput Transform(Expression expression);
	}
}
