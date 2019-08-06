// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
//using com.calitha.goldparser;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Errors
{
    public static class ParserErrorsStringResources
    {
        public static string Get(string Id)
        {
            return PascalABCCompiler.StringResources.Get("PARSER_ERRORS_" + Id);
        }
    }

    public class UnexpectedNodeType : SyntaxError
    {
        public UnexpectedNodeType(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            : base(ParserErrorsStringResources.Get("UNEXPECTED_NODE_TYPE"), _file_name, _source_context, _node)
        {
        }
    }

    public class BadFloat : SyntaxError
    {
        public BadFloat(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            : base(ParserErrorsStringResources.Get("BAD_FLOAT"), _file_name, _source_context, _node)
        {
        }
    }
    public class BadInt : SyntaxError
    {
        public BadInt(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            : base(ParserErrorsStringResources.Get("BAD_INT"), _file_name, _source_context, _node)
        {
        }
    }
    public class BadHex : SyntaxError
    {
        public BadHex(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            : base(ParserErrorsStringResources.Get("BAD_HEX"), _file_name, _source_context, _node)
        {
        }
    }
    public class BadExpr : SyntaxError
    {
        public BadExpr(string _file_name, SourceContext _source_context, syntax_tree_node _node)
            : base(ParserErrorsStringResources.Get("BAD_EXPR"), _file_name, _source_context, _node)
        {
        }
    }
    public class TooBigCharNumberInSharpCharConstant: SyntaxError
    {
        public TooBigCharNumberInSharpCharConstant(string _file_name,  SourceContext _source_context, syntax_tree_node _node)
            : base(ParserErrorsStringResources.Get("BIG_CHAR_NUMBER"), _file_name, _source_context, _node)
        {
        }
    }
    public class ParserBadFileExtension : LocatedError
    {
        public ParserBadFileExtension(string FileName)
            : base(String.Format(ParserErrorsStringResources.Get("BAD_SOURCEFILE{0}_EXT"), System.IO.Path.GetFileName(FileName)), FileName)
        {
        }
        public override string ToString()
        {
            return Message;
        }
    }	
}
