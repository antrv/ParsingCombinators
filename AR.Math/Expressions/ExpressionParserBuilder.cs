using AR.Math.Parsing;

namespace AR.Math.Expressions
{
    public class ExpressionParserBuilder
    {
        private readonly Parser<char, Expression> _expressionParser;

        public ExpressionParserBuilder(Storage storage)
        {
            var constantParser =
                Parse.Double.Select(s => (Expression)s).
                    Or(Parse.BigInt.Select(d => (Expression)d)).
                    Trim();

            var functionParser =
               (from name in Parse.Ident
                from leftBracket in Parse.Char('(').Trim()
                from parameters in Parse.Ref(() => _expressionParser).Many(Parse.Char(',').Trim())
                from rightBracket in Parse.Char(')').Trim()
                select new FunctionExpression(storage.Functions[name], parameters)).
                Catch("Function not found");

            var variableParser = Parse.Ident.
                Trim().
                Select(n => storage.Variables[n]).
                Catch("Variable not found");

            var bracketExpressionParser =
                from leftBracket in Parse.Char('(').Trim()
                from expression in Parse.Ref(() => _expressionParser)
                from rightBracket in Parse.Char(')').Trim()
                select expression;

            var simpleExpression =
                constantParser.Or(functionParser).Or(variableParser).Or(bracketExpressionParser);

            var unarySignExpression =
                    from @operator in Parse.Char('+').Or(Parse.Char('-')).Optional('+')
                    from expression in simpleExpression
                    select @operator == '+' ? expression : new FunctionExpression(Operators.Negate, expression);
                // TODO: multiple signs chars, i.e. -+-6

            var powerExpression =
                from @base in unarySignExpression
                from power in unarySignExpression.RequiredIf(Parse.Char('^').Trim())
                select power == null ? @base : new FunctionExpression(Operators.Power, @base, power);

            var productOperator =
                Parse.Char('*').Trim().Select(c => Operators.Multiply).Or(
                    Parse.Char('/').Trim().Select(c => Operators.Divide));

            var productExpression =
                from product in powerExpression.AtLeastOnce(productOperator,
                    (left, @operator, right) => new FunctionExpression(@operator, left, right))
                select product;

            var sumOperator =
                Parse.Char('+').Trim().Select(c => Operators.Add).Or(
                    Parse.Char('-').Trim().Select(c => Operators.Subtract));

            var sumExpression =
                from product in productExpression.AtLeastOnce(sumOperator,
                    (left, @operator, right) => new FunctionExpression(@operator, left, right))
                select product;

            _expressionParser = sumExpression;
        }

        public Parser<char, Expression> Parser => _expressionParser;
    }
}