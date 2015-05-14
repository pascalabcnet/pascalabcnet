using System;
using System.Collections;
using com.calitha.commons;

namespace com.calitha.goldparser.dfa
{
	/// <summary>
	/// DFA State.
	/// </summary>
	public class State
	{
		private int id;
		private TransitionCollection transitions;

		/// <summary>
		/// Creates a new DFA state.
		/// </summary>
		/// <param name="id">The id of this state.</param>
		public State(int id)
		{
			this.id = id;
			transitions = new TransitionCollection();
		}	

		/// <summary>
		/// The id of the DFA state.
		/// </summary>
		public int Id {get {return id;}}

		/// <summary>
		/// The transitions (edges) to other states.
		/// </summary>
		public TransitionCollection Transitions {get {return transitions;}}
	}
	
	/// <summary>
	/// A specific type of DFA state. When the current state of the DFA is an EndState,
	/// then it means the input so far can be a token.
	/// </summary>
	public class EndState : State
	{
		private SymbolTerminal acceptSymbol;

		public EndState(int id, SymbolTerminal acceptSymbol) : base(id)
		{
			this.acceptSymbol = acceptSymbol;
		}
		
		/// <summary>
		/// The accept symbol for the DFA.
		/// </summary>
		public SymbolTerminal AcceptSymbol {get {return acceptSymbol;}}
	}

	/// <summary>
	/// Type-safe list for DFA states.
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
			get { return Get(index);}
		}

	}

	
}
