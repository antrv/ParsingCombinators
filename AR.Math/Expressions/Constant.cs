using System;
using System.Text;

namespace AR.Math.Expressions
{
    [Serializable]
    public sealed class Constant: Expression
    {
        private readonly string _name;

        public Constant(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (name.Length == 0)
                throw new ArgumentException("Constant name cannot be empty", nameof(name));
            _name = name;
        }

        public string Name => _name;

        public override void ToString(StringBuilder stringBuilder)
        {
            stringBuilder.Append(_name);
        }
    }
}