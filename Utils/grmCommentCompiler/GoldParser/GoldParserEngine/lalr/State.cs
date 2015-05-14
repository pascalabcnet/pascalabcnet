using System;
using System.Collections;
using com.calitha.commons;

namespace com.calitha.goldparser.lalr
{
	/// <summary>
	/// State is a LALR state.
	/// </summary>
	public class State
	{
		private int id;
		private ActionCollection actions;

		/// <summary>
		/// Creates a new LALR state.
		/// </summary>
		/// <param name="id">The id of the state.</param>
		public State(int id)
		{
			this.id = id;
			actions = new ActionCollection();
		}

		/// <summary>
		/// Id of the state.
		/// </summary>
		public int Id {get{return id;}}

		/// <summary>
		/// Actions in this state. An action will be done depending on the
		/// symbol of the token.
		/// </summary>
		public ActionCollection Actions {get{return actions;}}
	}

	/// <summary>
	/// Type-safe list of LALR states.
	/// </summary>
	public class StateCollection : IEnumerable
	{
		
		private IList list;
		
		public StateCollection()
		{
			list = new ArrayList();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
		
		public void Add(State state)
		{
			list.Add(state);
		}
		
		public State Get(int index)
		{
			return list[index] as State;
		}

		public State this[int index]
		{
			get {return Get(index);}
		}
	}

}
