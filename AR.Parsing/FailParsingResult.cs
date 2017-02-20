using System.Collections.Generic;

namespace AR.Parsing
{
	internal sealed class FailParsingResult<TInput, TValue>: IParsingResult<TInput, TValue>
	{
		private readonly IParserInput<TInput> _input;
		private readonly ParsingError _error;
		private readonly IReadOnlyList<ParsingError> _corrections;

		public FailParsingResult(IParserInput<TInput> input, ParsingError error)
		{
			_input = input;
			_corrections = ReadOnlyList<ParsingError>.Empty;
			_error = error;
		}

		public FailParsingResult(IParserInput<TInput> input, ParsingError[] corrections, ParsingError error)
		{
			_input = input;
			_corrections = corrections == null ? ReadOnlyList<ParsingError>.Empty : new ReadOnlyList<ParsingError>(corrections);
			_error = error;
		}

		public FailParsingResult(IParserInput<TInput> input, IReadOnlyList<ParsingError> corrections, ParsingError error)
		{
			_input = input;
			_corrections = corrections;
			_error = error;
		}

		bool IParsingResult<TInput, TValue>.Success => false;
	    TValue IParsingResult<TInput, TValue>.Value => default(TValue);
	    public IParserInput<TInput> Input => _input;
	    public IReadOnlyList<ParsingError> Corrections => _corrections;
	    public ParsingError Error => _error;

	    public override string ToString()
		{
			return "Error in position " + _error.Position + ": " + _error.Message;
		}
	}
}