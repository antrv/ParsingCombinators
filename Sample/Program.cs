using System;
using AR.Math;
using AR.Math.Expressions;
using AR.Math.Expressions.Transformations;
using AR.Math.Parsing;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string expression = "a + b * 2.5 - a * b";

            Variable a = new Variable("a");
            Variable b = new Variable("b");
            Storage storage = new Storage();
            storage.Variables.Add(a);
            storage.Variables.Add(b);

            ExpressionParserBuilder parserBuilder = new ExpressionParserBuilder(storage);
            Parser<char, Expression> parser = parserBuilder.Parser.Eof();

            IParsingResult<char, Expression> result = parser(new StringInput(expression));
            if (result.Success)
            {
                Expression expr = result.Value;
                Console.WriteLine(expr);

                Console.WriteLine("Perform simplification");
                expr = expr.Simplify();
                Console.WriteLine(expr);

                Console.WriteLine("Perform substitution");

                SubstituteTransformation substituteTransformation = new SubstituteTransformation(
                    new ReadOnlyList<Variable>(a, b),
                    new ReadOnlyList<Expression>(25.2, 11.1));
                expr = substituteTransformation.Transform(expr);
                Console.WriteLine(expr);

                Console.WriteLine("Perform simplification");
                expr = expr.Simplify();
                Console.WriteLine(expr);
            }
            else
            {
                Console.WriteLine("Error: " + result.Error.Message + " at " + result.Error.Position);
            }

            Console.ReadLine();
        }
    }
}