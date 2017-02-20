using System;
using System.Numerics;

namespace AR.Math
{
	[Serializable]
	public struct Rational: IEquatable<Rational>, IComparable<Rational>, IFormattable
	{
		private readonly BigInteger _dividend;
		private readonly BigInteger _divisor;

		public static readonly Rational One = new Rational(BigInteger.One);
		public static readonly Rational Zero = new Rational(BigInteger.Zero);
		public static readonly Rational PositiveInfinity = new Rational(BigInteger.One, BigInteger.Zero, false);
		public static readonly Rational NegativeInfinity = new Rational(BigInteger.MinusOne, BigInteger.Zero, false);
		public static readonly Rational NaN = new Rational(BigInteger.Zero, BigInteger.Zero, false);

		#region Constructors

		public Rational(BigInteger value)
		{
			_dividend = value;
			_divisor = BigInteger.One;
		}

		private Rational(BigInteger dividend, BigInteger divisor, bool dummy)
		{
			_dividend = dividend;
			_divisor = divisor;
		}

		public Rational(BigInteger dividend, BigInteger divisor)
		{
			if (divisor.IsZero)
			{
				_divisor = BigInteger.Zero;
				_dividend = dividend.Sign > 0 ? BigInteger.One : (dividend.Sign < 0 ? BigInteger.MinusOne : BigInteger.Zero);
			}
			else
			{
				BigInteger gcd = BigInteger.GreatestCommonDivisor(dividend, divisor);
				if (divisor.Sign < 0)
				{
					if (gcd > 1)
					{
						_dividend = -dividend / gcd;
						_divisor = -divisor / gcd;
					}
					else
					{
						_dividend = -dividend;
						_divisor = -divisor;
					}
				}
				else if (gcd > 1)
				{
					_dividend = dividend / gcd;
					_divisor = divisor / gcd;
				}
				else
				{
					_dividend = dividend;
					_divisor = divisor;
				}
			}
		}

		#endregion Constructors

		#region Public properties

		public BigInteger Dividend
		{
			get
			{
				return _dividend;
			}
		}

		public BigInteger Divisor
		{
			get
			{
				return _divisor;
			}
		}

		public bool IsZero
		{
			get
			{
				return _dividend.IsZero && !_divisor.IsZero;
			}
		}

		public bool IsOne
		{
			get
			{
				return _dividend.IsOne && _divisor.IsOne;
			}
		}

		public bool IsInteger
		{
			get
			{
				return _divisor.IsOne;
			}
		}

		public bool IsPositiveInfinity
		{
			get
			{
				return _dividend.Sign > 0 && _divisor.IsZero;
			}
		}

		public bool IsNegativeInfinity
		{
			get
			{
				return _dividend.Sign < 0 && _divisor.IsZero;
			}
		}

		public bool IsInfinity
		{
			get
			{
				return _divisor.IsZero && !_dividend.IsZero;
			}
		}

		public bool IsNaN
		{
			get
			{
				return _dividend.IsZero && _divisor.IsZero;
			}
		}

		#endregion Public properties

		#region Overrides and interfaces implementation

		public override string ToString()
		{
			if (_divisor.IsOne)
				return _dividend.ToString();
			if (_divisor.IsZero)
				if (_dividend.IsZero)
					return "NaN";
				else if (_dividend > 0)
					return "Infinity";
				else 
					return "-Infinity";
			return string.Format("{0}/{1}", _dividend, _divisor);
		}

		public string ToString(string format)
		{
			if (_divisor.IsOne)
				return _dividend.ToString(format);
			if (_divisor.IsZero)
				if (_dividend.IsZero)
					return "NaN";
				else if (_dividend > 0)
					return "Infinity";
				else
					return "-Infinity";
			return string.Format("{0}/{1}", _dividend.ToString(format), _divisor.ToString(format));
		}

		public string ToString(IFormatProvider formatProvider)
		{
			if (_divisor.IsOne)
				return _dividend.ToString(formatProvider);
			if (_divisor.IsZero)
				if (_dividend.IsZero)
					return "NaN";
				else if (_dividend > 0)
					return "Infinity";
				else
					return "-Infinity";
			return string.Format("{0}/{1}", _dividend.ToString(formatProvider), _divisor.ToString(formatProvider));
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (_divisor.IsOne)
					return _dividend.ToString(format, formatProvider);
			if (_divisor.IsZero)
				if (_dividend.IsZero)
					return "NaN";
				else if (_dividend > 0)
					return "Infinity";
				else
					return "-Infinity";
			return string.Format("{0}/{1}", _dividend.ToString(format, formatProvider), _divisor.ToString(format, formatProvider));
		}

