// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;

namespace PascalABCCompiler.Errors
{
    public class bad_operand_type: SyntaxError
	{
		public bad_operand_type(string _file_name,SourceContext _source_context,syntax_tree_node _node):
            base(PascalABCSavParser.StringResources.Get("OPERATOR_NOT_IMPLEMET_TO_THIS_OPEAND_TYPE"), _file_name, _source_context, _node)
		{
		}
	}
    public class statement_expected : SyntaxError
	{
		public statement_expected(string _file_name,SourceContext _source_context,syntax_tree_node _node):
            base(PascalABCSavParser.StringResources.Get("STATEMENT_EXPECTED"), _file_name, _source_context, _node)
		{
		}
		
	}
    public class bad_leftside_assigment : SyntaxError
    {
        public bad_leftside_assigment(string _file_name, SourceContext _source_context, syntax_tree_node _node) :
            base(PascalABCSavParser.StringResources.Get("LEFT_SIDE_ASSIGN_MUST_BE_VARIABLE"), _file_name, _source_context, _node)
        {
        }
    }
    public class anon_type_duplicate_name : SyntaxError
    {
        public anon_type_duplicate_name(string _file_name, SourceContext _source_context, syntax_tree_node _node) :
            base(PascalABCSavParser.StringResources.Get("DUPLICATE_NAMES_IN_ANON_TYPE"), _file_name, _source_context, _node)
        {
        }
    }
    public class nonterminal_token_return_null : SyntaxError
	{
		public nonterminal_token_return_null(string _file_name,SourceContext _source_context,syntax_tree_node _node,string nonterminal_token_name):
            base(string.Format(PascalABCSavParser.StringResources.Get("NONTERMINALTOKEN_{0}_RETURN_NULL"), nonterminal_token_name),
				_file_name,_source_context,_node)
		{
		}
	}
    public class bad_anon_type : SyntaxError
    {
        public bad_anon_type(string _file_name, SourceContext _source_context, syntax_tree_node _node) :
            base(PascalABCSavParser.StringResources.Get("BAD_ANON_TYPE"), _file_name, _source_context, _node)
        {
        }
    }
    public class unexpected_return_value : SyntaxError
    {
        public unexpected_return_value(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            : base(PascalABCSavParser.StringResources.Get("PROCEDURE_CANNOT_HAVE_RETURNED_VALUE"), _file_name, _source_context, _node)
        { 
        }
    }
    public class unexpected_ident : SyntaxError
    {
        public unexpected_ident(string _file_name,ident unexpected_id,string expected,SourceContext _source_context, syntax_tree_node _node)
            : base(string.Format(PascalABCSavParser.StringResources.Get("EXPECTED_{0}_BUT_FOUND_{1}"), expected, unexpected_id.name), _file_name, _source_context, _node)
        {
        }
    }
    public class PABCNETUnexpectedToken : SyntaxError
    {
        private string _message;
        public PABCNETUnexpectedToken(string _file_name, string expected, SourceContext _source_context, syntax_tree_node _node)
            : base("", _file_name, _source_context, _node)
        {
            _message = string.Format(PascalABCSavParser.StringResources.Get("EXPECTED{0}"), expected);
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
