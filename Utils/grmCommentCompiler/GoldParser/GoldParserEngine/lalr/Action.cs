using System;
using System.Collections;
using System.Collections.Specialized;
using com.calitha.goldparser;

namespace com.calitha.goldparser.lalr
{

	/// <summary>
	/// Type-safe list of Action items.
	/// </summary>
	public class ActionCollection : IEnumerable
	{
		private IDictionary table;

		public ActionCollection()
		{
			table = new HybridDictionary();
		}

		public IEnumerator GetEnumerator()
		{
			return table.Values.GetEnumerator();
		}

		public void Add(Action action)
		{
			table.Add(action.symbol,action);
		}

		public Action Get(Symbol symbol)
		{
			return table[symbol] as Action;
		}

		public Action this[Symbol symbol]
		{
			get { return Get(symbol);}
		}
	}

	/// <summary>
	/// Abstract action class. All actions in a LALR must be inherited from this class.
	/// </summary>
	public abstract class Action
	{
		internal Symbol symbol;
	}
}
