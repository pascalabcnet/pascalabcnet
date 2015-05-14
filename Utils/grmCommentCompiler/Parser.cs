
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using com.calitha.goldparser.lalr;
using com.calitha.commons;


namespace com.calitha.goldparser
{

    [Serializable()]
	public class StringDictionary : DictionaryBase  
	{

		public String this[ String key ]  
		{
			get  
			{
				return( (String) Dictionary[key] );
			}
			set  
			{
				Dictionary[key] = value;
			}
		}

		public ICollection Keys  
		{
			get  
			{
				return( Dictionary.Keys );
			}
		}

		public ICollection Values  
		{
			get  
			{
				return( Dictionary.Values );
			}
		}

		public void Add( String key, String value )  
		{
			Dictionary.Add( key, value );
		}

		public bool Contains( String key )  
		{
			return( Dictionary.Contains( key ) );
		}

		public void Remove( String key )  
		{
			Dictionary.Remove( key );
		}
	}
	public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF            = 0  , // (EOF)
        SYMBOL_ERROR          = 1  , // (Error)
        SYMBOL_WHITESPACE     = 2  , // (Whitespace)
        SYMBOL_COMMENTLINE    = 3  , // (Comment Line)
        SYMBOL_MINUS          = 4  , // '-'
        SYMBOL_LPARAN         = 5  , // '('
        SYMBOL_RPARAN         = 6  , // ')'
        SYMBOL_TIMES          = 7  , // '*'
        SYMBOL_COLONCOLONEQ   = 8  , // '::='
        SYMBOL_QUESTION       = 9  , // '?'
        SYMBOL_PIPE           = 10 , // '|'
        SYMBOL_PLUS           = 11 , // '+'
        SYMBOL_EQ             = 12 , // '='
        SYMBOL_LARGECOMMENT   = 13 , // LargeComment
        SYMBOL_NEWLINE        = 14 , // Newline
        SYMBOL_NONTERMINAL    = 15 , // Nonterminal
        SYMBOL_PARAMETERNAME  = 16 , // ParameterName
        SYMBOL_SETLITERAL     = 17 , // SetLiteral
        SYMBOL_SETNAME        = 18 , // SetName
        SYMBOL_TERMINAL       = 19 , // Terminal
        SYMBOL_CONTENT        = 20 , // <Content>
        SYMBOL_DEFINITION     = 21 , // <Definition>
        SYMBOL_GRAMMAR        = 22 , // <Grammar>
        SYMBOL_HANDLE         = 23 , // <Handle>
        SYMBOL_HANDLES        = 24 , // <Handles>
        SYMBOL_KLEENEOPT      = 25 , // <Kleene Opt>
        SYMBOL_LARGE_COMMENT  = 26 , // <Large_Comment>
        SYMBOL_NL             = 27 , // <nl>
        SYMBOL_NLOPT          = 28 , // <nl opt>
        SYMBOL_PARAMETER      = 29 , // <Parameter>
        SYMBOL_PARAMETERBODY  = 30 , // <Parameter Body>
        SYMBOL_PARAMETERITEM  = 31 , // <Parameter Item>
        SYMBOL_PARAMETERITEMS = 32 , // <Parameter Items>
        SYMBOL_REGEXP         = 33 , // <Reg Exp>
        SYMBOL_REGEXP2        = 34 , // <Reg Exp 2>
        SYMBOL_REGEXPITEM     = 35 , // <Reg Exp Item>
        SYMBOL_REGEXPSEQ      = 36 , // <Reg Exp Seq>
        SYMBOL_RULEDECL       = 37 , // <Rule Decl>
        SYMBOL_SETDECL        = 38 , // <Set Decl>
        SYMBOL_SETEXP         = 39 , // <Set Exp>
        SYMBOL_SETITEM        = 40 , // <Set Item>
        SYMBOL_SYMBOL         = 41 , // <Symbol>
        SYMBOL_TERMINALDECL   = 42 , // <Terminal Decl>
        SYMBOL_TERMINALNAME   = 43   // <Terminal Name>
    };

    enum RuleConstants : int
    {
        RULE_GRAMMAR                           = 0  , // <Grammar> ::= <nl opt> <Content>
        RULE_CONTENT                           = 1  , // <Content> ::= <Content> <Definition>
        RULE_CONTENT2                          = 2  , // <Content> ::= <Definition>
        RULE_DEFINITION                        = 3  , // <Definition> ::= <Parameter>
        RULE_DEFINITION2                       = 4  , // <Definition> ::= <Set Decl>
        RULE_DEFINITION3                       = 5  , // <Definition> ::= <Terminal Decl>
        RULE_DEFINITION4                       = 6  , // <Definition> ::= <Rule Decl>
        RULE_DEFINITION5                       = 7  , // <Definition> ::= <Large_Comment>
        RULE_NLOPT_NEWLINE                     = 8  , // <nl opt> ::= Newline <nl opt>
        RULE_NLOPT                             = 9  , // <nl opt> ::= 
        RULE_NL_NEWLINE                        = 10 , // <nl> ::= Newline <nl>
        RULE_NL_NEWLINE2                       = 11 , // <nl> ::= Newline
        RULE_LARGE_COMMENT_LARGECOMMENT        = 12 , // <Large_Comment> ::= LargeComment
        RULE_PARAMETER_PARAMETERNAME_EQ        = 13 , // <Parameter> ::= ParameterName <nl opt> '=' <Parameter Body> <nl>
        RULE_PARAMETERBODY_PIPE                = 14 , // <Parameter Body> ::= <Parameter Body> <nl opt> '|' <Parameter Items>
        RULE_PARAMETERBODY                     = 15 , // <Parameter Body> ::= <Parameter Items>
        RULE_PARAMETERITEMS                    = 16 , // <Parameter Items> ::= <Parameter Items> <Parameter Item>
        RULE_PARAMETERITEMS2                   = 17 , // <Parameter Items> ::= <Parameter Item>
        RULE_PARAMETERITEM_PARAMETERNAME       = 18 , // <Parameter Item> ::= ParameterName
        RULE_PARAMETERITEM_TERMINAL            = 19 , // <Parameter Item> ::= Terminal
        RULE_PARAMETERITEM_SETLITERAL          = 20 , // <Parameter Item> ::= SetLiteral
        RULE_PARAMETERITEM_SETNAME             = 21 , // <Parameter Item> ::= SetName
        RULE_PARAMETERITEM_NONTERMINAL         = 22 , // <Parameter Item> ::= Nonterminal
        RULE_SETDECL_SETNAME_EQ                = 23 , // <Set Decl> ::= SetName <nl opt> '=' <Set Exp> <nl>
        RULE_SETEXP_PLUS                       = 24 , // <Set Exp> ::= <Set Exp> <nl opt> '+' <Set Item>
        RULE_SETEXP_MINUS                      = 25 , // <Set Exp> ::= <Set Exp> <nl opt> '-' <Set Item>
        RULE_SETEXP                            = 26 , // <Set Exp> ::= <Set Item>
        RULE_SETITEM_SETLITERAL                = 27 , // <Set Item> ::= SetLiteral
        RULE_SETITEM_SETNAME                   = 28 , // <Set Item> ::= SetName
        RULE_TERMINALDECL_EQ                   = 29 , // <Terminal Decl> ::= <Terminal Name> <nl opt> '=' <Reg Exp> <nl>
        RULE_TERMINALDECL_EQ2                  = 30 , // <Terminal Decl> ::= <Terminal Name> <nl opt> '=' <Reg Exp> <Large_Comment> <nl>
        RULE_TERMINALNAME_TERMINAL             = 31 , // <Terminal Name> ::= <Terminal Name> Terminal
        RULE_TERMINALNAME_TERMINAL2            = 32 , // <Terminal Name> ::= Terminal
        RULE_REGEXP_PIPE                       = 33 , // <Reg Exp> ::= <Reg Exp> <nl opt> '|' <Reg Exp Seq>
        RULE_REGEXP                            = 34 , // <Reg Exp> ::= <Reg Exp Seq>
        RULE_REGEXPSEQ                         = 35 , // <Reg Exp Seq> ::= <Reg Exp Seq> <Reg Exp Item>
        RULE_REGEXPSEQ2                        = 36 , // <Reg Exp Seq> ::= <Reg Exp Item>
        RULE_REGEXPITEM_SETLITERAL             = 37 , // <Reg Exp Item> ::= SetLiteral <Kleene Opt>
        RULE_REGEXPITEM_SETNAME                = 38 , // <Reg Exp Item> ::= SetName <Kleene Opt>
        RULE_REGEXPITEM_TERMINAL               = 39 , // <Reg Exp Item> ::= Terminal <Kleene Opt>
        RULE_REGEXPITEM_LPARAN_RPARAN          = 40 , // <Reg Exp Item> ::= '(' <Reg Exp 2> ')' <Kleene Opt>
        RULE_REGEXP2_PIPE                      = 41 , // <Reg Exp 2> ::= <Reg Exp 2> '|' <Reg Exp Seq>
        RULE_REGEXP2                           = 42 , // <Reg Exp 2> ::= <Reg Exp Seq>
        RULE_KLEENEOPT_PLUS                    = 43 , // <Kleene Opt> ::= '+'
        RULE_KLEENEOPT_QUESTION                = 44 , // <Kleene Opt> ::= '?'
        RULE_KLEENEOPT_TIMES                   = 45 , // <Kleene Opt> ::= '*'
        RULE_KLEENEOPT                         = 46 , // <Kleene Opt> ::= 
        RULE_RULEDECL_NONTERMINAL_COLONCOLONEQ = 47 , // <Rule Decl> ::= Nonterminal <nl opt> '::=' <Handles> <nl>
        RULE_HANDLES_PIPE                      = 48 , // <Handles> ::= <Handles> <nl opt> '|' <Handle>
        RULE_HANDLES                           = 49 , // <Handles> ::= <Handle>
        RULE_HANDLE                            = 50 , // <Handle> ::= <Handle> <Symbol>
        RULE_HANDLE2                           = 51 , // <Handle> ::= <Handle> <Symbol> <Large_Comment>
        RULE_HANDLE3                           = 52 , // <Handle> ::= 
        RULE_SYMBOL_TERMINAL                   = 53 , // <Symbol> ::= Terminal
        RULE_SYMBOL_NONTERMINAL                = 54   // <Symbol> ::= Nonterminal
    };

    public class MyParser
    {
        public LALRParser parser;
        private ShiftEventArgs LastShiftEventArgs;
		private ReduceEventArgs LastReduceEventArgs;
		private GotoEventArgs LastGotoEventArgs;
		
		public StringDictionary TerminalsDefs;
		public string Comments; 
        public MyParser(string filename)
        {
            FileStream stream = new FileStream(filename,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

			TerminalsDefs=new StringDictionary();
			Comments="";

            parser.OnReduce += new LALRParser.ReduceHandler(ReduceEvent);
            parser.OnTokenRead += new LALRParser.TokenReadHandler(TokenReadEvent);
            parser.OnAccept += new LALRParser.AcceptHandler(AcceptEvent);
            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

	public NonterminalToken Parse(string source)
        {
		return parser.Parse(source);	
        }

        private void TokenReadEvent(LALRParser parser, TokenReadEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }

        private Object CreateObject(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //(Whitespace)
                return null;

                case (int)SymbolConstants.SYMBOL_COMMENTLINE :
                //(Comment Line)
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                return null;

                case (int)SymbolConstants.SYMBOL_LPARAN :
                //'('
                return null;

                case (int)SymbolConstants.SYMBOL_RPARAN :
                //')'
                return null;

                case (int)SymbolConstants.SYMBOL_TIMES :
                //'*'
                return null;

                case (int)SymbolConstants.SYMBOL_COLONCOLONEQ :
                //'::='
                return null;

                case (int)SymbolConstants.SYMBOL_QUESTION :
                //'?'
                return null;

                case (int)SymbolConstants.SYMBOL_PIPE :
                //'|'
                return null;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                return null;

                case (int)SymbolConstants.SYMBOL_LARGECOMMENT :
                //LargeComment
                return token.Text;

                case (int)SymbolConstants.SYMBOL_NEWLINE :
                //Newline
                return null;

                case (int)SymbolConstants.SYMBOL_NONTERMINAL :
                //Nonterminal
                return token.Text;

                case (int)SymbolConstants.SYMBOL_PARAMETERNAME :
                //ParameterName
                return null;

                case (int)SymbolConstants.SYMBOL_SETLITERAL :
                //SetLiteral
                return null;

                case (int)SymbolConstants.SYMBOL_SETNAME :
                //SetName
                return null;

                case (int)SymbolConstants.SYMBOL_TERMINAL :
                //Terminal
                return token.Text;

                case (int)SymbolConstants.SYMBOL_CONTENT :
                //<Content>
                return null;

                case (int)SymbolConstants.SYMBOL_DEFINITION :
                //<Definition>
                return null;

                case (int)SymbolConstants.SYMBOL_GRAMMAR :
                //<Grammar>
                return null;

                case (int)SymbolConstants.SYMBOL_HANDLE :
                //<Handle>
                return null;

                case (int)SymbolConstants.SYMBOL_HANDLES :
                //<Handles>
                return null;

                case (int)SymbolConstants.SYMBOL_KLEENEOPT :
                //<Kleene Opt>
                return null;

                case (int)SymbolConstants.SYMBOL_LARGE_COMMENT :
                //<Large_Comment>
                return null;

                case (int)SymbolConstants.SYMBOL_NL :
                //<nl>
                return null;

                case (int)SymbolConstants.SYMBOL_NLOPT :
                //<nl opt>
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMETER :
                //<Parameter>
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMETERBODY :
                //<Parameter Body>
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMETERITEM :
                //<Parameter Item>
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMETERITEMS :
                //<Parameter Items>
                return null;

                case (int)SymbolConstants.SYMBOL_REGEXP :
                //<Reg Exp>
                return null;

                case (int)SymbolConstants.SYMBOL_REGEXP2 :
                //<Reg Exp 2>
                return null;

                case (int)SymbolConstants.SYMBOL_REGEXPITEM :
                //<Reg Exp Item>
                return null;

                case (int)SymbolConstants.SYMBOL_REGEXPSEQ :
                //<Reg Exp Seq>
                return null;

                case (int)SymbolConstants.SYMBOL_RULEDECL :
                //<Rule Decl>
                return null;

                case (int)SymbolConstants.SYMBOL_SETDECL :
                //<Set Decl>
                return null;

                case (int)SymbolConstants.SYMBOL_SETEXP :
                //<Set Exp>
                return null;

                case (int)SymbolConstants.SYMBOL_SETITEM :
                //<Set Item>
                return null;

                case (int)SymbolConstants.SYMBOL_SYMBOL :
                //<Symbol>
                return null;

                case (int)SymbolConstants.SYMBOL_TERMINALDECL :
                //<Terminal Decl>
                return null;

                case (int)SymbolConstants.SYMBOL_TERMINALNAME :
                //<Terminal Name>
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        private void ReduceEvent(LALRParser parser, ReduceEventArgs args)
        {
            try
            {
                args.Token.UserObject = CreateObject(args.Token);
            }
            catch (Exception e)
            {
                args.Continue = false;
                //todo: Report message to UI?
            }
        }
		public void MakeNonTerminal(string part1,string part2)
		{
			string str=part1+" ::="+part2;
			if (str.IndexOf("!*")==-1) 
			{
				string s=part2;			
				int l=(s.Split(" ".ToCharArray())).Length-1;
				if (l!=1){
					//debug
                    TerminalsDefs.Add(str, " {errors.Add(new nonterminal_token_return_null(current_file_name,parsertools.GetTokenSourceContext(),(syntax_tree_node)prev_node,\"" + str + "\"));}return null;"); 
					return;
				}
				//для праил типа <rule> ::= term или <rule> ::= <rule2> добавляем $$=$1
				TerminalsDefs.Add(str,"$$=$1;");
				return;
			}
			string grmname=str.Substring(0,str.IndexOf("!*"));
			string cstext=str.Substring(str.IndexOf("!*")+2,str.Length-str.IndexOf("!*")-4);
			//Console.WriteLine(grmname+"_"+cstext);
			TerminalsDefs.Add(grmname,cstext);
		}
        public Object CreateObject(NonterminalToken token)
        {
			string grmname,cstext,str;
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_GRAMMAR :
                //<Grammar> ::= <nl opt> <Content>
                return null;

                case (int)RuleConstants.RULE_CONTENT :
                //<Content> ::= <Content> <Definition>
                return null;

                case (int)RuleConstants.RULE_CONTENT2 :
                //<Content> ::= <Definition>
                return null;

                case (int)RuleConstants.RULE_DEFINITION :
                //<Definition> ::= <Parameter>
                return null;

                case (int)RuleConstants.RULE_DEFINITION2 :
                //<Definition> ::= <Set Decl>
                return null;

                case (int)RuleConstants.RULE_DEFINITION3 :
                //<Definition> ::= <Terminal Decl>
                return null;

                case (int)RuleConstants.RULE_DEFINITION4 :
                //<Definition> ::= <Rule Decl>
                return null;

                case (int)RuleConstants.RULE_DEFINITION5 :
                //<Definition> ::= <Large_Comment>
				Comments+=(string)token.Tokens[0].UserObject;
				//Console.WriteLine(str);
                return null;

                case (int)RuleConstants.RULE_NLOPT_NEWLINE :
                //<nl opt> ::= Newline <nl opt>
                return null;

                case (int)RuleConstants.RULE_NLOPT :
                //<nl opt> ::= 
                return null;

                case (int)RuleConstants.RULE_NL_NEWLINE :
                //<nl> ::= Newline <nl>
                return null;

                case (int)RuleConstants.RULE_NL_NEWLINE2 :
                //<nl> ::= Newline
                return null;

                case (int)RuleConstants.RULE_LARGE_COMMENT_LARGECOMMENT :
                //<Large_Comment> ::= LargeComment
                return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_PARAMETER_PARAMETERNAME_EQ :
                //<Parameter> ::= ParameterName <nl opt> '=' <Parameter Body> <nl>
                return null;

                case (int)RuleConstants.RULE_PARAMETERBODY_PIPE :
                //<Parameter Body> ::= <Parameter Body> <nl opt> '|' <Parameter Items>
                return null;

                case (int)RuleConstants.RULE_PARAMETERBODY :
                //<Parameter Body> ::= <Parameter Items>
                return null;

                case (int)RuleConstants.RULE_PARAMETERITEMS :
                //<Parameter Items> ::= <Parameter Items> <Parameter Item>
                return null;

                case (int)RuleConstants.RULE_PARAMETERITEMS2 :
                //<Parameter Items> ::= <Parameter Item>
                return null;

                case (int)RuleConstants.RULE_PARAMETERITEM_PARAMETERNAME :
                //<Parameter Item> ::= ParameterName
                return null;

                case (int)RuleConstants.RULE_PARAMETERITEM_TERMINAL :
                //<Parameter Item> ::= Terminal
                return null;

                case (int)RuleConstants.RULE_PARAMETERITEM_SETLITERAL :
                //<Parameter Item> ::= SetLiteral
                return null;

                case (int)RuleConstants.RULE_PARAMETERITEM_SETNAME :
                //<Parameter Item> ::= SetName
                return null;

                case (int)RuleConstants.RULE_PARAMETERITEM_NONTERMINAL :
                //<Parameter Item> ::= Nonterminal
                return null;

                case (int)RuleConstants.RULE_SETDECL_SETNAME_EQ :
                //<Set Decl> ::= SetName <nl opt> '=' <Set Exp> <nl>
                return null;

                case (int)RuleConstants.RULE_SETEXP_PLUS :
                //<Set Exp> ::= <Set Exp> <nl opt> '+' <Set Item>
                return null;

                case (int)RuleConstants.RULE_SETEXP_MINUS :
                //<Set Exp> ::= <Set Exp> <nl opt> '-' <Set Item>
                return null;

                case (int)RuleConstants.RULE_SETEXP :
                //<Set Exp> ::= <Set Item>
                return null;

                case (int)RuleConstants.RULE_SETITEM_SETLITERAL :
                //<Set Item> ::= SetLiteral
                return null;

                case (int)RuleConstants.RULE_SETITEM_SETNAME :
                //<Set Item> ::= SetName
                return null;

                case (int)RuleConstants.RULE_TERMINALDECL_EQ :
                //<Terminal Decl> ::= <Terminal Name> <nl opt> '=' <Reg Exp> <nl>
					str=(string)token.Tokens[0].UserObject;
					//debug
                    TerminalsDefs.Add(str, " /*Console.Error.WriteLine(\"\\n\\rwarning: TerminalToken '" + str + "' return NULL! \\n\\r\");*/return null;"); 
                return null;

                case (int)RuleConstants.RULE_TERMINALDECL_EQ2 :
                //<Terminal Decl> ::= <Terminal Name> <nl opt> '=' <Reg Exp> <Large_Comment> <nl>
				//string termdecl=(string)token.Tokens[0].UserObject+(string)token.Tokens[4].UserObject;
				//Console.WriteLine(termdecl)
					str=(string)token.Tokens[4].UserObject;
					grmname=(string)token.Tokens[0].UserObject;
					cstext=str.Substring(str.IndexOf("!*")+2,str.IndexOf("*!")-2);
					//Console.WriteLine(grmname+"_"+cstext);
					TerminalsDefs.Add(grmname,cstext);
                return null;

                case (int)RuleConstants.RULE_TERMINALNAME_TERMINAL :
                //<Terminal Name> ::= <Terminal Name> Terminal
                return (string)token.Tokens[0].UserObject+" "+(string)token.Tokens[1].UserObject;

                case (int)RuleConstants.RULE_TERMINALNAME_TERMINAL2 :
                //<Terminal Name> ::= Terminal
                return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_REGEXP_PIPE :
                //<Reg Exp> ::= <Reg Exp> <nl opt> '|' <Reg Exp Seq>
                return null;

                case (int)RuleConstants.RULE_REGEXP :
                //<Reg Exp> ::= <Reg Exp Seq>
                return null;

                case (int)RuleConstants.RULE_REGEXPSEQ :
                //<Reg Exp Seq> ::= <Reg Exp Seq> <Reg Exp Item>
                return null;

                case (int)RuleConstants.RULE_REGEXPSEQ2 :
                //<Reg Exp Seq> ::= <Reg Exp Item>
                return null;

                case (int)RuleConstants.RULE_REGEXPITEM_SETLITERAL :
                //<Reg Exp Item> ::= SetLiteral <Kleene Opt>
                return null;

                case (int)RuleConstants.RULE_REGEXPITEM_SETNAME :
                //<Reg Exp Item> ::= SetName <Kleene Opt>
                return null;

                case (int)RuleConstants.RULE_REGEXPITEM_TERMINAL :
                //<Reg Exp Item> ::= Terminal <Kleene Opt>
                return null;

                case (int)RuleConstants.RULE_REGEXPITEM_LPARAN_RPARAN :
                //<Reg Exp Item> ::= '(' <Reg Exp 2> ')' <Kleene Opt>
                return null;

                case (int)RuleConstants.RULE_REGEXP2_PIPE :
                //<Reg Exp 2> ::= <Reg Exp 2> '|' <Reg Exp Seq>
                return null;

                case (int)RuleConstants.RULE_REGEXP2 :
                //<Reg Exp 2> ::= <Reg Exp Seq>
                return null;

                case (int)RuleConstants.RULE_KLEENEOPT_PLUS :
                //<Kleene Opt> ::= '+'
                return null;

                case (int)RuleConstants.RULE_KLEENEOPT_QUESTION :
                //<Kleene Opt> ::= '?'
                return null;

                case (int)RuleConstants.RULE_KLEENEOPT_TIMES :
                //<Kleene Opt> ::= '*'
                return null;

                case (int)RuleConstants.RULE_KLEENEOPT :
                //<Kleene Opt> ::= 
                return null;

                case (int)RuleConstants.RULE_RULEDECL_NONTERMINAL_COLONCOLONEQ :
                //<Rule Decl> ::= Nonterminal <nl opt> '::=' <Handles> <nl>
					string nonterm=(string)token.Tokens[0].UserObject;
					string handles=(string)token.Tokens[3].UserObject;
					string[] parts=handles.Split("Ё".ToCharArray());
					foreach (string s in parts) 
						MakeNonTerminal(nonterm,s);
				return null;

                case (int)RuleConstants.RULE_HANDLES_PIPE :
                //<Handles> ::= <Handles> <nl opt> '|' <Handle>
                return (string)token.Tokens[0].UserObject+"Ё"+(string)token.Tokens[3].UserObject;

                case (int)RuleConstants.RULE_HANDLES :
                //<Handles> ::= <Handle>
                return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_HANDLE :
                //<Handle> ::= <Handle> <Symbol>
                return (string)token.Tokens[0].UserObject+" "+(string)token.Tokens[1].UserObject;

                case (int)RuleConstants.RULE_HANDLE2 :
                //<Handle> ::= <Handle> <Symbol> <Large_Comment>
                return (string)token.Tokens[0].UserObject+" "+(string)token.Tokens[1].UserObject+(string)token.Tokens[2].UserObject;

                case (int)RuleConstants.RULE_HANDLE3 :
                //<Handle> ::= 
                return "";

                case (int)RuleConstants.RULE_SYMBOL_TERMINAL :
                //<Symbol> ::= Terminal
                return token.Tokens[0].UserObject;

                case (int)RuleConstants.RULE_SYMBOL_NONTERMINAL :
                //<Symbol> ::= Nonterminal
                return token.Tokens[0].UserObject;

            }
            throw new RuleException("Unknown rule");
        }

        private void AcceptEvent(LALRParser parser, AcceptEventArgs args)
        {
            //todo: Use your fully reduced args.Token.UserObject
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = args.Token.Location.ToString()+"[\n\r";
			message+="Token error with input: '"+args.Token.ToString()+"'";
			if (LastGotoEventArgs!=null) message+="\n\rLastGoto:"+LastGotoEventArgs.Symbol.ToString();
			if (LastReduceEventArgs!=null) message+="\n\rLastReduce.Rule:"+LastReduceEventArgs.Rule.ToString();			            
			message+="\n\r]";
			Console.WriteLine(message);
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {   
            
			string message = args.UnexpectedToken.Location.ToString()+"[\n\r"; 
			message+="Unexpected Token: '"+args.UnexpectedToken.ToString()+"'\n\rExpected: '"+args.ExpectedTokens.ToString()+"'";
            if (LastGotoEventArgs!=null) message+="\n\rLastGoto:"+LastGotoEventArgs.Symbol.ToString();
			if (LastReduceEventArgs!=null) message+="\n\rLastReduce.Rule:"+LastReduceEventArgs.Rule.ToString();			
			message+="\n\r]";
			Console.WriteLine(message);
        }                

    }
}
