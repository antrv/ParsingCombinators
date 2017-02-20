namespace AR.Expressions
{
	public static class Constants
	{
		/// <summary>
		/// The number π is a mathematical constant that is the ratio of a circle's circumference to its diameter and is approximately equal to 3.14159.
		/// </summary>
		public static readonly Constant Pi = new Constant("@pi");

		/// <summary>
		/// The number e is a mathematical constant that is the base of the natural logarithm. It is approximately equal to 2.71828.
		/// </summary>
		public static readonly Constant E = new Constant("@e");

		/// <summary>
		/// The imaginary unit is a mathematical concept which extends the real number system to the complex number system. i^2 = −1.
		/// </summary>
		public static readonly Constant I = new Constant("@i");

		/// <summary>
		/// Infinity ∞ is an abstract concept describing something without any limit and is relevant in a number of fields.
		/// </summary>
		public static readonly Constant Infinity = new Constant("@inf");

		/// <summary>
		/// Infinity ∞ is an abstract concept describing something without any limit and is relevant in a number of fields.
		/// </summary>
		public static readonly Constant PositiveInfinity = new Constant("@pinf");

		/// <summary>
		/// Infinity ∞ is an abstract concept describing something without any limit and is relevant in a number of fields.
		/// </summary>
		public static readonly Constant NegativeInfinity = new Constant("@ninf");
	}
}