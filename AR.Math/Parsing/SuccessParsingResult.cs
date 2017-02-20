using System;
using System.Collections.Generic;

namespace AR.Math.Parsing
{
    internal sealed class SuccessParsingResult<TInput, TValue> : IParsingResult<TInput, TValue>
    {
        private readonly TValue _value;
        private readonly IParserInput<TInput> _next;
        private readonly ParsingError _continuationError;
        private readonly IReadOnlyList<ParsingError> _corrections;

        public SuccessParsingResult(TValue value, IParserInput<TInput> next)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _value = value;
            _next = next;
            _corrections = ReadOnlyList<ParsingError>.Empty;
        }

        public SuccessParsingResult(TValue value, IParserInput<TInput> next,
            ParsingError continuationError)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _value = value;
            _next = next;
            _continuationError = continuationError;
            _corrections = ReadOnlyList<ParsingError>.Empty;
        }

        public SuccessParsingResult(TValue value, IParserInput<TInput> next,
            IReadOnlyList<ParsingError> corrections)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _value = value;
            _next = next;
            _corrections = corrections ?? ReadOnlyList<ParsingError>.Empty;
        }

        public SuccessParsingResult(TValue value, IParserInput<TInput> next,
            IReadOnlyList<ParsingError> corrections, ParsingError continuationError)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _value = value;
            _next = next;
            _continuationError = continuationError;
            _corrections = corrections ?? ReadOnlyList<ParsingError>.Empty;
        }

        bool IParsingResult<TInput, TValue>.Success => true;
        TValue IParsingResult<TInput, TValue>.Value => _value;
        public IParserInput<TInput> Input => _next;
        public IReadOnlyList<ParsingError> Corrections => _corrections;
        public ParsingError Error => _continuationError;

        public override string ToString()
        {
            return "Success: " + _value;
        }
    }
}