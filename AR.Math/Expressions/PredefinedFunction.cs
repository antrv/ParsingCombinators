using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Math.Expressions
{
	[Serializable]
	public class PredefinedFunction: Function
	{
		private readonly int _argumentCount;
		private readonly Func<double[], double> _calculateFunction;

		public PredefinedFunction(string name, Func<double, double> calculateFunction)
			: base(name)
		{
			if (calculateFunction == null)
				throw new ArgumentNullException(nameof(calculateFunction));
			_calculateFunction = args => calculateFunction(args[0]);
			_argumentCount = 1;
		}

		public PredefinedFunction(string name, Func<double, double, double> calculateFunction)
			: base(name)
		{
			if (calculateFunction == null)
				throw new ArgumentNullException(nameof(calculateFunction));
			_calculateFunction = args => calculateFunction(args[0], args[1]);
			_argumentCount = 2;
		}

		public PredefinedFunction(string name, int argumentCount, Func<double[], double> calculateFunction)
			: base(name)
		{
			if (calculateFunction == null)
				throw new ArgumentNullException(nameof(calculateFunction));
			if (argumentCount < 1)
				throw new ArgumentOutOfRangeException(nameof(argumentCount));
			_calculateFunction = calculateFunction;
			_argumentCount = argumentCount;
		}

		public override void ValidateArguments(IReadOnlyList<Expression> arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Count != _argumentCount)
				throw new ArgumentException("Invalid argument count.", nameof(arguments));
			if (arguments.Any(x => x == null))
				throw new ArgumentException("Argument list cannot contain nulls.", nameof(arguments));
		}

		public override double Evaluate(params double[] arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Length != _argumentCount)
				throw new ArgumentException("Invalid argument count.", nameof(arguments));
			return _calculateFunction(arguments);
		}

		public override string ToString()
		{
			return Name + "(" + string.Join(", ", Enumerable.Range(1, _argumentCount).Select(i => "a" + i)) + ")";
		}
	}
}