using System;

namespace AR.Math.Expressions
{
    [Serializable]
    public struct VariableValuePair
    {
        private readonly Variable _variable;
        private readonly Expression _value;

        public VariableValuePair(Variable variable, Expression value)
        {
            _variable = variable ?? throw new ArgumentNullException(nameof(variable));
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public Variable Variable => _variable;
        public Expression Value => _value;
    }
}