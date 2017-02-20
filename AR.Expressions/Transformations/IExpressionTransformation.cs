namespace AR.Expressions.Transformations
{
	public interface IExpressionTransformation<out TOutput>
	{
		TOutput Transform(Expression expression);
	}
}
