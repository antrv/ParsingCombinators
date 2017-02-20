using System;

namespace AR.Math.Expressions
{
	[Serializable]
	public class Storage
	{
		private readonly VariableList _variables = new VariableList();
		private readonly FunctionList _functions = new FunctionList();

		public Storage()
		{
			//_constants.Add(Constants.Pi);
			//_constants.Add(Constants.E);

			_functions.Add(Operators.Plus);
			_functions.Add(Operators.Negate);
			_functions.Add(Operators.Add);
			_functions.Add(Operators.Subtract);
			_functions.Add(Operators.Multiply);
			_functions.Add(Operators.Divide);
			_functions.Add(Operators.Power);

			_functions.Add(Expressions.Functions.Sqrt);
		}

		public VariableList Variables => _variables;

	    public FunctionList Functions => _functions;
	}
}