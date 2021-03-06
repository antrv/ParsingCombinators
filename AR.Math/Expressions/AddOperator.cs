﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Math.Expressions
{
	[Serializable]
	internal sealed class AddOperator: Operator
	{
		internal AddOperator()
			: base("@add")
		{
		}

		public override double Evaluate(params double[] arguments)
		{
			if (arguments == null || arguments.Length < 2)
				throw new ArgumentException("Invalid arguments.", nameof(arguments));

			double result = arguments[0];
			for (int i = 1; i < arguments.Length; i++)
				result += arguments[i];
			return result;
		}

		public override string Symbol => "+";
	    public override OperatorType Type => OperatorType.Binary;
	    public override int Priority => OperatorPriorities.AddPriority;

	    public override void ValidateArguments(IReadOnlyList<Expression> arguments)
		{
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			if (arguments.Count < 2)
				throw new ArgumentException("Invalid argument count.", nameof(arguments));
			if (arguments.Any(x => x == null))
				throw new ArgumentException("Argument list cannot contain nulls.", nameof(arguments));
		}
	}
}