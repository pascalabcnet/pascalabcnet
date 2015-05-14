using System;
using System.Text;
//using com.calitha.goldparser;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TestParserTools;
using PascalABCCompiler.TestGoldParserEngine;


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

    public class pascalabc_parsertools:parser_tools
    {

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

       


    }
}
