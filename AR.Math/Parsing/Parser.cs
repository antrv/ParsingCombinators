namespace AR.Math.Parsing
{
	public delegate IParsingResult<TInput, TResult> Parser<TInput, out TResult>(IParserInput<TInput> input);
}
