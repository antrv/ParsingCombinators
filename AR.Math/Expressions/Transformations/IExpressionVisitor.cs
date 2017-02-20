namespace AR.Math.Expressions.Transformations
{
    public interface IExpressionVisitor
    {
        void Visit(Expression expression);
        void VisitNumber(Number number);
        void VisitConstant(Constant constant);
        void VisitVariable(Variable variable);
        void VisitFunction(FunctionExpression functionExpression);
    }
}