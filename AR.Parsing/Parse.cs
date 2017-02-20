using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AR.Parsing
{
	public static class Parse
	{
		public static Parser<char, char> Char(char c)
		{
			return input =>
			{
				if (!input.Eof && input.Current == c)
					return new SuccessParsingResult<char, char>(c, input.Next);

				return new FailParsingResult<char, char>(input, new ParsingError()
				{
					Position = input.Position,
					Message = c + " expected",
					Method = "Parse.Char(char)",
					Type = typeof(char)
				});
			};
		}

		public static Parser<char, char> Char(char c, bool ignoreCase, CultureInfo culture = null)
		{
			if (!ignoreCase)
				return Char(c);

			if (culture == null)
				culture = CultureInfo.CurrentCulture;
			char lowChar = char.ToLower(c, culture);
			char highChar = char.ToUpper(c, culture);
			if (c == lowChar && c == highChar)
				return Char(c);

			return input =>
			{
				if (!input.Eof)
				{
					char current = input.Current;
					if (current == c || current == lowChar || current == highChar)
						return new SuccessParsingResult<char, char>(current, input.Next);
				}
				return new FailParsingResult<char, char>(input, new ParsingError()
				{
					Position = input.Position,
					Message = c + " expected",
					Method = "Parse.Char(char, bool, CultureInfo)",
					Type = typeof(char)
				});
			};
		}

		public static Parser<char, string> String(string str)
		{
			if (str == null)
				throw new ArgumentNullException(nameof(str));
			if (string.IsNullOrEmpty(str))
				throw new ArgumentException("String cannot be empty", nameof(str));
			return input =>
			{
				IParserInput<char> next = input;
				for (int i = 0, count = str.Length; i < count; i++)
				{
					if (next.Eof || next.Current != str[i])
						return new FailParsingResult<char, string>(input, new ParsingError()
						{
							Position = input.Position,
							Message = str + " expected",
							Method = "Parse.String(string)",
							Type = typeof(string)
						});
					next = next.Next;
				}
				return new SuccessParsingResult<char, string>(str, next);
			};
		}

		public static Parser<char, string> String(string str, bool ignoreCase,
			CultureInfo culture = null)
		{
			if (!ignoreCase)
				return String(str);

			if (str == null)
				throw new ArgumentNullException(nameof(str));
			if (string.IsNullOrEmpty(str))
				throw new ArgumentException("String cannot be empty", nameof(str));

			if (culture == null)
				culture = CultureInfo.CurrentCulture;

			return input =>
			{
				IParserInput<char> next = input;
				int count = str.Length;
				char[] data = new char[count];
				for (int i = 0; i < count; i++)
				{
					char stri = str[i];
					if (!next.Eof)
					{
						char current = next.Current;
						if (current == stri || current == char.ToLower(stri, culture) || current == char.ToUpper(stri, culture))
						{
							data[i] = current;
							next = next.Next;
							continue;
						}
					}
					return new FailParsingResult<char, string>(input, new ParsingError()
					{
						Position = input.Position,
						Message = str + " expected",
						Method = "Parse.String(string, bool, CultureInfo)",
						Type = typeof(string)
					});
				}
				return new SuccessParsingResult<char, string>(new string(data), next);
			};
		}

		public static Parser<char, char> Char(Predicate<char> predicate, 
			string errorMessage)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));
			if (errorMessage == null)
				throw new ArgumentNullException(nameof(errorMessage));
			return input =>
			{
				if (input.Eof || !predicate(input.Current))
					return new FailParsingResult<char, char>(input, new ParsingError()
					{
						Position = input.Position,
						Message = errorMessage,
						Method = "Parse.Char(Predicate<char>, string)",
						Type = typeof(char)
					});
				return new SuccessParsingResult<char, char>(input.Current, input.Next);
			};
		}

		public static Parser<TItem, TItem> Item<TItem>(Predicate<TItem> predicate, 
			string errorMessage)
		{
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));
			if (errorMessage == null)
				throw new ArgumentNullException(nameof(errorMessage));
			return input =>
			{
				if (input.Eof || !predicate(input.Current))
					return new FailParsingResult<TItem, TItem>(input, new ParsingError()
					{
						Position = input.Position,
						Message = errorMessage,
						Method = "Parse.Item<TItem>(Predicate<TItem>, string)",
						Type = typeof(TItem)
					});
				return new SuccessParsingResult<TItem, TItem>(input.Current, input.Next);
			};
		}

		public static Parser<TInput, TResult> Error<TInput, TResult>(string errorMessage)
		{
			if (errorMessage == null)
				throw new ArgumentNullException(nameof(errorMessage));
			return input => new FailParsingResult<TInput, TResult>(input, new ParsingError()
			{
				Position = input.Position,
				Message = errorMessage,
				Method = "Parse.Error<TInput, TResult>(string)",
				Type = typeof(TResult)
			});
		}

		public static Parser<TInput, TResult> ErrorMessage<TInput, TResult>(
			this Parser<TInput, TResult> parser, string errorMessage)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (errorMessage == null)
				throw new ArgumentNullException(nameof(errorMessage));
			return input => parser(input).ErrorMessage(errorMessage);
		}

		public static Parser<TInput, TResult> Value<TInput, TResult>(TResult value)
		{
			return input => new SuccessParsingResult<TInput, TResult>(value, input);
		}

		public static Parser<TInput, T> Value<TInput, T>(
			this Parser<TInput, T> parser, T value)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input => parser(input).Value<TInput, T>(value);
		}

		public static Parser<TInput, TResult> Value<TInput, T, TResult>(
			this Parser<TInput, T> parser, TResult value)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input => parser(input).Value(value);
		}

		public static Parser<TInput, T> Catch<TInput, T>(this Parser<TInput, T> parser, 
			string errorMessage = null)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				try
				{
					return parser(input);
				}
				catch (Exception exception)
				{
					return new FailParsingResult<TInput, T>(input, new ParsingError()
					{
						Position = input.Position,
						Message = errorMessage ?? exception.Message,
						Exception = exception,
						Method = "Parser.Catch<TInput, T>(Parser<TInput, T>, string)",
						Type = typeof(T)
					});
				}
			};
		}

		public static Parser<TInput, T> Catch<TInput, T, TException>(			
			this Parser<TInput, T> parser, string errorMessage = null)
			where TException: Exception
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				try
				{
					return parser(input);
				}
				catch (TException exception)
				{
					return new FailParsingResult<TInput, T>(input, new ParsingError()
					{
						Position = input.Position,
						Message = errorMessage ?? exception.Message,
						Exception = exception,
						Method = "Parser.Catch<TInput, T>(Parser<TInput, T>, string)",
						Type = typeof(T)
					});
				}
			};
		}

		public static Parser<TInput, T> Or<TInput, T>(this Parser<TInput, T> parser, 
			Parser<TInput, T> other)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (other == null)
				throw new ArgumentNullException(nameof(other));
			return input =>
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
					return result;

				// loop for other parsers
				IParsingResult<TInput, T> otherResult = other(input);
				if (otherResult.Success)
					return otherResult;
				if (otherResult.Error.Position >= result.Error.Position)
					result = otherResult;
				// end of loop

				return result;
			};
		}

		public static Parser<TInput, T> Or<TInput, T>(this Parser<TInput, T> parser, 
			Parser<TInput, T> parser1, Parser<TInput, T> parser2)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (parser1 == null)
				throw new ArgumentNullException(nameof(parser1));
			if (parser2 == null)
				throw new ArgumentNullException(nameof(parser2));
			return input =>
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
					return result;

				// loop for other parsers
				IParsingResult<TInput, T> otherResult = parser1(input);
				if (otherResult.Success)
					return otherResult;
				if (otherResult.Error.Position >= result.Error.Position)
					result = otherResult;

				otherResult = parser2(input);
				if (otherResult.Success)
					return otherResult;
				if (otherResult.Error.Position >= result.Error.Position)
					result = otherResult;
				// end of loop

				return result;
			};
		}

		public static Parser<TInput, T> Or<TInput, T>(this Parser<TInput, T> parser, 
			params Parser<TInput, T>[] others)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (others == null)
				throw new ArgumentNullException(nameof(others));
			if (others.Length == 0)
				return parser;
			if (others.Any(item => item == null))
				throw new ArgumentException("Parser should not be null", nameof(others));

			int count = others.Length;
			Parser<TInput, T>[] parsers = new Parser<TInput, T>[1 + count];
			parsers[0] = parser;
			Array.Copy(others, 0, parsers, 1, count);

			return input =>
			{
				IParsingResult<TInput, T> result = parsers[0](input);
				if (result.Success)
					return result;

				// loop for other parsers
				for (int i = 1; i < parsers.Length; i++)
				{
					IParsingResult<TInput, T> otherResult = parsers[i](input);
					if (otherResult.Success)
						return otherResult;
					if (otherResult.Error.Position >= result.Error.Position)
						result = otherResult;
				}
				// end of loop

				return result;
			};
		}

		public static Parser<TInput, TResult> Select<TInput, TResult, T>(
			this Parser<TInput, T> parser, Func<T, TResult> func)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (func == null)
				throw new ArgumentNullException(nameof(func));
			return input => parser(input).Select(func);
		}

		public static Parser<TInput, TResult> SelectMany<TInput, T, TOther, TResult>(
			this Parser<TInput, T> parser, Func<T, Parser<TInput, TOther>> selectFunc, 
			Func<T, TOther, TResult> resultFunc)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (selectFunc == null)
				throw new ArgumentNullException(nameof(selectFunc));
			if (resultFunc == null)
				throw new ArgumentNullException(nameof(resultFunc));
			return input => 
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
				{
					IParsingResult<TInput, TOther> otherResult = selectFunc(result.Value)(result.Input);
					if (otherResult.Success)
						return new SuccessParsingResult<TInput, TResult>(
							resultFunc(result.Value, otherResult.Value), otherResult.Input, 
                            new ReadOnlyList<ParsingError>(result.Corrections.Concat(otherResult.Corrections).ToArray()));
					return new FailParsingResult<TInput, TResult>(input, otherResult.Error);
				}
				return new FailParsingResult<TInput, TResult>(input, result.Error);
			};
		}

		public static Parser<TInput, List<T>> Many<TInput, T>(this Parser<TInput, T> parser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				List<T> list = new List<T>();
				IParsingResult<TInput, T> result = parser(input);
				while (result.Success)
				{
					list.Add(result.Value);
					input = result.Input;
					result = parser(input);
				}
				return new SuccessParsingResult<TInput, List<T>>(list, input, result.Error);
			};
		}

		public static Parser<TInput, List<T>> Many<TInput, T, TSeparator>(
			this Parser<TInput, T> parser, Parser<TInput, TSeparator> separatorParser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (separatorParser == null)
				throw new ArgumentNullException(nameof(separatorParser));
			return input =>
			{
				List<T> list = new List<T>();
				IParsingResult<TInput, T> result = parser(input);
				while (result.Success)
				{
					list.Add(result.Value);
					input = result.Input;

					IParsingResult<TInput, TSeparator> dividerResult = separatorParser(input);
					if (!dividerResult.Success)
						return new SuccessParsingResult<TInput, List<T>>(list, input, dividerResult.Error);

					result = parser(dividerResult.Input);
				}
				return new SuccessParsingResult<TInput, List<T>>(list, input, result.Error);
			};
		}

		public static Parser<TInput, List<T>> AtLeastOnce<TInput, T>(this Parser<TInput, T> parser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				List<T> list = new List<T>();
				IParsingResult<TInput, T> result = parser(input);
				if (!result.Success)
					return new FailParsingResult<TInput, List<T>>(input, result.Error);
				while (result.Success)
				{
					list.Add(result.Value);
					input = result.Input;
					result = parser(input);
				}
				return new SuccessParsingResult<TInput, List<T>>(list, input, result.Error);
			};
		}

		public static Parser<TInput, List<T>> AtLeastOnce<TInput, T, TSeparator>(
			this Parser<TInput, T> parser, Parser<TInput, TSeparator> separatorParser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (separatorParser == null)
				throw new ArgumentNullException(nameof(separatorParser));
			return input =>
			{
				List<T> list = new List<T>();
				IParsingResult<TInput, T> result = parser(input);
				if (!result.Success)
					return new FailParsingResult<TInput, List<T>>(input, result.Error);
				while (result.Success)
				{
					list.Add(result.Value);
					input = result.Input;

					IParsingResult<TInput, TSeparator> dividerResult = separatorParser(input);
					if (!dividerResult.Success)
						return new SuccessParsingResult<TInput, List<T>>(list, input, dividerResult.Error);
					result = parser(dividerResult.Input);
				}
				return new SuccessParsingResult<TInput, List<T>>(list, input, result.Error);
			};
		}

		public static Parser<TInput, List<T>> Until<TInput, T, TParser>(
			this Parser<TInput, T> parser, Parser<TInput, TParser> conditionParser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				List<T> list = new List<T>();

				IParsingResult<TInput, TParser> conditionResult = conditionParser(input);
				IParserInput<TInput> next = input;
				while (!conditionResult.Success)
				{
					IParsingResult<TInput, T> result = parser(next);
					if (!result.Success)
						return new FailParsingResult<TInput, List<T>>(input, result.Error);
					list.Add(result.Value);
					next = result.Input;
					conditionResult = conditionParser(next);
				}
				return new SuccessParsingResult<TInput, List<T>>(list, next);
			};
		}

		public static Parser<TInput, List<T>> Until<TInput, T, TParser, TDivider>(
			this Parser<TInput, T> parser, Parser<TInput, TParser> conditionParser, 
			Parser<TInput, TDivider> dividerParser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				List<T> list = new List<T>();

				IParsingResult<TInput, TParser> conditionResult = conditionParser(input);
				IParserInput<TInput> next = input;
				if (!conditionResult.Success)
				{
					IParsingResult<TInput, T> result = parser(input);
					if (!result.Success)
						return new FailParsingResult<TInput, List<T>>(input, result.Error);
					next = result.Input;
					conditionResult = conditionParser(next);

					while (!conditionResult.Success)
					{
						IParsingResult<TInput, TDivider> dividerResult = dividerParser(next);
						if (!dividerResult.Success)
							return new FailParsingResult<TInput, List<T>>(input, dividerResult.Error);

						result = parser(dividerResult.Input);
						if (!result.Success)
							return new FailParsingResult<TInput, List<T>>(input, result.Error);

						list.Add(result.Value);
						next = result.Input;
						conditionResult = conditionParser(input);
					}
				}
				return new SuccessParsingResult<TInput, List<T>>(list, next);
			};
		}

		public static Parser<TInput, List<T>> Repeat<TInput, T>(
			this Parser<TInput, T> parser, int times)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (times <= 0)
				throw new ArgumentOutOfRangeException(nameof(times));
			return input =>
			{
				List<T> list = new List<T>();
				IParserInput<TInput> next = input;
				for (int i = 0; i < times; i++)
				{
					IParsingResult<TInput, T> result = parser(input);
					if (!result.Success)
						return new FailParsingResult<TInput, List<T>>(input, result.Error);
					list.Add(result.Value);
					next = result.Input;
				}
				return new SuccessParsingResult<TInput, List<T>>(list, next);
			};
		}

		public static Parser<TInput, string> AsText<TInput>(
			this Parser<TInput, List<char>> parser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input => parser(input).Select(e => new string(e.ToArray()));
		}

		public static Parser<TInput, string> AsText<TInput>(
			this Parser<TInput, IEnumerable<char>> parser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input => parser(input).Select(e =>
			{
				StringBuilder sb = new StringBuilder();
				foreach (char c in e)
					sb.Append(c);
				return sb.ToString();
			});
		}

		public static Parser<TInput, T> Optional<TInput, T>(this Parser<TInput, T> parser)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
					return result;
				return new SuccessParsingResult<TInput, T>(default(T), input, result.Error);
			};
		}

		public static Parser<TInput, T> Optional<TInput, T>(this Parser<TInput, T> parser, 
			T defaultValue)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
					return result;
				return new SuccessParsingResult<TInput, T>(defaultValue, input, result.Error);
			};
		}

		public static Parser<TInput, T> Ref<TInput, T>(Func<Parser<TInput, T>> func)
		{
			if (func == null)
				throw new ArgumentNullException(nameof(func));
			return input => func()(input);
		}

		public static Parser<TInput, T> Eof<TInput, T>(this Parser<TInput, T> parser, 
			string errorMessage = null)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			return input =>
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
					if (!result.Input.Eof)
						return new FailParsingResult<TInput, T>(input, new ParsingError()
						{
							Position = result.Input.Position,
							Message = errorMessage ?? "End of input expected",
							Method = "Eof<TInput, T>(Parser<TInput, T>, string)",
							Type = typeof(T)
						});
				return result;
			};
		}

		public static Parser<TInput, T> Where<TInput, T>(this Parser<TInput, T> parser, 
			Predicate<T> predicate, string errorMessage = null)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));
			return input =>
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
					if (!predicate(result.Value))
						return new FailParsingResult<TInput, T>(input, new ParsingError()
						{
							Position = input.Position,
							Message = errorMessage ?? "Condition fails",
							Method = "Where<TInput, T>(Parser<TInput, T>, Predicate<T>, string)",
							Type = typeof(T)
						});
				return result;
			};
		}

		public static Parser<TInput, T> Enclose<TInput, T, TEnclosing>(
			this Parser<TInput, T> parser, Parser<TInput, TEnclosing> enclosingParser)
		{
			return from en1 in enclosingParser
				from result in parser
				from en2 in enclosingParser
				select result;
		}

		public static readonly Parser<char, string> Ident = 
			from firstChar in Char(c => char.IsLetter(c) || c == '_', "Identifier expected")
			from nextChars in Char(c => char.IsLetterOrDigit(c) || c == '_', string.Empty).Many().AsText()
			select firstChar + nextChars;

		public static readonly Parser<char, string> QuotedString = 
			(from quote1 in Char('"')
			from chars in Char(c => c != '"', "End of string expected").Many().AsText()
			from quote2 in Char('"')
			select chars).AtLeastOnce().Select(c => string.Join("\"", c));

		public static readonly Parser<char, string> Whitespaces = Char(char.IsWhiteSpace, "Whitespace expected").AtLeastOnce().AsText();

		private static readonly Parser<char, string> _digits = Char(c => c >= '0' && c <= '9', "Digit expected").AtLeastOnce().AsText();

		private static readonly Parser<char, string> _float =
			from sign in Char('-').Or(Char('+')).Optional('+')
			from integer in _digits
			from fraction in
				(from point in Char('.')
					from fraction in _digits
					select point + fraction).Optional(string.Empty)
			from exponent in
				(from e in Char('e', true)
					from esign in Char('-').Or(Char('+')).Optional('+')
					from exponent in _digits
					select e + esign + exponent).Optional(string.Empty)
			select integer + fraction + exponent;

		public static readonly Parser<char, float> Float = _float.
			Select(f => float.Parse(f, CultureInfo.InvariantCulture)).
			Catch("Floating point number overflow");
	
		public static readonly Parser<char, double> Double = _float.
			Select(f => double.Parse(f, CultureInfo.InvariantCulture)).
			Catch("Floating point number overflow");
	
		public static readonly Parser<char, int> Int32 = 
			(from sign in Char('-').Or(Char('+')).Optional('+')
			from digits in Char(c => c >= '0' && c <= '9', "Digit expected").AtLeastOnce().AsText()
			select int.Parse(sign + digits)).ErrorMessage("Integer expected").Catch("Integer overflow");

		public static readonly Parser<char, long> Int64 = 
			(from sign in Char('-').Or(Char('+')).Optional('+')
			from digits in Char(c => c >= '0' && c <= '9', "Digit expected").AtLeastOnce().AsText()
			select long.Parse(sign + digits)).ErrorMessage("Integer expected").Catch("Integer overflow");

		public static readonly Parser<char, string> NewLine = String("\x000D\x000A").
			Or(String("\x000D"), String("\x000A"), String("\x0085"), String("\x2028"), String("\x2029"));

		public static Parser<char, string> SingleLineComment(string startToken)
		{
			if (startToken == null)
				throw new ArgumentNullException(nameof(startToken));
			if (startToken.Length == 0)
				throw new ArgumentException("Start token cannot be empty", nameof(startToken));
			return from token in String(startToken)
				from str in Char(c => c != '\x000D' && c != '\x000A' && c != '\x0085' && c != '\x2028' && c != '\x2029', "New line expected").Many().AsText()
				from newLine in NewLine.Optional()
				select str;
		}

		public static Parser<char, string> TillToken(string token, bool ignoreCase = false)
		{
			if (token == null)
				throw new ArgumentNullException(nameof(token));
			if (token.Length == 0)
				throw new ArgumentException("Token cannot be empty", nameof(token));

			return Until(Char(c => true, "Any char"), String(token)).AsText();
		}

		public static Parser<char, string> MultiLineComment(string startToken, 
			string endToken)
		{
			return 
				from tokenStart in String(startToken)
				from str in TillToken(endToken)
				from tokenEnd in String(endToken)
				select str;
		}

		public static Parser<char, T> TrimRight<T>(this Parser<char, T> parser)
		{
			return 
				from value in parser
				from ws in Whitespaces.Optional()
				select value;
		}

		public static Parser<char, T> TrimLeft<T>(this Parser<char, T> parser)
		{
			return 
				from ws in Whitespaces.Optional()
				from value in parser
				select value;
		}

		public static Parser<char, T> Trim<T>(this Parser<char, T> parser)
		{
			return 
				from ws in Whitespaces.Optional()
				from value in parser
				from ws1 in Whitespaces.Optional()
				select value;
		}

		public static Parser<TInput, T> RequiredIf<TInput, T, TPrefix>(
			this Parser<TInput, T> parser, Parser<TInput, TPrefix> prefix)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (prefix == null)
				throw new ArgumentNullException(nameof(prefix));

			return input =>
			{
				IParsingResult<TInput, TPrefix> prefixResult = prefix(input);
				
				if (prefixResult.Success)
					return parser(prefixResult.Input);

				return new SuccessParsingResult<TInput, T>(default(T), input);
			};
		}

		public static Parser<TInput, List<T>> RequiredWhile<TInput, T, TPrefix>(
			this Parser<TInput, T> parser, Parser<TInput, TPrefix> prefix)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (prefix == null)
				throw new ArgumentNullException(nameof(prefix));

			return input =>
			{
				List<T> list = new List<T>();
				IParserInput<TInput> next = input;
				IParsingResult<TInput, TPrefix> prefixResult = prefix(input);
				while (prefixResult.Success)
				{
					IParsingResult<TInput, T> result = parser(prefixResult.Input);
					if (!result.Success)
						return new FailParsingResult<TInput, List<T>>(input, result.Error);
					list.Add(result.Value);
					next = result.Input;
					prefixResult = prefix(next);
				}
				return new SuccessParsingResult<TInput, List<T>>(list, next);
			};
		}

		public static Parser<TInput, TResult> Then<TInput, TResult, T>(
			this Parser<TInput, T> parser, Func<T, Parser<TInput, TResult>> func)
		{
			if (parser == null)
				throw new ArgumentNullException(nameof(parser));
			if (func == null)
				throw new ArgumentNullException(nameof(func));

			return input =>
			{
				IParsingResult<TInput, T> result = parser(input);
				if (result.Success)
					return func(result.Value)(input);
				return new FailParsingResult<TInput, TResult>(input, result.Error);
			};
		}
	}
}