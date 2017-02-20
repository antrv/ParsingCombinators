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
            const string expression = "a + b * 5 / 2 - a * b * 5/7";

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

                var variables = new ReadOnlyList<Variable>(a, b);
                var values = new ReadOnlyList<Expression>(
                    new Rational(252, 10), // Rational is used because double represents 25.2 not accerately
                    new Rational(111, 10) // Rational is used because double is not accurately represents 11.1
                    );

                for (int i = 0; i < variables.Count; i++)
                {
                    Console.WriteLine(variables[i].Name + " = " + values[i]);
                }

                SubstituteTransformation substituteTransformation = new SubstituteTransformation(variables, values);
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