using System;

namespace AR.Expressions.Transformations
{
    [Serializable]
    public abstract class ExpressionVisitor: IExpressionVisitor
    {
        public virtual void Visit(Expression expression)
        {
			Number number = expression as Number;
            if (number != null)
            {
                VisitNumber(number);
                return;
            }

			Constant constant = expression as Constant;
            if (constant != null)
            {
                VisitConstant(constant);
                return;
            }

			Variable variableExpression = expression as Variable;
            if (variableExpression != null)
            {
                VisitVariable(variableExpression);
                return;
            }

			FunctionExpression functionExpression = expression as FunctionExpression;
            if (functionExpression != null)
            {
                VisitFunction(functionExpression);
                return;
            }

			throw new ArgumentException("Invalid expression");
        }

        public virtual void VisitNumber(Number number)
        {
        }

        public virtual void VisitConstant(Constant constant)
        {
        }

        public virtual void VisitVariable(Variable variable)
        {
        }

        public virtual void VisitFunction(FunctionExpression functionExpression)
        {
            foreach (Expression argument in functionExpression)
                Visit(argument);
        }
    }
}