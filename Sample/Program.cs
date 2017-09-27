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
            StringInput parserInput = expression;

            IParsingResult<char, Expression> result = parser(parserInput);
            if (result.Success)
            {
                Expression expr = result.Value;
                Console.WriteLine(expr);

                Console.WriteLine("Perform simplification");
                expr = expr.Simplify();
                Console.WriteLine(expr);

                Console.WriteLine("Perform substitution");

                var variableValuePairs = new ReadOnlyList<VariableValuePair>(
                    new VariableValuePair(a, new Rational(252, 10)), // Rational is used because double represents 25.2 not accerately
                    new VariableValuePair(b, new Rational(111, 10))); 

                foreach (var pair in variableValuePairs)
                {
                    Console.WriteLine(pair.Variable.Name + " = " + pair.Value);
                }

                SubstituteTransformation substituteTransformation = new SubstituteTransformation(variableValuePairs);
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