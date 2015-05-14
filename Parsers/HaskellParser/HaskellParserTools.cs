using System;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using GoldParser;

namespace PascalABCCompiler.HaskellParser
{
    public static class StringResources
    {
        private static string prefix = "HaskellPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }

    public class Haskell_parsertools : parser_tools
    {
        public override string symbol_to_string(Symbol symbol)
        {
            /*switch (symbol.Index)
            {
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKMINUS:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKSLASH:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKSTAR:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKGREATER:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKGREATEREQUAL:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKLOWER:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKLOWEREQUAL:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKNOTEQUAL:
                    return StringResources.Get("OPERATOR");
            }*/
            string res = StringResources.Get(symbol.Name.ToUpper());
            if (res.IndexOf("TK") == 0)
                return res.Remove(0, 2).ToLower();
            return res;
        }

        public override int symbol_priority(Symbol symbol)
        {
            /*switch (symbol.Index)
            {
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKEND:
                    return 8;
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKBEGIN:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKNUMBER:
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKROUNDCLOSE:
                    return 9;
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKIDENT:
                    return 10;

                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKASSIGN:
                    return 25;

                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKEQUAL:
                    return 25;

                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_TKSEMICOLON:
                    return 30;

                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_EXPRESSION:
                    return 100;
                case (int)GPBParser_PL0.SymbolConstants.SYMBOL_STATEMENT:
                    return 110;

            }*/
            if (symbol.SymbolType == SymbolType.Terminal)
                return 1;
            return 0;
        }
    }
}
