using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Math.Expressions
{
	[Serializable]
	internal sealed class DivideOperator: Operator
	{
		internal DivideOperator()
			: base("@divide")
		{
		}

		public override double Evaluate(params double[] arguments)
		{
			if (arguments == null || arguments.Length != 2)
				throw new ArgumentException("Invalid arguments.", nameof(arguments));

			return arguments[0] / arguments[1];
		}

		public override string Symbol => "/";
	    public override OperatorType Type => OperatorType.Binary;
	    public override int Priority => OperatorPriorities.DividePriority;

	    public override void ValidateArguments(IReadOnlyList<Expression> arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Count != 2)
				throw new ArgumentException("Invalid argument count.", nameof(arguments));
			if (arguments.Any(x => x == null))
				throw new ArgumentException("Argument list cannot contain nulls.", nameof(arguments));
		}
	}
}