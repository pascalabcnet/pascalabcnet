using System;
using System.Text;
using com.calitha.goldparser.lalr;


namespace com.calitha.goldparser
{

	/// <summary>
	/// The LALR Parser is used to parse a source string into tokens and rules.
	/// </summary>
	public class LALRParser
	{

		public enum StoreTokensMode {Always, NoUserObject, Never}

		private IStringTokenizer tokenizer;
		private StateCollection states;
		private State startState;
		private StateStack stateStack;
		private TokenStack tokenStack;
		private TerminalToken lookahead;
		private bool continueParsing;
		private bool accepted;
		private bool trimReductions;
		private StoreTokensMode storeTokens;

		/// <summary>
		/// Creates a new LALR parser.
		/// </summary>
		/// <param name="tokenizer">A tokenizer.</param>
		/// <param name="states">The LALR states.</param>
		/// <param name="startState">The starting state.</param>
		public LALRParser(IStringTokenizer tokenizer,
			StateCollection states,
			State startState)
		{
			this.tokenizer = tokenizer;
			this.states = states;
			this.startState = startState;
			this.trimReductions = trimReductions;
			storeTokens = StoreTokensMode.NoUserObject;
		}

		private void Reset()
		{
			stateStack = new StateStack();
			stateStack.Push(startState);
			tokenStack = new TokenStack();
			lookahead = null;
			continueParsing = true;
			accepted = false;
		}

		/// <summary>
		/// Parse the input with tokens and rules.
		/// </summary>
		/// <param name="input">The source input</param>
		/// <returns>The nonterminal token that the input has been reduced to.
		/// Null if the parse has failed.</returns>
		public NonterminalToken Parse(String input)
		{
			Reset();
			tokenizer.SetInput(input);

			while (continueParsing)
			{
				TerminalToken token = GetLookahead();
				if (token != null)
					ParseTerminal(token);
			}
			if (accepted)
				return (NonterminalToken)tokenStack.Pop();
			else
				return null;
		}

		private void DoShift(TerminalToken token, ShiftAction action)
		{
			stateStack.Push(action.State);
			tokenStack.Push(token);
			lookahead = null;
			if (OnShift != null)
				OnShift(this,new ShiftEventArgs(token,action.State));
		}

		private void DoReduce(Token token, ReduceAction action)
		{
			int reduceLength = action.Rule.Rhs.Length;

			State currentState;
			// Do not reduce if the rule is single nonterminal and TrimReductions is on
			bool skipReduce = ((TrimReductions) &&
				(reduceLength == 1) && ( action.Rule.Rhs[0] is SymbolNonterminal));
			if (skipReduce)
			{
				stateStack.Pop();
				currentState = stateStack.Peek();
			}
			else
			{
				Token[] tokens = new Token[reduceLength];
				for (int i = 0; i < reduceLength; i++)
				{
					stateStack.Pop();
					tokens[reduceLength-i-1] = tokenStack.Pop();
				}
				NonterminalToken nttoken = new NonterminalToken(action.Rule,tokens);
				tokenStack.Push(nttoken);
				currentState = stateStack.Peek();

				if (OnReduce != null)
				{
					ReduceEventArgs args = new ReduceEventArgs(action.Rule,
						                                       nttoken,
						                                       currentState);
					OnReduce(this,args);
					DoReleaseTokens(args.Token);

					continueParsing = args.Continue;
				}
			}
			Action gotoAction = currentState.Actions.Get(action.Rule.Lhs);

			if (gotoAction is GotoAction)
			{
				DoGoto(token,(GotoAction)gotoAction);
			}
			else
			{
				throw new ParserException("Invalid action table in state");
			}
		}

		private void DoReleaseTokens(NonterminalToken token)
		{
			if ((StoreTokens == StoreTokensMode.Never) ||
				(StoreTokens == StoreTokensMode.NoUserObject &&
				token.UserObject != null))
			{
				token.ClearTokens();
			}
		}

		private void DoAccept(Token token, AcceptAction action)
		{
			continueParsing = false;
			accepted = true;
			if (OnAccept != null)
				OnAccept(this, new AcceptEventArgs((NonterminalToken)tokenStack.Peek()));
		}

		private void DoGoto(Token token, GotoAction action)
		{
			stateStack.Push(action.State);
			if (OnGoto != null)
			{
				OnGoto(this,new GotoEventArgs(action.Symbol,stateStack.Peek()));
			}
		}

		private void ParseTerminal(TerminalToken token)
		{
			State currentState = stateStack.Peek();

			Action action = currentState.Actions.Get(token.Symbol);

			if (action is ShiftAction)
				DoShift(token,(ShiftAction)action);
			else if (action is ReduceAction)
				DoReduce(token,(ReduceAction)action);
			else if (action is AcceptAction)
				DoAccept(token,(AcceptAction)action);
			else
			{
				continueParsing = false;
				FireParseError(token);
			}
		}

		private void FireParseError(TerminalToken token)
		{
			if (OnParseError != null)
			{
				ParseErrorEventArgs e = 
					new ParseErrorEventArgs(token, FindExpectedTokens());
				OnParseError(this, e);
				continueParsing = e.Continue;
				lookahead = e.NextToken;
			}
		}

