using System;
//using com.calitha.goldparser;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;
using System.Collections.Generic;
using PascalABCCompiler.KuMirParser;


namespace PascalABCCompiler.KuMirParser.Errors
{
    public class nonterminal_token_return_null : SyntaxError
    {
        public nonterminal_token_return_null(string _file_name, SourceContext _source_context, syntax_tree_node _node, string nonterminal_token_name)
            :
            base(string.Format(StringResources.Get("NONTERMINALTOKEN_{0}_RETURN_NULL"), nonterminal_token_name),
                _file_name, _source_context, _node)
        {
        }
    }
    
    public class UnexpectedToken : SyntaxError
    {
        private string _message;
        public UnexpectedToken(PascalABCCompiler.ParserTools.GPBParser parser)
            : base("", parser.current_file_name, parser.parsertools.GetTokenSourceContext(parser.LRParser), (syntax_tree_node)parser.prev_node)
        {
            List<GoldParser.Symbol> Symbols = parser.parsertools.GetPrioritySymbols(parser.LRParser.GetExpectedTokens());
            if (Symbols.Count == 1)
            {
                string OneSybmolText = "ONE_" + Symbols[0].Name.ToUpper();
                string LocOneSybmolText = StringResources.Get(OneSybmolText);
                if (LocOneSybmolText != OneSybmolText)
                {
                    _message = string.Format(LocOneSybmolText);
                    return;
                }
            }
            _message = string.Format(StringResources.Get("EXPECTED{0}"), PascalABCCompiler.FormatTools.ObjectsToString(parser.parsertools.SymbolsToStrings(Symbols.ToArray()), ","));
        }
        public UnexpectedToken(string _file_name, string expected, SourceContext _source_context, syntax_tree_node _node)
            : base("", _file_name, _source_context, _node)
        {
            _message = string.Format(StringResources.Get("EXPECTED{0}"), expected);
        }
        public override string Message
        {
            get
            {
                return _message;
            }
        }


    }

}
