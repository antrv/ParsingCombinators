using System;
using System.Numerics;
using System.Text;

namespace AR.Math.Expressions
{
	[Serializable]
	public abstract class Expression
	{
		public abstract void ToString(StringBuilder stringBuilder);

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			ToString(sb);
			return sb.ToString();
		}

		public static implicit operator Expression(BigInteger value)
		{
			return new Number(value);
		}

		public static implicit operator Expression(Rational value)
		{
			return new Number(value);
		}

		public static implicit operator Expression(int value)
		{
			return new Number(value);
		}

		public static implicit operator Expression(long value)
		{
			return new Number(value);
		}

		public static implicit operator Expression(uint value)
		{
			return new Number(value);
		}

		public static implicit operator Expression(ulong value)
		{
			return new Number(value);
		}

	    public static implicit operator Expression(double value)
	    {
	        return new Number(value);
	    }

	    public static implicit operator Expression(float value)
		{
	        return new Number(value);
		}

		public static implicit operator Expression(decimal value)
		{
		    return new Number(value);
		}

		public static Expression operator +(Expression expression)
		{
			return expression;
		}

		public static Expression operator +(Expression expression, Expression addent)
		{
			return new FunctionExpression(Operators.Add, expression, addent);
		}

		public static Expression operator -(Expression expression)
		{
			return new FunctionExpression(Operators.Negate, expression);
		}

		public static Expression operator -(Expression expression, Expression otherExpression)
		{
			return new FunctionExpression(Operators.Subtract, expression, otherExpression);
		}

		public static Expression operator *(Expression expression, Expression otherExpression)
		{
			return new FunctionExpression(Operators.Multiply, expression, otherExpression);
		}

		public static Expression operator /(Expression expression, Expression otherExpression)
		{
			return new FunctionExpression(Operators.Divide, expression, otherExpression);
		}
	}
}