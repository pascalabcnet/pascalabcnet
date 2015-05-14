using System;
//using com.calitha.goldparser;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.CParser;
using PascalABCCompiler.ParserTools;
using PascalABCCompiler.Errors;
using System.Collections.Generic;


namespace PascalABCCompiler.CPreprocessor.Errors
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

    public class IncludeFileNotFound : SyntaxError
    {
        public IncludeFileNotFound(string _file_name, SourceContext _source_context, syntax_tree_node _node, string FileName)
            :
            base(String.Format(StringResources.Get("INCLUDE_FILE_{0}_NOTFOUND"), FileName), _file_name, _source_context, _node)
        {
        }
    }

    public class InvalidIntegerExpression : SyntaxError
    {
        public InvalidIntegerExpression(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("INVALID_INTEGER_EXPRESSION"), _file_name, _source_context, null)
        {
        }
    }

    public class CircularInclude : SyntaxError
    {
        public CircularInclude(string _file_name, SourceContext _source_context, syntax_tree_node _node, string FileName)
            :
            base(String.Format(StringResources.Get("CIRCULAR_INCLUDE_{0}"), FileName), _file_name, _source_context, _node)
        {
        }
    }
    public class ErrorInDefineSyntax : SyntaxError
    {
        public ErrorInDefineSyntax(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("ERROR_IN_DEFINE_SYNTAX"), _file_name, _source_context, null)
        {
        }
    }
    public class ErrorDirective : SyntaxError
    {
        public ErrorDirective(string _file_name, SourceContext _source_context, string text)
            :
            base(String.Format(StringResources.Get("ERROR_DIRECTIVE_{0}"),text), _file_name, _source_context, null)
        {
        }
    }
    public class ErrorInIncludeSyntax : SyntaxError
    {
        public ErrorInIncludeSyntax(string _file_name, SourceContext _source_context)
            :
            base(StringResources.Get("ERROR_IN_INCLUDE_SYNTAX"), _file_name, _source_context, null)
        {
        }
    }
    public class InvalidPreprocessorCommand : SyntaxError
    {
        public InvalidPreprocessorCommand(string _file_name, SourceContext _source_context, string command)
            :
            base(String.Format(StringResources.Get("INVALID_PREPROCESSOR_COMMAND_{0}"),command), _file_name, _source_context, null)
        {
        }
    }
}