		private void FireEOFError()
		{
			TerminalToken eofToken = new TerminalToken(SymbolCollection.EOF,
				SymbolCollection.EOF.Name,
				tokenizer.GetCurrentLocation());
			FireParseError(eofToken);
		}

		private SymbolCollection FindExpectedTokens()
		{
			SymbolCollection symbols = new SymbolCollection();
			State state = stateStack.Peek();
			foreach(Action action in state.Actions)
			{
				if ((action is ShiftAction) || (action is ReduceAction) 
					|| (action is AcceptAction))
				{
					symbols.Add(action.symbol);
				}
			}
			return symbols;
		}

		private bool SkipToEndOfLine()
		{
			bool result = tokenizer.SkipToChar('\n');
			if (! result)
			{
				FireEOFError();
			}
			return result;
		}

		private bool SkipAfterCommentEnd()
		{
			int commentDepth = 1;
			while (commentDepth > 0)
			{
				TerminalToken token = tokenizer.RetrieveToken();
				if (token.Symbol is SymbolCommentEnd)
				{
					commentDepth--;
				}
				else if (token.Symbol is SymbolEnd)
				{
					FireEOFError();
					break;
				}
			}
			return commentDepth == 0;
		}

		private TerminalToken GetLookahead()
		{
			if (lookahead != null)
			{
				return lookahead;
			}
			do
			{
				TerminalToken token = tokenizer.RetrieveToken();
				if (token.Symbol is SymbolCommentLine)
				{
					if (!ProcessCommentLine(token))
						continueParsing = false;
				}
				else if (token.Symbol is SymbolCommentStart)
				{
					if (!ProcessCommenStart(token))
						continueParsing = false;
				}
				else if (token.Symbol is SymbolWhiteSpace)
				{
					if (!ProcessWhiteSpace(token))
						continueParsing = false;
				}
				else if (token.Symbol is SymbolError)
				{
					if (!ProcessError(token))
						continueParsing = false;
				}
				else
				{
					lookahead = token;
				}
				if (!continueParsing)
					break;
			} while (lookahead == null);

			if ((lookahead != null) && (OnTokenRead != null))
			{
				TokenReadEventArgs args = new TokenReadEventArgs(lookahead);
				OnTokenRead(this, args);
				if (args.Continue == false)
				{
					continueParsing = false;
					lookahead = null;
				}
			}
			return lookahead;
		}

		private bool ProcessCommentLine(Token token)
		{
			return SkipToEndOfLine();
		}

		private bool ProcessCommenStart(Token token)
		{
			return SkipAfterCommentEnd();
		}

		private bool ProcessWhiteSpace(Token token)
		{
			return true;
		}

		private bool ProcessError(TerminalToken token)
		{
			if (OnTokenError != null)
			{
				TokenErrorEventArgs e = new TokenErrorEventArgs(token);
				OnTokenError(this, e);
				return e.Continue;
			}
			else
				return false;
		}

		/// <summary>
		/// Trim Reductions.
		/// When true there will be no reductions for single nonterminal rules,
		/// and no events for this will be generated.
		/// </summary>
		public bool TrimReductions
		{
			get {return trimReductions;} 
			set {this.trimReductions = value;}
		}

		/// <summary>
		/// This property determines if reduced tokens should be stored
		/// in a reduced token after the reduce event has occured.
		/// There are three possible values:
		/// Always means that the tokens should always be kept,
		/// NoUserObject (default) means that the tokens should only be kept if there
		/// was no user object assigned in the reduced token,
		/// Never means that the tokens are no longer available after the reduce event.
		/// </summary>
		public StoreTokensMode StoreTokens
		{
			get {return storeTokens;}
			set {storeTokens = value;}
		}

		public delegate void TokenReadHandler(LALRParser parser, TokenReadEventArgs args);
		public delegate void ShiftHandler(LALRParser parser, ShiftEventArgs args);
		public delegate	void ReduceHandler(LALRParser parser, ReduceEventArgs args);
		public delegate	void GotoHandler(LALRParser parser, GotoEventArgs args);
		public delegate void AcceptHandler(LALRParser parser, AcceptEventArgs args);
		public delegate void TokenErrorHandler(LALRParser parser, TokenErrorEventArgs args);
		public delegate void ParseErrorHandler(LALRParser parser, ParseErrorEventArgs args);

		/// <summary>
		/// This event will be called if a token has been read which will be parsed by
		/// the LALR parser.
		/// </summary>
 		public event TokenReadHandler OnTokenRead;

		/// <summary>
		/// This event will be called when a token is shifted onto the stack.
		/// </summary>
		public event ShiftHandler OnShift;

		/// <summary>
		/// This event will be called when tokens are reduced.
		/// </summary>
		public event ReduceHandler OnReduce;

		/// <summary>
		/// This event will be called when a goto occurs (after a reduction).
		/// </summary>
		public event GotoHandler OnGoto;

		/// <summary>
		/// This event will be called if the parser is finished and the input has been
		/// accepted.
		/// </summary>
		public event AcceptHandler OnAccept;

		/// <summary>
		/// This event will be called when the tokenizer cannot recognize the input.
		/// </summary>
		public event TokenErrorHandler OnTokenError;

		/// <summary>
		/// This event will be called when the parser has a token it cannot parse.
		/// </summary>
		public event ParseErrorHandler OnParseError;

	}
}
