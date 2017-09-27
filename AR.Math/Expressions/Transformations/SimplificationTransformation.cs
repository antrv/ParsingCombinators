using System;
using System.Collections.Generic;
using System.Linq;
using AR.Math.Expressions.Simplification;

namespace AR.Math.Expressions.Transformations
{
    public sealed class SimplificationTransformation: ExpressionTransformation<Expression>
    {
        protected override Expression TransformNumber(Number number)
        {
            return number;
        }

        protected override Expression TransformConstant(Constant constant)
        {
            return constant;
        }

        protected override Expression TransformVariable(Variable variable)
        {
            return variable;
        }

        protected override Expression TransformFunction(FunctionExpression functionExpression)
        {
            if (functionExpression.Function == Operators.Diff)
            {
                VariableListExtractor extractor = new VariableListExtractor();
                extractor.Visit(functionExpression[1]);
                IReadOnlyList<Variable> variables = extractor.Variables;
                if (variables.Count == 0)
                    throw new InvalidOperationException("Cannot calculate derivative by constant");

                throw new NotImplementedException();
            }
            if (functionExpression.Function == Operators.Plus)
                return Transform(functionExpression[0]);
            if (functionExpression.Function is Operator)
            {
                SumAggregator aggregator = new SumAggregator();
                aggregator.VisitFunction(functionExpression);
                return aggregator.ToExpression();
            }
            return functionExpression.Function.Apply(functionExpression.Select(Transform));
        }
    }
}