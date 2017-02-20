using System;
using System.Numerics;
using System.Text;

namespace AR.Expressions
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
	        // http://en.wikipedia.org/wiki/Double_precision_floating-point_format

	        long val = BitConverter.DoubleToInt64Bits(value);
	        int exp = ((int)(val >> 52)) & 0x7FF;
	        long frac = val & 0x000FFFFFFFFFFFFF;

	        // exclusive cases
	        if (exp == 0x7FF)
	        {
	            if (frac == 0) // Infinity
	            {
	                if (val < 0)
	                    return Constants.NegativeInfinity;
	                return Constants.PositiveInfinity;
	            }
	            throw new ArgumentException("Cannot convert NaN to expression", nameof(value));
	        }
	        if (exp == 0 && frac == 0) // Zero
	            return 0;

	        if (exp != 0) // Normalized
	            frac |= 0x0010000000000000; // +2^52

	        exp = exp - 0x3FF - 52;
	        if (exp == 0)
	            return frac;

	        if (exp > 0)
	            return frac * BigInteger.Pow(2, exp);
	        return new Number(new Rational(frac, BigInteger.Pow(2, -exp)));
	    }

	    public static implicit operator Expression(float value)
		{
            // https://en.wikipedia.org/wiki/Single-precision_floating-point_format

	        int val = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
	        int exp = (val >> 23) & 0xFF;
	        int frac = val & 0x7FFFFF;

	        // exclusive cases
	        if (exp == 0xFF)
	        {
	            if (frac == 0) // Infinity
	            {
	                if (val < 0)
	                    return Constants.NegativeInfinity;
	                return Constants.PositiveInfinity;
	            }
	            throw new ArgumentException("Cannot convert NaN to expression", nameof(value));
	        }
	        if (exp == 0 && frac == 0) // Zero
	            return 0;

	        if (exp != 0) // Normalized
	            frac |= 0x800000; // +2^23

	        exp = exp - 0x7F - 23;
	        if (exp == 0)
	            return frac;

	        if (exp > 0)
	            return frac * BigInteger.Pow(2, exp);
	        return new Number(new Rational(frac, BigInteger.Pow(2, -exp)));
		}

		public static implicit operator Expression(decimal value)
		{
    		int[] bits = decimal.GetBits(value);

		    BigInteger result = (uint)bits[2];
	        result = (result << 32) + (uint)bits[1];
	        result = (result << 32) + (uint)bits[0];

			bool sign = bits[3] < 0;
			int exponent = (bits[3] & 0x00FF0000) >> 16;

		    if (sign)
		        result = -result;

		    return new Number(new Rational(result, BigInteger.Pow(10, exponent)));
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