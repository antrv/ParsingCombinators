using System.Collections.Generic;
using System.Linq;
using AR.Math.Expressions.Transformations;

namespace AR.Math.Expressions.Simplification
{
    internal sealed class SumAggregator: ExpressionVisitor
    {
        private Rational _value;
        private IReadOnlyList<Addent> _addents;

        public SumAggregator()
        {
            _value = Rational.Zero;
            _addents = ReadOnlyList<Addent>.Empty;
        }

        private void Append(SumAggregator aggregator, bool invert)
        {
            if (invert)
                _value -= aggregator._value;
            else 
                _value += aggregator._value;
            IEnumerable<Addent> addents = aggregator._addents;
            if (invert)
                addents = addents.Select(a => a.Negate());
            _addents = _addents.AddRange(addents);
        }

        private void AddExpression(Expression expression)
        {
            _addents = _addents.Add(new Addent()
            {
                Multiplier = Rational.One,
                Factors = new ReadOnlyList<Expression>(expression)
            });
        }

        public override void VisitNumber(Number number)
        {
            _value += number.Value;
        }

        public override void VisitConstant(Constant constant)
        {
            AddExpression(constant);
        }

        public override void VisitVariable(Variable variable)
        {
            AddExpression(variable);
        }

        public override void VisitFunction(FunctionExpression functionExpression)
        {
            if (functionExpression.Function == Operators.Plus)
                Visit(functionExpression[0]);
            else if (functionExpression.Function == Operators.Negate)
            {
                SumAggregator aggregator = new SumAggregator();
                aggregator.Visit(functionExpression[0]);
                Append(aggregator, true);
            }
            else if (functionExpression.Function == Operators.Add)
            {
                foreach (Expression argument in functionExpression)
                    Visit(argument);
            }
            else if (functionExpression.Function == Operators.Subtract)
            {
                Visit(functionExpression[0]);
                SumAggregator aggregator = new SumAggregator();
                aggregator.Visit(functionExpression[1]);
                Append(aggregator, true);
            }
            else if (functionExpression.Function == Operators.Multiply || functionExpression.Function == Operators.Divide)
            {
                ProductAggregator aggregator = new ProductAggregator();
                aggregator.Visit(functionExpression);

                Expression expression = aggregator.ToExpression();

                FunctionExpression functionExpression1 = expression as FunctionExpression;
                if (functionExpression1 != null && (functionExpression1.Function == Operators.Multiply || functionExpression1.Function == Operators.Divide))
                    AddExpression(expression);
                else
                    Visit(expression);
            }
            else
                AddExpression(functionExpression);
        }

        public Expression ToExpression()
        {
            Sum sum = new Sum()
            {
                Value = _value,
                Addents = _addents
            };

            return sum.ToExpression();
        }
    }
}