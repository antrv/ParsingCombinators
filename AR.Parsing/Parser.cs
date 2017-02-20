namespace AR.Parsing
{
	public delegate IParsingResult<TInput, TResult> Parser<TInput, out TResult>(IParserInput<TInput> input);
}
