using System;
using com.calitha.goldparser;

namespace com.calitha.goldparser
{

	/// <summary>
	/// Abstract class representing both terminal and nonterminal tokens.
	/// </summary>
	public abstract class Token
	{
		private Object userObject;

		public Token()
		{
			this.userObject = null;
		}

		/// <summary>
		/// This can be user for storing an object during the reduce
		/// event. This makes it possible to create a tree when the
		/// source is being parsed.
		/// </summary>
		public Object UserObject
		{
			get {return userObject;}
			set {this.userObject = value;}
		}

	}

	/// <summary>
	/// Terminal token objects are retrieved from the tokenizer.
	/// </summary>
	public class TerminalToken : Token
	{
		private SymbolTerminal symbol;
		private string text;
		private Location location;

		/// <summary>
		/// Creates a new terminal token object.
		/// </summary>
		/// <param name="symbol">The symbol that this token represents.</param>
		/// <param name="text">The text from the input that is the token.</param>
		/// <param name="location">The location in the input that this token
		/// has been found.</param>
		public TerminalToken(SymbolTerminal symbol, string text, Location location)
		{
			this.symbol = symbol;
			this.text   = text;
			this.location = location;
		}
		
		/// <summary>
		/// String representation of the token.
		/// </summary>
		/// <returns></returns>
		public override String ToString()
		{
			return text;
		}

		/// <summary>
		/// The symbol that this token represents.
		/// </summary>
		public SymbolTerminal Symbol {get {return symbol;}}

		/// <summary>
		/// The text from the input that is this token.
		/// </summary>
		public string Text {get {return text;}}

		/// <summary>
		/// The location in the input that this token was found.
		/// </summary>
		public Location Location {get {return location;}}
	}

	/// <summary>
	/// The nonterminal token is created when tokens are reduced by a rule.
	/// </summary>
	public class NonterminalToken : Token
	{
		private Token[] tokens;
		private Rule rule;

		/// <summary>
		/// Creates a new nonterminal token.
		/// </summary>
		/// <param name="rule">The reduction rule.</param>
		/// <param name="tokens">The tokens that are reduced.</param>
		public NonterminalToken(Rule rule, Token[] tokens)
		{
			this.rule = rule;
			this.tokens = tokens;			
		}

		public void ClearTokens()
		{
			tokens = new Token[0];
		}

		/// <summary>
		/// String representation of the nonterminal token.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString()
		{
			String str = rule.Lhs+" = [";
			for (int i = 0; i < tokens.Length; i++)
			{
				str += tokens[i]+"]";
			}
			return str;
		}


		/// <summary>
		/// The symbol that this nonterminal token represents.
		/// </summary>
		public SymbolNonterminal Symbol {get{return rule.Lhs;}}

		/// <summary>
		/// The tokens that are reduced.
		/// </summary>
		public Token[] Tokens {get{return tokens;}}

		/// <summary>
		/// The rule that caused the reduction.
		/// </summary>
		public Rule Rule {get{return rule;}}

	}


}
