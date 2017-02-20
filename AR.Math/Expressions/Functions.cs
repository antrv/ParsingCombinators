namespace AR.Math.Expressions
{
	public static class Functions
	{
		public static readonly Function Sqrt = new PredefinedFunction("Sqrt", System.Math.Sqrt);
		
		public static readonly Function Exp = new PredefinedFunction("Exp", System.Math.Exp);
		public static readonly Function Log = new PredefinedFunction("Log", (a, b) => System.Math.Log(a, b));
		public static readonly Function Ln = new PredefinedFunction("Ln", a => System.Math.Log(a));

		public static readonly Function Sin = new PredefinedFunction("Sin", System.Math.Sin);
		public static readonly Function Cos = new PredefinedFunction("Cos", System.Math.Cos);
		public static readonly Function Tan = new PredefinedFunction("Tan", System.Math.Tan);
		public static readonly Function Cot = new UserFunction("Cot", a => 1 / Tan.Apply(a));
		public static readonly Function Sec = new UserFunction("Sec", a => 1 / Cos.Apply(a));
		public static readonly Function Cosec = new UserFunction("Cosec", a => 1 / Sin.Apply(a));
		
		public static readonly Function Arcsin = new PredefinedFunction("Arcsin", System.Math.Asin);
		public static readonly Function Arccos = new PredefinedFunction("Arccos", System.Math.Acos);
		public static readonly Function Arctan = new PredefinedFunction("Arctan", System.Math.Atan);
		public static readonly Function Arccot = new UserFunction("Arccot", a => Arctan.Apply(1 / a));
		public static readonly Function Arcsec = new UserFunction("Arcsec", a => Arccos.Apply(1 / a));
		public static readonly Function Arccosec = new UserFunction("Arccosec", a => Arcsin.Apply(1 / a));

		public static readonly Function Sinh = new UserFunction("Sinh", a => (Exp.Apply(a) - Exp.Apply(-a)) / 2);
		public static readonly Function Cosh = new UserFunction("Cosh", a => (Exp.Apply(a) + Exp.Apply(-a)) / 2);
		public static readonly Function Tanh = new UserFunction("Tanh", a => (Exp.Apply(a) - Exp.Apply(-a)) / (Exp.Apply(a) + Exp.Apply(-a)));
		public static readonly Function Coth = new UserFunction("Coth", a => (Exp.Apply(a) + Exp.Apply(-a)) / (Exp.Apply(a) - Exp.Apply(-a)));
		public static readonly Function Sech = new UserFunction("Sech", a => 1 / Cosh.Apply(a));
		public static readonly Function Csch = new UserFunction("Csch", a => 1 / Sinh.Apply(a));

		public static readonly Function Arcsinh = new UserFunction("Arcsinh", a => Ln.Apply(a + Sqrt.Apply(a * a + 1)));
		public static readonly Function Arccosh = new UserFunction("Arccosh", a => Ln.Apply(a + Sqrt.Apply(a + 1) * Sqrt.Apply(a - 1)));
		public static readonly Function Arctanh = new UserFunction("Arctanh", a => Ln.Apply((1 + a) / (1 - a)) / 2);
		public static readonly Function Arccoth = new UserFunction("Arccoth", a => Ln.Apply((a + 1) / (a - 1)) / 2);
		public static readonly Function Arcsech = new UserFunction("Arcsech", a => Ln.Apply(1 / a + Sqrt.Apply(1 / a + 1) * Sqrt.Apply(1 / a - 1)));
		public static readonly Function Arccsch = new UserFunction("Arccsch", a => Ln.Apply(1 / a + Sqrt.Apply(1 / (a * a) + 1)));
	}
}