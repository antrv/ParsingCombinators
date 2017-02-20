using System;
using System.Collections.Generic;

namespace AR.Expressions
{
	[Serializable]
	internal sealed class NegateOperator: Operator
	{
		internal NegateOperator()
			: base("@minus")
		{
		}

		public override double Evaluate(params double[] arguments)
		{
			if (arguments == null || arguments.Length != 1)
				throw new ArgumentException("Invalid arguments.", nameof(arguments));

			return -arguments[0];
		}

		public override string Symbol => "-";
	    public override OperatorType Type => OperatorType.PrefixUnary;
	    public override int Priority => OperatorPriorities.NegatePriority;

	    public override void ValidateArguments(IReadOnlyList<Expression> arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Count != 1)
				throw new ArgumentException("Invalid argument count.", nameof(arguments));
			if (arguments.Any(x => x == null))
				throw new ArgumentException("Argument list cannot contain nulls.", nameof(arguments));
		}
	}
}