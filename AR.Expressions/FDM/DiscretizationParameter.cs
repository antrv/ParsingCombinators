using System;

namespace AR.Expressions.FDM
{
	[Serializable]
	public sealed class DiscretizationParameter
	{
		private readonly Variable _variable;
		private readonly Expression _minValue;
		private readonly Expression _maxValue;
		private readonly int _count;
		private readonly DiscretizationScheme _scheme;

		public DiscretizationParameter(Variable variable, Expression minValue, Expression maxValue, int count, DiscretizationScheme scheme)
		{
			if (variable == null)
				throw new ArgumentNullException(nameof(variable));
			if (minValue == null)
				throw new ArgumentNullException(nameof(minValue));
			if (maxValue == null)
				throw new ArgumentNullException(nameof(maxValue));
			if (count < 3)
				throw new ArgumentOutOfRangeException(nameof(count), "Count should be not less than 3");
			_variable = variable;
			_minValue = minValue;
			_maxValue = maxValue;
			_count = count;
			_scheme = scheme;
		}

		public Variable Variable
		{
			get
			{
				return _variable;
			}
		}

		public Expression MinValue
		{
			get
			{
				return _minValue;
			}
		}

		public Expression MaxValue
		{
			get
			{
				return _maxValue;
			}
		}

		public int Count
		{
			get
			{
				return _count;
			}
		}

		public DiscretizationScheme Scheme
		{
			get
			{
				return _scheme;
			}
		}
	}
}
