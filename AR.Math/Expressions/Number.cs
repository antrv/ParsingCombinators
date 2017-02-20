using System;
using System.Numerics;
using System.Text;

namespace AR.Math.Expressions
{
    [Serializable]
	public sealed class Number: Expression
	{
        private readonly Rational _value;

        public Number(BigInteger value)
        {
            _value = value;
        }

        public Number(Rational value)
        {
            if (value.IsInfinity)
                throw new ArgumentOutOfRangeException(nameof(value));
            _value = value;
        }

        public Rational Value => _value;

        public override void ToString(StringBuilder stringBuilder)
        {
            stringBuilder.Append(_value);
        }
	}
}