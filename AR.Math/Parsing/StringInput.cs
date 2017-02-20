namespace AR.Math.Parsing
{
	public sealed class StringInput: IParserInput<char>
	{
		private readonly string _text;
		private readonly int _position;
	    private StringInput _next;

		public StringInput(string text)
		{
			_text = text ?? string.Empty;
		}

		public StringInput(string text, int position)
		{
			_text = text ?? string.Empty;
			_position = position;
		}

		public int Position => _position;
	    public char Current => _text[_position];
	    public bool Eof => _position >= _text.Length;
	    public IParserInput<char> Next => _next ?? (_next = new StringInput(_text, _position + 1));
	}
}