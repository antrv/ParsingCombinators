using System.Collections.Generic;
using System.Linq;

namespace AR.Expressions.Simplification
{
    internal struct Sum
    {
        public Rational Value;
        public IReadOnlyList<Addent> Addents;

        public Expression ToExpression()
        {
            IReadOnlyList<Addent> addents = Addents;
            if (!Value.IsZero)
                addents = addents.Add(new Addent()
                {
                    Multiplier = Value,
                    Factors = ReadOnlyList<Expression>.Empty,
                });

            if (addents.Count == 0)
                return 0;
            if (addents.Count == 1)
                return addents[0].ToExpression();

            IReadOnlyList<Expression> positive = addents.Where(a => !a.IsNegative).Select(a => a.ToExpressionWithoutSign()).ToArray();
            IReadOnlyList<Expression> negative = addents.Where(a => a.IsNegative).Select(a => a.ToExpressionWithoutSign()).ToArray();

            Expression result;
            if (positive.Count > 0)
            {
                if (positive.Count == 1)
                    result = positive[0];
                else
                    result = Operators.Add.Apply(positive);

                result = negative.Aggregate(result, (current, subtract) => current - subtract);
            }
            else
            {
                result = negative.Skip(1).Aggregate(-negative[0], (current, subtract) => current - subtract);
            }

            return result;
        }
    }
}