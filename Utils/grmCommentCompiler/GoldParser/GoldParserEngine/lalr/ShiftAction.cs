using System;
using System.Collections;

namespace com.calitha.goldparser.lalr
{

	/// <summary>
	/// ShiftAction is an action to shift a token to the token stack.
	/// </summary>
	public class ShiftAction : Action
	{
		private State state;

		/// <summary>
		/// Creates a new shift action.
		/// </summary>
		/// <param name="symbol">The symbol that the token must be for this action to be done.</param>
		/// <param name="state">The new current state for the LALR parser.</param>
		public ShiftAction(SymbolTerminal symbol, State state)
		{
			this.symbol = symbol;
			this.state = state;
		}

		/// <summary>
		/// The criteria for this action to be done.
		/// </summary>
		public SymbolTerminal Symbol {get{ return (SymbolTerminal)symbol;}}

		/// <summary>
		/// The new current state for the LALR parser.
		/// </summary>
		public State State {get{ return state;}}

	}
}
