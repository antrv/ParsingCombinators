using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Math.Expressions.Transformations
{
	public class DifferentiationTransformation: ExpressionTransformation<Expression>
	{
		private delegate Expression DifferentiationRule(DifferentiationTransformation transformation, FunctionExpression functionExpression);
		private readonly Dictionary<Function, DifferentiationRule> _rules;
		private readonly Variable _variable;

		public DifferentiationTransformation(Variable variable)
		{
			if (variable == null)
				throw new ArgumentNullException(nameof(variable));
			_variable = variable;
            _rules = new Dictionary<Function, DifferentiationRule>();
            InitRules();
		}

		private void InitRules()
		{
			// Rules for differentiation
			_rules[Operators.Plus] = (t, e) => t.Transform(e[0]);
			_rules[Operators.Negate] = (t, e) => -t.Transform(e[0]);
			_rules[Operators.Add] = (t, e) => Operators.Add.Apply(e.Select(t.Transform));
			_rules[Operators.Subtract] = (t, e) => t.Transform(e[0]) - t.Transform(e[1]);
			_rules[Operators.Multiply] = MultiplicationDifferentiation;
			_rules[Operators.Divide] = (t, e) => t.Transform(e[0]) / e[1] - e[0] * t.Transform(e[1]) / e[1].Power(2);
			_rules[Operators.Power] = (t, e) => e[0].Power(e[1] - 1) * (t.Transform(e[0]) / e[0] * e[1] + Functions.Ln.Apply(e[0]) * t.Transform(e[1]));

			_rules[Functions.Sqrt] = (t, e) => t.Transform(e[0]) / (2 * e);
			_rules[Functions.Exp] = (t, e) => e * t.Transform(e[0]);
			_rules[Functions.Ln] = (t, e) => t.Transform(e[0]) / e[0];
			_rules[Functions.Log] = (t, e) => t.Transform(Functions.Ln.Apply(e[0]) / Functions.Ln.Apply(e[1]));

			_rules[Functions.Sin] = (t, e) => Functions.Cos.Apply(e[0]) * t.Transform(e[0]);
			_rules[Functions.Cos] = (t, e) => -Functions.Sin.Apply(e[0]) * t.Transform(e[0]);
			_rules[Functions.Tan] = (t, e) => 1 / Functions.Cos.Apply(e[0]).Power(2) * t.Transform(e[0]);
			_rules[Functions.Cot] = (t, e) => -1 / Functions.Sin.Apply(e[0]).Power(2) * t.Transform(e[0]);
			_rules[Functions.Sec] = (t, e) => Functions.Sin.Apply(e[0]) / Functions.Cos.Apply(e[0]).Power(2) * t.Transform(e[0]);
			_rules[Functions.Cosec] = (t, e) => -Functions.Cos.Apply(e[0]) / Functions.Sin.Apply(e[0]).Power(2) * t.Transform(e[0]);

			_rules[Functions.Arcsin] = (t, e) => 1 / Functions.Sqrt.Apply(1 - e[0].Power(2));
			_rules[Functions.Arccos] = (t, e) => -1 / Functions.Sqrt.Apply(1 - e[0].Power(2));
			_rules[Functions.Arctan] = (t, e) => 1 / (1 + e[0].Power(2));
			_rules[Functions.Arccot] = (t, e) => -1 / (1 + e[0].Power(2));

			_rules[Functions.Sinh] = (t, e) => Functions.Cosh.Apply(e[0]) * t.Transform(e[0]);
			_rules[Functions.Cosh] = (t, e) => Functions.Sinh.Apply(e[0]) * t.Transform(e[0]);
			_rules[Functions.Tanh] = (t, e) => 1 / Functions.Cosh.Apply(e[0]).Power(2) * t.Transform(e[0]);
			_rules[Functions.Coth] = (t, e) => -1 / Functions.Sinh.Apply(e[0]).Power(2) * t.Transform(e[0]);

			_rules[Functions.Arcsinh] = (t, e) => 1 / Functions.Sqrt.Apply(e[0].Power(2) + 1);
			_rules[Functions.Arccosh] = (t, e) => 1 / Functions.Sqrt.Apply(e[0].Power(2) - 1);
			_rules[Functions.Arctanh] = (t, e) => 1 / (1 - e[0].Power(2));
			_rules[Functions.Arccoth] = (t, e) => 1 / (1 - e[0].Power(2));
		}

		private static Expression MultiplicationDifferentiation(DifferentiationTransformation t, FunctionExpression e)
		{
			var count = e.Count;
			Expression[] addents = new Expression[count];
			for (int i = 0; i < count; i++)
			{
				Expression[] factors = new Expression[count];
				for (int j = 0; j < count; j++)
					factors[j] = e[j];
				factors[i] = t.Transform(e[i]);
				addents[i] = Operators.Multiply.Apply(factors);
			}
			return Operators.Add.Apply(addents);
		}

	    protected override Expression TransformConstant(Constant constant)
	    {
	        return 0;
	    }

	    protected override Expression TransformNumber(Number number)
		{
			return 0;
		}

		protected override Expression TransformVariable(Variable variable)
		{
			if (variable == _variable)
				return 1;
			return 0;
		}

		protected override Expression TransformFunction(FunctionExpression functionExpression)
		{
			DifferentiationRule rule;
			if (_rules.TryGetValue(functionExpression.Function, out rule))
				return rule(this, functionExpression);

			UserFunction userFunction = functionExpression.Function as UserFunction;
			if (userFunction != null)
			{
				SubstituteTransformation transformation = new SubstituteTransformation(userFunction.Arguments, functionExpression);
				return Transform(transformation.Transform(userFunction.Expression));
			}

			return functionExpression.Differentiate(_variable);
		}
	}
}