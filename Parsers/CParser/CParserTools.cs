using System;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using GoldParser;
using PascalABCCompiler.CParser.Errors;


namespace PascalABCCompiler.CParser
{
    public static class StringResources
    {
        private static string prefix = "CPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }


	public class C_parsertools:parser_tools
	{
        internal SourceContextMap scm=null;
        public override SourceContext GetTokenSourceContext()
        {
            return GetTokenSourceContext(parser);
        }
        int NewLineLength = Environment.NewLine.Length;
        int GetLinesCount(string s)
        {
            //Íå ó÷èòûâàåò ïîñëåäíèé ïåðåâîä åñëè çà íèì íè÷åãî
            int i=s.IndexOf(Environment.NewLine);
            int res = 0;
            if (i >= 0 && i < s.Length - NewLineLength)
                res++;
            while (i >= 0)
            {
                i = s.IndexOf(Environment.NewLine, i + NewLineLength);
                if (i>=0 && i<s.Length-NewLineLength) 
                    res++;
            }
            return res;
        }
        public override SourceContext GetTokenSourceContext(Parser parser)
        {
            int LineNum = parser.TokenLineNumber + LineCorrection;
            SourceContext sc = new SourceContext(LineNum, parser.TokenLinePosition, LineNum + GetLinesCount(parser.TokenText), parser.TokenLinePosition + parser.TokenLength - 1, parser.TokenCharPosition, parser.TokenCharPosition + parser.TokenLength);
            if (scm != null)
                sc = scm.GetSourceContext(sc);
            return sc;
        }
        public override string ReplaceSpecialSymbols(string text)
        {
            text = text.Replace(@"\n", '\n'.ToString());
            text = text.Replace(@"\r", '\r'.ToString());
            text = text.Replace(@"\t", '\t'.ToString());
            text = text.Replace(@"\'", '\''.ToString());
            text = text.Replace(@"\"+'"', '"'.ToString());
            return text;
        }

        public override char_const create_char_const(GPBParser parser)
        {
            char_const ch = base.create_char_const(parser);
            if (ch == null)
            {
                ch = new char_const();
                ch.source_context = GetTokenSourceContext(parser.LRParser);
                parser.errors.Add(new BadCharConst(parser.current_file_name, ch.source_context, ch));
            }
            return ch;
        }
        
        public override string symbol_to_string(Symbol symbol)
		{
			/*switch (symbol.Id)
			{
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_EOF :
					return "(ÊÎÍÅÖ ÔÀÉËÀ)";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKIDENTIFIER :
					return "ÈÄÅÍÒÅÔÈÊÀÒÎÐ";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKEND:
					return "END";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKBEGIN:
					return "BEGIN";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKNIL:
					return "NIL";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKSEMICOLON:
					return "';'";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKCOLON:
					return "':'";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKPOINT:
					return "'.'";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKASSIGN:
					return "':='";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKINTEGER:
					return "×ÈÑËÎ";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKEQUAL:
					return "'='";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKROUNDOPEN:
					return "'('";
				case (int)GPBParser_Oberon2.SymbolConstants.SYMBOL_TKROUNDCLOSE:
					return "')'";
			}*/
            string res = StringResources.Get(symbol.Name.ToUpper());
            if (res.IndexOf("TK") == 0)
                return res.Remove(0, 2).ToLower();
            return res;
		}
		public override int symbol_priority(Symbol symbol)
		{
			switch (symbol.Index)
			{
				case (int)GPBParser_C.SymbolConstants.SYMBOL_SEMICOLON:
					return 1;
                case (int)GPBParser_C.SymbolConstants.SYMBOL_IDENTIFIER:
                    return 10;
                case (int)GPBParser_C.SymbolConstants.SYMBOL_DECLARATION_SPECIFIERS:
					return 100;
				
			}
			return 0;
		}

	}
}