		public override int GetHashCode()
		{
			return _dividend.GetHashCode() ^ _divisor.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, null))
				return false;

			Type type = obj.GetType();
			if (type == typeof(Rational))
				return Equals((Rational)obj);
			if (type == typeof(BigInteger))
				return Equals(new Rational((BigInteger)obj));

			return false;
		}

		public bool Equals(Rational other)
		{
			return _dividend == other._dividend && _divisor == other._divisor;
		}

		public bool Equals(BigInteger other)
		{
			return _dividend == other && _divisor == 1;
		}

		public int CompareTo(Rational other)
		{
			if (IsNaN && !other.IsNaN)
				return -1;
			if (other.IsNaN && !IsNaN)
				return 1;
			if (_divisor == other._divisor)
				return _dividend.CompareTo(other._dividend);
			if (_dividend.Sign != other._dividend.Sign)
				return _dividend.CompareTo(other._dividend);
			return (_dividend * other._divisor).CompareTo(other._dividend * _divisor);
		}

		#endregion Overrides and interfaces implementation

		#region Conversion

		public static implicit operator Rational(int value)
		{
			return new Rational(new BigInteger(value));
		}

		public static implicit operator Rational(long value)
		{
			return new Rational(new BigInteger(value));
		}

		public static implicit operator Rational(uint value)
		{
			return new Rational(new BigInteger(value));
		}

		public static implicit operator Rational(ulong value)
		{
			return new Rational(new BigInteger(value));
		}

		public static implicit operator Rational(BigInteger value)
		{
			return new Rational(value);
		}

		public static implicit operator Rational(decimal value)
		{
			int[] bits = Decimal.GetBits(value);
			ulong high = (((ulong)bits[2]) << 32) + (uint)bits[1];
			BigInteger frac = new BigInteger(high);
			frac = (frac << 32) + (uint)bits[0];
			int exp = (bits[3] >> 16) & 0xFF;
			BigInteger div = BigInteger.One;
			if (exp > 0)
				div = BigInteger.Pow(10, exp);
			if (bits[3] < 0)
				return new Rational(-frac, div);
			return new Rational(frac, div);
		}

		public static implicit operator Rational(double value)
		{
			// http://en.wikipedia.org/wiki/Double_precision_floating-point_format

			ulong val = (ulong)BitConverter.DoubleToInt64Bits(value);
			bool sign = (val & 0x8000000000000000ul) != 0;
			int exp = ((int)(val >> 52)) & 0x7FF;
			ulong frac = val & 0x000FFFFFFFFFFFFFul;

			// exclusive cases
			if (exp == 0x7FFu)
				if (frac == 0) // Inf
					return sign ? NegativeInfinity : PositiveInfinity;
				else // NaN
					return NaN;
			if (exp == 0)
				if (frac == 0) // Zero
					return Zero;
				else // Denormalized
					if (sign)
						return new Rational(-(new BigInteger(frac)), BigInteger.Pow(2, 1022 + 52));
					else
						return new Rational(new BigInteger(frac), BigInteger.Pow(2, 1022 + 52));

			exp -= (1023 + 52);
			frac |= 0x0010000000000000ul; // +2^52
			BigInteger mantissa = sign ? (-(new BigInteger(frac))) : (new BigInteger(frac));
			if (exp == 0)
				return new Rational(mantissa);
			if (exp > 0)
				return new Rational(mantissa * BigInteger.Pow(2, exp));
			return new Rational(mantissa, BigInteger.Pow(2, -exp));
		}

		public double ToDouble()
		{
			return (double)_dividend / (double)_divisor;
		}

		#endregion Conversion

		#region Arithmetic operators

		public static Rational operator +(Rational value)
		{
			return value;
		}

		public static Rational operator -(Rational value)
		{
			return new Rational(-value._dividend, value._divisor, false);
		}

		public static Rational operator +(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return NaN;
			if (value1.IsPositiveInfinity)
				if (value2.IsNegativeInfinity)
					return NaN;
				else
					return PositiveInfinity;
			if (value1.IsNegativeInfinity)
				if (value2.IsPositiveInfinity)
					return NaN;
				else
					return NegativeInfinity;

			if (value1._divisor == value2._divisor)
				return new Rational(value1._dividend + value2._dividend, value1._divisor);
			return new Rational(value1._dividend * value2._divisor + value2._dividend * value1._divisor, value1._divisor * value2._divisor);
		}

		public static Rational operator -(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return NaN;
			if (value1.IsPositiveInfinity)
				if (value2.IsPositiveInfinity)
					return NaN;
				else
					return PositiveInfinity;
			if (value1.IsNegativeInfinity)
				if (value2.IsNegativeInfinity)
					return NaN;
				else
					return NegativeInfinity;
			if (value1._divisor == value2._divisor)
				return new Rational(value1._dividend - value2._dividend, value1._divisor);
			return new Rational(value1._dividend * value2._divisor - value2._dividend * value1._divisor, value1._divisor * value2._divisor);
		}

		public static Rational operator *(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return NaN;
			if (value1.IsInfinity && value2.IsZero)
				return NaN;
			if (value1.IsZero && value2.IsInfinity)
				return NaN;
			BigInteger gcd1 = BigInteger.GreatestCommonDivisor(value1._dividend, value2._divisor);
			BigInteger gcd2 = BigInteger.GreatestCommonDivisor(value2._dividend, value1._divisor);
			if (gcd1 > 1)
				if (gcd2 > 1)
					return new Rational((value1._dividend / gcd1) * (value2._dividend / gcd2), (value1._divisor / gcd2) * (value2._divisor / gcd1));
				else
					return new Rational((value1._dividend / gcd1) * (value2._dividend), (value1._divisor) * (value2._divisor / gcd1));
			if (gcd2 > 1)
				return new Rational((value1._dividend) * (value2._dividend / gcd2), (value1._divisor / gcd2) * (value2._divisor));
			return new Rational(value1._dividend * value2._dividend, value1._divisor * value2._divisor);
		}

		public static Rational operator /(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return NaN;
			if (value1.IsInfinity && value2.IsInfinity)
				return NaN;
			if (value1.IsZero && value2.IsZero)
				return NaN;
			BigInteger gcd1 = BigInteger.GreatestCommonDivisor(value1._dividend, value2._dividend);
			BigInteger gcd2 = BigInteger.GreatestCommonDivisor(value1._divisor, value2._divisor);
			if (gcd1 > 1)
				if (gcd2 > 1)
					return new Rational((value1._dividend / gcd1) * (value2._divisor / gcd2), (value2._dividend / gcd1) * (value1._divisor / gcd2));
				else
					return new Rational((value1._dividend / gcd1) * (value2._divisor), (value2._dividend / gcd1) * (value1._divisor));
			if (gcd2 > 1)
				return new Rational((value1._dividend) * (value2._divisor / gcd2), (value2._dividend) * (value1._divisor / gcd2));
			return new Rational(value1._dividend * value2._divisor, value2._dividend * value1._divisor);
		}

		public static bool operator ==(Rational value1, Rational value2)
		{
			return value1._dividend == value2._dividend && value1._divisor == value2._divisor;
		}

		public static bool operator !=(Rational value1, Rational value2)
		{
			return value1._dividend != value2._dividend || value1._divisor != value2._divisor;
		}

		public static bool operator >(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return false;
			if (value1._divisor == value2._divisor)
				return value1._dividend > value2._dividend;
			return value1._dividend * value2._divisor > value1._divisor * value2._dividend;
		}

		public static bool operator <(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return false;
			if (value1._divisor == value2._divisor)
				return value1._dividend < value2._dividend;
			return value1._dividend * value2._divisor < value1._divisor * value2._dividend;
		}

		public static bool operator >=(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return false;
			if (value1._divisor == value2._divisor)
				return value1._dividend >= value2._dividend;
			return value1._dividend * value2._divisor >= value1._divisor * value2._dividend;
		}

		public static bool operator <=(Rational value1, Rational value2)
		{
			if (value1.IsNaN || value2.IsNaN)
				return false;
			if (value1._divisor == value2._divisor)
				return value1._dividend <= value2._dividend;
			return value1._dividend * value2._divisor <= value1._divisor * value2._dividend;
		}

		#endregion Arithmetic operators

		#region Arithmetic functions

		public static Rational Abs(Rational value)
		{
			if (value._dividend < 0)
				return new Rational(BigInteger.Abs(value._dividend), value._divisor, false);
			return value;
		}

		public static Rational Min(Rational value1, Rational value2)
		{
			return value1.CompareTo(value2) <= 0 ? value1 : value2;
		}

		public static Rational Max(Rational value1, Rational value2)
		{
			return value1.CompareTo(value2) >= 0 ? value1 : value2;
		}

		public static Rational Pow(Rational value, int exponent)
		{
			return new Rational(BigInteger.Pow(value._dividend, exponent), BigInteger.Pow(value._divisor, exponent));
		}

		public static double Log(Rational value)
		{
			return BigInteger.Log(value._dividend) - BigInteger.Log(value._divisor);
		}

		public static double Log(Rational value, double baseValue)
		{
			return BigInteger.Log(value._dividend, baseValue) - BigInteger.Log(value._divisor, baseValue);
		}

		#endregion Arithmetic functions
	}
}
