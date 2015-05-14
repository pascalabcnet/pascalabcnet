using System;
using System.Collections;

namespace com.calitha.goldparser.lalr
{

	/// <summary>
	/// ReduceAction is an action that tells the LALR parser to reduce tokens according
	/// to a rule.
	/// </summary>
	public class ReduceAction  : Action
	{
		private Rule rule;

		/// <summary>
		/// Creates a new ReduceAction.
		/// </summary>
		/// <param name="symbol">The symbol that a token must be for this action
		/// to be done.</param>
		/// <param name="rule">The rule to be used to reduce tokens.</param>
		public ReduceAction(SymbolTerminal symbol, Rule rule)
		{
			this.symbol = symbol;
			this.rule = rule;
		}

		/// <summary>
		/// the criteria of this action to be done.
		/// </summary>
		public SymbolTerminal Symbol {get{return (SymbolTerminal)symbol;}}

		/// <summary>
		/// The rule to reduce the tokens.
		/// </summary>
		public Rule Rule {get{return rule;}}
	}
}
