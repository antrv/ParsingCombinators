using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AR.Math.Expressions
{
	[Serializable]
	public sealed class FunctionExpression: Expression, IReadOnlyList<Expression>
	{
		private readonly Function _function;
		private readonly IReadOnlyList<Expression> _arguments;

		public FunctionExpression(Function function, params Expression[] arguments)
		{
			if (function == null)
				throw new ArgumentNullException(nameof(function));
			function.ValidateArguments(arguments);

			_function = function;
			_arguments = arguments;
		}

		public FunctionExpression(Function function, IReadOnlyList<Expression> arguments)
		{
			if (function == null)
				throw new ArgumentNullException(nameof(function));
			function.ValidateArguments(arguments);

			_function = function;
			_arguments = arguments;
		}

		public Function Function => _function;
	    public Expression this[int index] => _arguments[index];

	    private static void AppendArgument(StringBuilder stringBuilder, Expression expression, int priority)
	    {
	        Operator op = (expression as FunctionExpression)?.Function as Operator;
	        bool needBrackets = op != null && op.Priority > priority;
	        if (needBrackets)
	            stringBuilder.Append("(");
	        expression.ToString(stringBuilder);
	        if (needBrackets)
	            stringBuilder.Append(")");
	    }

	    private void AppendArguments(StringBuilder stringBuilder, string separator, int priority)
	    {
            AppendArgument(stringBuilder, _arguments[0], priority);
	        for (int i = 1, count = _arguments.Count; i < count; i++)
	        {
	            stringBuilder.Append(separator);
	            AppendArgument(stringBuilder, _arguments[i], priority);
	        }
	    }

	    public override void ToString(StringBuilder stringBuilder)
	    {
	        Operator op = _function as Operator;
	        if (op != null)
	        {
	            switch (op.Type)
	            {
	                case OperatorType.PrefixUnary:
	                    stringBuilder.Append(op.Symbol ?? op.Name);
	                    AppendArguments(stringBuilder, string.Empty, op.Priority);
	                    break;
	                case OperatorType.PostfixUnary:
	                    AppendArguments(stringBuilder, string.Empty, op.Priority);
	                    stringBuilder.Append(op.Symbol ?? op.Name);
	                    break;
	                case OperatorType.Binary:
	                    AppendArguments(stringBuilder, " " + (op.Symbol ?? op.Name) + " ", op.Priority);
	                    break;
                    default:
                        throw new InvalidOperationException("Invalid or unknown operator type");
	            }
	            return;
	        }

			stringBuilder.Append(_function.Name);
			stringBuilder.Append("(");
	        AppendArguments(stringBuilder, ", ", int.MaxValue);
			stringBuilder.Append(")");
		}

        public IEnumerator<Expression> GetEnumerator() => _arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _arguments.GetEnumerator();
        public int Count => _arguments.Count;
	}
}