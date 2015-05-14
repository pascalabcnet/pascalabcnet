using System;
//using com.calitha.goldparser;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.PascalABCParser;
using PascalABCCompiler.TestParserTools;
using PascalABCCompiler.Errors;
using System.Collections.Generic;
using PascalABCCompiler.TestGoldParserEngine;


namespace PascalABCCompiler.PascalABCParser.Errors
{
    public class bad_operand_type: SyntaxError
	{
		public bad_operand_type(string _file_name,SourceContext _source_context,syntax_tree_node _node):
            base(StringResources.Get("OPERATOR_NOT_IMPLEMET_TO_THIS_OPEAND_TYPE"), _file_name, _source_context, _node)
		{
		}
	}
    public class statement_expected : SyntaxError
	{
		public statement_expected(string _file_name,SourceContext _source_context,syntax_tree_node _node):
            base(StringResources.Get("STATEMENT_EXPECTED"), _file_name, _source_context, _node)
		{
		}
		
	}
    public class bad_leftside_assigment : SyntaxError
	{
		public bad_leftside_assigment(string _file_name,SourceContext _source_context,syntax_tree_node _node):
            base(StringResources.Get("LEFT_SIDE_ASSIGN_MUST_BE_VARIABLE"), _file_name, _source_context, _node)
		{
		}
	}
    public class nonterminal_token_return_null : SyntaxError
	{
		public nonterminal_token_return_null(string _file_name,SourceContext _source_context,syntax_tree_node _node,string nonterminal_token_name):
            base(string.Format(StringResources.Get("NONTERMINALTOKEN_{0}_RETURN_NULL"), nonterminal_token_name),
				_file_name,_source_context,_node)
		{
		}
	}
    public class unexpected_return_value : SyntaxError
    {
        public unexpected_return_value(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            : base(StringResources.Get("PROCEDURE_CANNOT_HAVE_RETURNED_VALUE"), _file_name, _source_context, _node)
        { 
        }
    }
    public class unexpected_ident : SyntaxError
    {
        public unexpected_ident(string _file_name,ident unexpected_id,string expected,SourceContext _source_context, syntax_tree_node _node)
            : base(string.Format(StringResources.Get("EXPECTED_{0}_BUT_FOUND_{1}"), expected, unexpected_id.name), _file_name, _source_context, _node)
        {
        }
    }
    public class PABCNETUnexpectedToken : SyntaxError
    {
        private string _message;
        public Symbol error_token = null;//tasha(24.08.08)
        public PABCNETUnexpectedToken(TestParserTools.GPBParser parser)
            : base("", parser.current_file_name, parser.parsertools.GetTokenSourceContext(parser.LRParser), (syntax_tree_node)parser.prev_node)
        {
            List<Symbol> Symbols = parser.parsertools.GetPrioritySymbols(parser.LRParser.GetExpectedTokens());
            if (Symbols.Count == 1)
            {
                error_token = Symbols[0];//tasha
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
        public PABCNETUnexpectedToken(string _file_name, string expected, SourceContext _source_context, syntax_tree_node _node)
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
