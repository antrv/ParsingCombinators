using System;

namespace AR.Math.Expressions
{
    public static class Operators
	{
		public static readonly Operator Plus = new PlusOperator();
		public static readonly Operator Negate = new NegateOperator();
		public static readonly Operator Add = new AddOperator();
		public static readonly Operator Subtract = new SubtractOperator();
		public static readonly Operator Multiply = new MultiplyOperator();
		public static readonly Operator Divide = new DivideOperator();
		public static readonly Operator Power = new PowerOperator();

		public static readonly Function Diff = new PredefinedFunction("@diff", 2, args => { throw new NotSupportedException(); });
	}
}