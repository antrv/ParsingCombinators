using System.Collections.Generic;

namespace AR.Parsing
{
    public interface IParsingResult<out TInput, out TValue>
    {
        bool Success { get; }
        TValue Value { get; }
        IParserInput<TInput> Input { get; }
        IReadOnlyList<ParsingError> Corrections { get; }
        ParsingError Error { get; }
    }
}