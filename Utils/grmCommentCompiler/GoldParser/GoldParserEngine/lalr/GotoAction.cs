using System;
using System.Collections;

namespace com.calitha.goldparser.lalr
{

	/// <summary>
	/// A GotoAction is an action that tells the LALR parser to go to a new state.
	/// A goto action happens after a reduction.
	/// </summary>
	public class GotoAction	: Action
	{
		private State state;

		/// <summary>
		/// Creates a new goto action. 
		/// </summary>
		/// <param name="symbol">The symbol that a reduction must be so that
		/// the goto action will be done.</param>
		/// <param name="state">The new current state for the LALR parser.</param>
		public GotoAction(SymbolNonterminal symbol, State state)
		{
			this.symbol = symbol;
			this.state = state;
		}

		/// <summary>
		/// The symbol that is the criteria for the action to be done.
		/// </summary>
		public SymbolNonterminal Symbol {get{return (SymbolNonterminal)symbol;}}

		/// <summary>
		/// The new current state for the LALR parser.
		/// </summary>
		public State State {get{return state;}}
	}
}
