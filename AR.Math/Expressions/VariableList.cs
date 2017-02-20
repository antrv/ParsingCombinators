using System;
using System.Collections.ObjectModel;

namespace AR.Math.Expressions
{
	[Serializable]
	public sealed class VariableList: KeyedCollection<string, Variable>
	{
		public VariableList()
			: base(StringComparer.InvariantCulture)
		{
		}

	    protected override string GetKeyForItem(Variable item)
	    {
	        return item.Id;
	    }
	}
}