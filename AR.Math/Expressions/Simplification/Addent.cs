using System.Collections.Generic;
using System.Linq;

namespace AR.Math.Expressions.Simplification
{
    internal struct Addent
    {
        public bool IsNegative; // false: +, true: -
        public Rational Multiplier;
        public IReadOnlyList<Expression> Factors;
        public Expression Divider;

        public Addent Negate()
        {
            IsNegative = !IsNegative;
            return this;
        }

        public bool IsZero
        {
            get
            {
                return Multiplier.IsZero || Factors.Any(f => (f as Number)?.Value.IsZero ?? false);
            }
        }

        public Expression ToExpressionWithoutSign()
        {
            if (IsZero)
                return 0;

            IReadOnlyList<Expression> factors = Factors;
            if (!Multiplier.IsOne)
                factors = factors.Concat(new Expression[] {Multiplier}).ToArray();

            Expression result;
            if (factors.Count == 0)
                result = 1;
            else if (factors.Count == 1)
                result = factors[0];
            else
                result = Operators.Multiply.Apply(factors);

            bool hasDivider = Divider != null && !((Divider as Number)?.Value.IsOne ?? false);
            if (hasDivider)
                result /= Divider;

            return result;
        }

        public Expression ToExpression()
        {
            Expression factorsExpression = ToExpressionWithoutSign();

            if (IsNegative) // -
                return -factorsExpression;
            return factorsExpression;
        }
    }
}