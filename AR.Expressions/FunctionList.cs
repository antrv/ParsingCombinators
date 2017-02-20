using System;
using System.Collections.ObjectModel;

namespace AR.Expressions
{
	[Serializable]
	public sealed class FunctionList: KeyedCollection<string, Function>
	{
		public FunctionList()
			: base(StringComparer.InvariantCultureIgnoreCase)
		{
		}

	    protected override string GetKeyForItem(Function item)
	    {
	        return item.Id;
	    }
	}
}