using System;
using System.Collections;

namespace com.calitha.goldparser.lalr
{

	/// <summary>
	/// An AcceptAction is an action in a LALR state which means that the input for the
	/// LALR parser is tokenized, parsed and accepted .
	/// </summary>
	public class AcceptAction  : Action
	{
		/// <summary>
		/// Creates a new accept action.
		/// </summary>
		/// <param name="symbol">The symbol that a token must be for it to be accepted.</param>
		public AcceptAction(SymbolTerminal symbol)
		{
			this.symbol = symbol;
		}

		/// <summary>
		/// The symbol that a token must be for it to be accepted.
		/// </summary>
		public SymbolTerminal Symbol{get{return (SymbolTerminal)symbol;}}

	}
}
