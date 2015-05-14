using System;
using System.Text;
//using com.calitha.goldparser;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using GoldParser;


namespace PascalABCCompiler.KuMirParser
{
    public static class StringResources
    {
        private static string prefix = "KUMIRPARSER_";
        public static string Get(string Id)
        {
            string ret = PascalABCCompiler.StringResources.Get(prefix + Id);
            if (ret == prefix + Id)
                return Id;
            else
                return ret;
        }
    }

    public class KuMir_parsertools : parser_tools
    {
        public override string symbol_to_string(Symbol symbol)
        {
            switch (symbol.Index)
            {
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_MINUS:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_DIV:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_MULT:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_GREATER:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_GREATEREQUAL:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_LOWER:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_LOWEREQUAL:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_NOTEQUAL:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_POWER:
                    return StringResources.Get("OPERATOR");
            }
            string res = StringResources.Get(symbol.Name);
            /*if (res.IndexOf("TK_") == 0)
                return res.Remove(0, 3).ToLower();
            return res;*/            
            
            return res;
        }

        public override int symbol_priority(Symbol symbol)
        {

            switch (symbol.Index)
            {
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_END:
                    return 8;
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_BEGIN:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_INTEGER:
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_ROUNDCLOSE:
                    return 9;
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_IDENTIFIER:
                    return 10;

                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_ASSIGN:
                    return 25;

                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_EQUAL:
                    return 25;

                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_EXPRESSION:
                    return 100;
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_STATEMENT:
                    return 110;
                case (int)GPBParser_KuMir.SymbolConstants.SYMBOL_TK_SEMICOLON:
                    return 1000;
            }
            if (symbol.SymbolType == SymbolType.Terminal)
                return 1;
            return 0;
        }


    }
}
