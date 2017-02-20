using System;
using System.Globalization;
using System.Numerics;

namespace AR.Expressions
{
	public class ExpressionParser
	{
		private readonly Storage _storage;
		private readonly Parser<char, Expression> _expressionParser;

		public ExpressionParser(Storage storage)
		{
			_storage = storage;

			var bigIntegerParser = Parse.Char(c => c >= '0' && c <= '9', "Number expected").
				AtLeastOnce().AsText().Select(s => BigInteger.Parse(s, NumberStyles.None)).Catch();

			var constantParser = bigIntegerParser.Select(s => (Expression)s).
				Or(Parse.Double.Select(d => (Expression)d)); // TODO

			var functionParser = (from name in Parse.Ident
								 from leftBracket in Parse.Char('(')
								 from parameters in Parse.Ref(() => _expressionParser).Many(Parse.Char(','))
								 from rightBracket in Parse.Char(')')
								 select new FunctionExpression(_storage.Functions[name], parameters)).Catch("Function not found");

			var variableParser = Parse.Ident.Select(n => _storage.Variables[n]).Catch("Variable not found");

			var bracketExpressionParser = from leftBracket in Parse.Char('(')
										  from expression in Parse.Ref(() => _expressionParser)
										  from rightBracket in Parse.Char(')')
										  select expression;

			var simpleExpression = Parse.Or(constantParser, functionParser, variableParser, bracketExpressionParser);

			var powerExpression = from @base in simpleExpression
								  from power in simpleExpression.RequiredIf(Parse.Char('^'))
								  select power == null ? @base : new FunctionExpression(_storage.Functions["@power"], @base, power);

			//var productExpression = from factor in powerExpression
			//						from op in (from op in Parse.Char('*').Or(Parse.Char('/')) select )
		}

		public Expression ParseExpression(string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			throw new NotImplementedException();
		}
	}
}