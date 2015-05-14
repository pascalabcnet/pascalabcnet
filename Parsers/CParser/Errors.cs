using System;
//using com.calitha.goldparser;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.CParser;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;
using System.Collections.Generic;


namespace PascalABCCompiler.CParser.Errors
{

    public class FunctionHeaderExpected : SyntaxError
    {
        public FunctionHeaderExpected(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("FUNCTION_HEADER_EXPECTED"), _file_name, _source_context, _node)
        {
        }
    }

    public class UnexpectedPartOfFunctionHeader : SyntaxError
    {
        public UnexpectedPartOfFunctionHeader(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("UNEXPECTED_PART_OF_FUNCTION_HEADER"), _file_name, _source_context, _node)
        {
        }
    }

    public class FunctionReturnFuction : SyntaxError
    {
        public FunctionReturnFuction(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("FUNCTION_RETURN_FUNCTION"), _file_name, _source_context, _node)
        {
        }
    }

    public class BadDeclaration : SyntaxError
    {
        public BadDeclaration(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("BAD_DECLARATION"), _file_name, _source_context, _node)
        {
        }
    }

    public class IntExpected : SyntaxError
    {
        public IntExpected(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("INT_EXPECTED"), _file_name, _source_context, _node)
        {
        }
    }

    public class ShortOrLongExpected : SyntaxError
    {
        public ShortOrLongExpected(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("SHORT_OR_LONG_EXPECTED"), _file_name, _source_context, _node)
        {
        }
    }

    public class ScalarTypeNameExpected : SyntaxError
    {
        public ScalarTypeNameExpected(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("SCALAR_TYPE_NAME_EXPECTED"), _file_name, _source_context, _node)
        {
        }
    }

    public class InitDeclaratorListExpected : SyntaxError
    {
        public InitDeclaratorListExpected(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("INIT_DECLARATOR_LIST_EXPECTED"), _file_name, _source_context, _node)
        {
        }
    }

    public class BadCharConst : SyntaxError
    {
        public BadCharConst(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("BAD_CHAR_CONST"), _file_name, _source_context, _node)
        {
        }
    }

    public class MissingTypeSpecifier : SyntaxError
    {
        public MissingTypeSpecifier(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            :
            base(StringResources.Get("MISSING_TYPE_SPECIFIER"), _file_name, _source_context, _node)
        {
        }
    }


    public class bad_operand_type : SyntaxError
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
        public PABCNETUnexpectedToken(PascalABCCompiler.ParserTools.GPBParser parser)
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
        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }

}
