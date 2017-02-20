using System.Collections.Generic;
using System.Linq;
using AR.Expressions.Transformations;

namespace AR.Expressions.Simplification
{
    internal sealed class ProductAggregator: ExpressionVisitor
    {
        private bool _sign;
        private Rational _value;
        private IReadOnlyList<Expression> _factors;
        private Expression _divider;

        public ProductAggregator()
        {
            _value = Rational.One;
            _factors = ReadOnlyList<Expression>.Empty;
            _divider = 1;
        }

        public override void VisitNumber(Number number)
        {
            _value *= number.Value;
        }

        public override void VisitConstant(Constant constant)
        {
            _factors = _factors.Add(constant);
        }

        public override void VisitVariable(Variable variable)
        {
            _factors = _factors.Add(variable);
        }

        public override void VisitFunction(FunctionExpression functionExpression)
        {
            if (functionExpression.Function == Operators.Plus)
                Visit(functionExpression[0].Simplify());
            else if (functionExpression.Function == Operators.Negate)
            {
                _sign = !_sign;
                Visit(functionExpression[0].Simplify());
            }
            else if (functionExpression.Function == Operators.Multiply)
            {
                foreach (Expression factor in functionExpression)
                    Visit(factor.Simplify());
            }
            else if (functionExpression.Function == Operators.Divide)
            {
                Visit(functionExpression[0].Simplify());
                ProductAggregator aggregator = new ProductAggregator();
                aggregator.Visit(functionExpression[1].Simplify());

                _sign ^= aggregator._sign;
                _value /= aggregator._value;
                Visit(aggregator._divider);
                foreach (Expression factor in aggregator._factors)
                    _divider *= factor;
            }
            else
            {
                _factors = _factors.Add(functionExpression.Function.
                    Apply(functionExpression.Select(arg => arg.Simplify())));
            }
        }

        private static IReadOnlyList<Addent> ReplaceFactor(IReadOnlyList<Addent> addents, int factorIndex, IEnumerable<Expression> expressions)
        {
            return expressions.SelectMany(arg => addents.Select(a =>
            {
                a.Factors = a.Factors.SetItem(factorIndex, arg);
                return a;
            })).ToArray();
        }

        public Expression ToExpression()
        {
            Addent addent = new Addent()
            {
                IsNegative = _sign,
                Multiplier = _value,
                Factors = _factors,
                Divider = _divider.Simplify()
            };

            int factorsCount = _factors.Count;
            IReadOnlyList<Addent> addents = new ReadOnlyList<Addent>(addent);
            for (int i = 0; i < factorsCount; i++)
            {
                Expression factor = _factors[i];
                FunctionExpression functionExpression = factor as FunctionExpression;
                if (functionExpression != null)
                {
                    if (functionExpression.Function == Operators.Add)
                    {
                        addents = ReplaceFactor(addents, i, functionExpression);
                    }
                    else if (functionExpression.Function == Operators.Subtract)
                    {
                        IReadOnlyList<Expression> arguments = new ReadOnlyList<Expression>(functionExpression[0], -functionExpression[1]);
                        addents = ReplaceFactor(addents, i, arguments);
                    }
                }
            }

            if (addents.Count == 1)
                return addents[0].ToExpression();

            return Operators.Add.Apply(addents.Select(a => a.ToExpression()));
        }
    }
}