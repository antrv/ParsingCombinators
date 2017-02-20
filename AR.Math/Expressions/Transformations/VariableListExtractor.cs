using System.Collections.Generic;
using System.Linq;

namespace AR.Math.Expressions.Transformations
{
    public sealed class VariableListExtractor: ExpressionVisitor
    {
        private readonly HashSet<Variable> _variables;

        public VariableListExtractor()
        {
            _variables = new HashSet<Variable>();
        }

        public IReadOnlyList<Variable> Variables => new ReadOnlyList<Variable>(_variables.ToArray());

        public override void VisitVariable(Variable variable)
        {
            _variables.Add(variable);
        }
    }
}