using System;

namespace AR.Parsing
{
	internal static class Helpers
	{
		public static IParsingResult<TInput, TResult> Select<TInput, T, TResult>(
			this IParsingResult<TInput, T> result, Func<T, TResult> func)
		{
			if (result.Success)
				return new SuccessParsingResult<TInput, TResult>(func(result.Value), result.Input, result.Corrections, result.Error);
			return new FailParsingResult<TInput, TResult>(result.Input, result.Corrections, result.Error);
		}

		public static IParsingResult<TInput, T> ErrorMessage<TInput, T>(
			this IParsingResult<TInput, T> result, string errorMessage)
		{
			if (result.Success)
				return result;
			ParsingError error = result.Error;
			error.Message = errorMessage;
			return new FailParsingResult<TInput, T>(result.Input, result.Corrections, error);
		}

		public static IParsingResult<TInput, T> Value<TInput, T>(
			this IParsingResult<TInput, T> result, T value)
		{
			if (result.Success)
				return new SuccessParsingResult<TInput, T>(value, result.Input, result.Corrections);
			return result;
		}

		public static IParsingResult<TInput, TResult> Value<TInput, T, TResult>(
			this IParsingResult<TInput, T> result, TResult value)
		{
			if (result.Success)
				return new SuccessParsingResult<TInput, TResult>(value, result.Input, result.Corrections, result.Error);
			return new FailParsingResult<TInput, TResult>(result.Input, result.Corrections, result.Error);
		}
	}
}