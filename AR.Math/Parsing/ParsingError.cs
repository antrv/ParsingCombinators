using System;

namespace AR.Math.Parsing
{
	public struct ParsingError
	{
		public int Position;
		public string Message;
		public Exception Exception;
		public string Method;
		public Type Type;
	}
}