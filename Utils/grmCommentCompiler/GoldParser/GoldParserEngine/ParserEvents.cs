using System;
using com.calitha.goldparser.lalr;

namespace com.calitha.goldparser
{

	/// <summary>
	/// Event arguments for the TokenRead event.
	/// </summary>
	public class TokenReadEventArgs : EventArgs
	{
		private TerminalToken token;
		private bool contin;

		public TokenReadEventArgs(TerminalToken token)
		{
			this.token = token;
			contin = true;
		}

		/// <summary>
		/// The terminal token that will be processed by the LALR parser.
		/// </summary>
		public TerminalToken Token {get{return token;}}

		/// <summary>
		/// Determines if the parse process should continue
		/// after this event. True by default.
		/// </summary>
		public bool Continue
		{
			get {return contin;} 
			set {contin = value;}
		}

	}

	/// <summary>
	/// Event arguments for the Shift event.
	/// </summary>
	public class ShiftEventArgs : EventArgs
	{
		private TerminalToken token;
		private State newState;

		public ShiftEventArgs(TerminalToken token, State newState)
		{
			this.token = token;
			this.newState = newState;
		}

		/// <summary>
		/// The terminal token that is shifted onto the stack.
		/// </summary>
		public TerminalToken Token{get{return token;}}

		/// <summary>
		/// The state that the parser is in after the shift.
		/// </summary>
		public State NewState{get{return newState;}}
	}

	/// <summary>
	/// Event arguments for the Reduce event.
	/// </summary>
	public class ReduceEventArgs : EventArgs
	{
		private Rule rule;
		private NonterminalToken token;
		private State newState;
		private bool contin;

		public ReduceEventArgs(Rule rule, NonterminalToken token, State newState)
		{
			this.rule = rule;
			this.token = token;
			this.newState = newState;
			this.contin = true;
		}

		/// <summary>
		/// The rule that was used to reduce tokens.
		/// </summary>
		public Rule Rule{get{return rule;}}

		/// <summary>
		/// The nonterminal token that consists of nonterminal or terminal
		/// tokens that has been reduced by the rule.
		/// </summary>
		public NonterminalToken Token{get{return token;}}

		/// <summary>
		/// The state after the reduction.
		/// </summary>
		public State NewState{get{return newState;}}

		/// <summary>
		/// Determines if the parse process should continue
		/// after this event. True by default.
		/// </summary>
		public bool Continue
		{
			get {return contin;} 
			set {contin = value;}
		}
	}

	/// <summary>
	/// Event arguments after a goto event.
	/// </summary>
	public class GotoEventArgs : EventArgs
	{
		private SymbolNonterminal symbol;
		private State newState;

		public GotoEventArgs(SymbolNonterminal symbol, State newState)
		{
			this.symbol = symbol;
			this.newState = newState;
		}

		/// <summary>
		/// The symbol that causes the goto event.
		/// </summary>
		public SymbolNonterminal Symbol{get{return symbol;}}

		/// <summary>
		/// The state after the goto event.
		/// </summary>
		public State NewState{get{return newState;}}
	}

	/// <summary>
	/// Event argument for an Accept event.
	/// </summary>
	public class AcceptEventArgs : EventArgs
	{
		private NonterminalToken token;

		public AcceptEventArgs(NonterminalToken token)
		{
			this.token = token;
		}

		/// <summary>
		/// The fully reduced nonterminal token that consists of
		/// all the other reduced tokens.
		/// </summary>
		public NonterminalToken Token{get{return token;}}
	}

	/// <summary>
	/// Event arguments for a token read error.
	/// </summary>
	public class TokenErrorEventArgs : EventArgs
	{
		private TerminalToken token;
		private bool contin;

		public TokenErrorEventArgs(TerminalToken token)
		{
			this.token = token;
			this.contin = false;
		}

		/// <summary>
		/// The error token that also consists of the character that causes the
		/// token read error.
		/// </summary>
		public TerminalToken Token {get{return token;}}

		/// <summary>
		/// The continue property can be set during the token error event,
		/// to continue the parsing process. The current token will be ignored.
		/// Default value is false.
		/// </summary>
		public bool Continue
		{
			get{return contin;}
			set{this.contin = value;}
		}
	}

	/// <summary>
	/// Event arguments for the Parse Error event.
	/// </summary>
	public class ParseErrorEventArgs : EventArgs
	{
		private TerminalToken unexpectedToken;
		private SymbolCollection expectedTokens;
		private bool contin;
		private TerminalToken nextToken;

		public ParseErrorEventArgs(TerminalToken unexpectedToken,
			                       SymbolCollection expectedTokens)
		{
			this.unexpectedToken = unexpectedToken;
			this.expectedTokens = expectedTokens;
			this.contin = false;
			this.nextToken = null;
		}

		/// <summary>
		/// The token that caused this parser error.
		/// </summary>
		public TerminalToken UnexpectedToken { get{return unexpectedToken;}}

		/// <summary>
		/// The symbols that were expected by the parser.
		/// </summary>
		public SymbolCollection ExpectedTokens{get{return expectedTokens;}}

		/// <summary>
		/// The continue property can be set during the parse error event.
		/// to continue the parsing process. 
		/// Default value is false.
		/// </summary>
		public bool Continue
		{
			get{return contin;}
			set{this.contin = value;}
		}

		/// <summary>
		/// If the continue property is set to true, then NextToken will be the
		/// next token to be used as input to the parser (it will become the lookahead token).
		/// The default value is null, which means that the next token will be read from the
		/// normal input stream.
		/// stream.
		/// </summary>
		public TerminalToken NextToken
		{
			get{return nextToken;}
		}

	}
}
