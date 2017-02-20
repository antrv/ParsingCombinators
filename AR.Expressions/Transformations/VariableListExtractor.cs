using System.Collections.Generic;

namespace AR.Expressions.Transformations
{
    public sealed class VariableListExtractor: ExpressionVisitor
    {
        private readonly HashSet<Variable> _variables;

        public VariableListExtractor()
        {
            _variables = new HashSet<Variable>();
        }

        public IReadOnlyList<Variable> Variables => _variables.ToImmutableList();

        public override void VisitVariable(Variable variable)
        {
            _variables.Add(variable);
        }
    }
}