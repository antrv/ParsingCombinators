namespace AR.Math.Parsing
{
	public sealed class StringInput: IParserInput<char>
	{
		private readonly string _text;
		private readonly int _position;

		public StringInput(string text)
            : this(text, 0)
		{
		}

		public StringInput(string text, int position)
		{
			_text = text ?? string.Empty;
			_position = position;
		}

		public int Position => _position;
	    public char Current => _text[_position];
	    public bool Eof => _position >= _text.Length;
	    public IParserInput<char> Next => new StringInput(_text, _position + 1);
        public static implicit operator StringInput(string s) => new StringInput(s);
    }
}