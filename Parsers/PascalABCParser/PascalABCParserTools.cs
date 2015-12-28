using System;
using System.Text;
//using com.calitha.goldparser;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using GoldParser;


namespace PascalABCCompiler.PascalABCParser
{
    public static class StringResources
    {
        private static string prefix = "PASCALABCPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }

    public class pascalabc_parsertools : parser_tools
    {
        public SourceContextMap scm = null;
        public override SourceContext GetTokenSourceContext()
        {
            return GetTokenSourceContext(parser);
        }
        int NewLineLength = Environment.NewLine.Length;
        int GetLinesCount(string s)
        {
            //Не учитывает последний перевод если за ним ничего
            int i = s.IndexOf(Environment.NewLine);
            int res = 0;
            if (i >= 0 && i < s.Length - NewLineLength)
                res++;
            while (i >= 0)
            {
                i = s.IndexOf(Environment.NewLine, i + NewLineLength);
                if (i >= 0 && i < s.Length - NewLineLength)
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

        public compiler_directive MakeDirective(token_info ti)
        {
            compiler_directive cd = new compiler_directive();
            cd.Directive = new token_info();
            string name = ti.text.Remove(0, 2).Replace("}","");
            int i = name.IndexOf(' ');
            if (i > 0)
            {
                cd.Directive = new token_info(name.Remove(0, i + 1));
                cd.Directive.source_context = ti.source_context;
                name = name.Remove(i);
                cd.Directive.text = cd.Directive.text.Trim(' ');
                cd.Directive.text = cd.Directive.text.Replace(@"'", "".ToString());
            }
            
            cd.Name = new token_info(name);
            cd.Name.source_context = ti.source_context;
            cd.source_context = ti.source_context;
            return cd;
        }

        public override string symbol_to_string(Symbol symbol)
        {
            switch (symbol.Index)
            {
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKMINUS:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKOR:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKXOR:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKAND:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKDIV:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKMOD:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSHL:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSHR:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSLASH:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSTAR:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKGREATER:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKGREATEREQUAL:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKIN:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKLOWER:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKLOWEREQUAL:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKNOTEQUAL:
                    return StringResources.Get("OPERATOR");
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSQUARECLOSE:
                    return "]";
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSQUAREOPEN:
                    return "[";
            }
            string res = StringResources.Get(symbol.Name.ToUpper());
            if (res.IndexOf("TK") == 0)
                return res.Remove(0, 2).ToLower();
            return res;
        }

        public override int symbol_priority(Symbol symbol)
        {

            switch (symbol.Index)
            {
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKEND:
                    return 8;
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKBEGIN:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKINTEGER:
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKROUNDCLOSE:
                    return 9;
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKIDENTIFIER:
                    return 10;

                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKASSIGN:
                    return 25;

                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKCOLON:
                    return 20;

                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKEQUAL:
                    return 25;

                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKSEMICOLON:
                    return 30;

                //                 case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKDO:
                //                     return 50;
                //case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_TKPOINT:
                //  return 35;

                //	return 5;
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_EXPR:
                    return 100;
                case (int)GPBParser_PascalABC.SymbolConstants.SYMBOL_STMT:
                    return 110;

            }
            if (symbol.SymbolType == SymbolType.Terminal)
                return 1;
            return 0;
        }

        /*public override void check_comment_text(GPBParser parser)
        {
            string original_text = parser.LRParser.TokenString;
            if (original_text[0] == '{' && original_text[1] == '$')
            {
                string text = original_text.Substring(2, original_text.Length - 3);
                int delim_pos = text.IndexOf(' ');
                if (delim_pos > 1)
                {
                    string dname = text.Substring(0, delim_pos - 1);
                    string derictive = text.Substring(delim_pos, text.Length - delim_pos - 1);
                    compiler_directive cd = new compiler_directive(dname, derictive);
                    cd.source_context = GetTokenSourceContext(parser.LRParser);
                    parser.CompilerDirectives.Add(cd);
                }
            }
        }*/


    }
}
