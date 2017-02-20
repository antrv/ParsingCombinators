using System;

namespace AR.Math.Expressions
{
	[Serializable]
	public abstract class Operator: Function
	{
		internal Operator(string name)
			: base(name)
		{
		}

		public virtual int Priority => 0;

	    public abstract OperatorType Type
	    {
	        get;
	    }

		public abstract string Symbol
		{
			get;
		}
	}
}